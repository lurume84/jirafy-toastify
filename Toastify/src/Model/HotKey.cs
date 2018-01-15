﻿using log4net;
using log4net.Util;
using ManagedWinapi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Toastify.Common;
using Toastify.Core;
using Toastify.Events;
using Toastify.Helpers;
using Toastify.Services;
using Toastify.View;
using Clipboard = System.Windows.Clipboard;

namespace Toastify.Model
{
    [Serializable]
    [XmlRoot("Hotkey")]
    public class Hotkey : ObservableObject, IXmlSerializable, ICloneable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Hotkey));

        #region Private fields

        private ToastifyAction _action;
        private bool _enabled;
        private bool _shift;
        private bool _ctrl;
        private bool _alt;
        private bool _windowsKey;
        private Key _key;

        private bool _isValid;
        private string _invalidReason;

        private bool _active;
        private ManagedWinapi.Hotkey _globalKey;

        #endregion Private fields

        #region Public properties

        public ToastifyAction Action
        {
            get
            {
                return this._action;
            }
            set
            {
                if (this._action != value)
                {
                    this._action = value;
                    this.NotifyPropertyChanged(nameof(this.Action), false);
                }
            }
        }

        /// <summary>
        /// Specifies whether or not the hotkey is enabled from a user's perspective.
        /// Does not actually register the hotkey, use Activate() and Deactivate().
        /// </summary>
        /// <remarks>
        /// Why do we have these two properties? We need a way to be able to deactivate a
        /// Hotkey (for example when unloading settings) without changing the Enabled
        /// property (which only indicates the user's preference)
        /// </remarks>
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                if (this._enabled != value)
                {
                    this._enabled = value;
                    this.NotifyPropertyChanged(nameof(this.Enabled), false);
                }
            }
        }

        public bool Shift
        {
            get
            {
                return this._shift;
            }
            set
            {
                if (this._shift != value)
                {
                    this._shift = value;
                    this.NotifyPropertyChanged(nameof(this.Shift));
                }
            }
        }

        public bool Ctrl
        {
            get
            {
                return this._ctrl;
            }
            set
            {
                if (this._ctrl != value)
                {
                    this._ctrl = value;
                    this.NotifyPropertyChanged(nameof(this.Ctrl));
                }
            }
        }

        public bool Alt
        {
            get
            {
                return this._alt;
            }
            set
            {
                if (this._alt != value)
                {
                    this._alt = value;
                    this.NotifyPropertyChanged(nameof(this.Alt));
                }
            }
        }

        public bool WindowsKey
        {
            get
            {
                return this._windowsKey;
            }
            set
            {
                if (this._windowsKey != value)
                {
                    this._windowsKey = value;
                    this.NotifyPropertyChanged(nameof(this.WindowsKey));
                }
            }
        }

        public Key Key
        {
            get
            {
                return this._key;
            }
            set
            {
                if (this._key != value)
                {
                    this._key = value;
                    this.NotifyPropertyChanged(nameof(this.Key));
                }
            }
        }

        [XmlIgnore]
        public string HumanReadableKey
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (this.Shift) sb.Append("Shift+");
                if (this.Ctrl) sb.Append("Ctrl+");
                if (this.Alt) sb.Append("Alt+");
                if (this.WindowsKey) sb.Append("Win+");
                sb.Append(this.Key);
                return sb.ToString();
            }
        }

        [XmlIgnore]
        public string HumanReadableAction
        {
            get { return this.Action.GetReadableName(); }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
            set
            {
                if (this._isValid != value)
                {
                    this._isValid = value;
                    this.NotifyPropertyChanged(nameof(this.IsValid), false);
                }
            }
        }

        [XmlIgnore]
        public string InvalidReason
        {
            get
            {
                return this._invalidReason;
            }
            set
            {
                if (this._invalidReason != value)
                {
                    this._invalidReason = value;
                    this.NotifyPropertyChanged(nameof(this.InvalidReason), false);
                }
            }
        }

        [XmlIgnore]
        public bool Active
        {
            get
            {
                return this._active;
            }
            private set
            {
                if (this._active != value)
                {
                    this._active = value;
                    this.InitGlobalKey();
                }
            }
        }

        #endregion Public properties

        public object Clone()
        {
            Hotkey clone = (Hotkey)this.MemberwiseClone();

            // Regardless of whether or not the original hotkey was active,
            // the cloned one should not start in an active state.
            clone._active = false;

            return clone;
        }

        /// <summary>
        /// Turn this hotkey on. Does nothing if this Hotkey is not enabled
        /// </summary>
        public void Activate()
        {
            this.Active = true;
        }

        /// <summary>
        /// Turn this HotKey off.
        /// </summary>
        public void Deactivate()
        {
            this.Active = false;
        }

        private void InitGlobalKey()
        {
            if (this._globalKey != null)
                this._globalKey.HotkeyPressed -= this.GlobalKey_HotkeyPressed;

            // If we're not enabled shut everything down asap
            if (!this.Enabled || !this.Active)
            {
                if (this._globalKey != null)
                {
                    this._globalKey.Enabled = false;
                    this._globalKey = null; // may as well collect the memory
                }

                // May not be false if !Enabled
                this._active = false;

                return;
            }

            if (this._globalKey == null)
                this._globalKey = new ManagedWinapi.Hotkey();

            // Make sure that we don't try to re-register the key midway updating the combination.
            if (this._globalKey.Enabled)
                this._globalKey.Enabled = false;

            this._globalKey.Shift = this.Shift;
            this._globalKey.Ctrl = this.Ctrl;
            this._globalKey.Alt = this.Alt;
            this._globalKey.WindowsKey = this.WindowsKey;
            this._globalKey.KeyCode = ConvertInputKeyToFormsKeys(this.Key);

            this._globalKey.HotkeyPressed += this.GlobalKey_HotkeyPressed;

            try
            {
                this._globalKey.Enabled = true;
            }
            catch (HotkeyAlreadyInUseException)
            {
                this.IsValid = false;
                this.InvalidReason = "Hotkey is already in use by a different program";
            }
        }

        /// <summary>
        /// Validity rules are:
        ///
        /// 1. Ctrl or Alt must be selected
        /// 2. a key must be specified
        /// </summary>
        private void CheckIfValid()
        {
            if (this.Key == Key.None)
            {
                this.IsValid = false;
                this.InvalidReason = "You must select a valid key for your hotkey combination";
            }
            else if (this.Shift == false && this.Ctrl == false && this.Alt == false)
            {
                this.IsValid = false;
                this.InvalidReason = "At least one modifier key must be selected";
            }
            else
            {
                this.IsValid = true;
                this.InvalidReason = "";
            }
        }

        public override string ToString()
        {
            string prefix = $"{(this.Enabled ? 'E' : ' ')}{(this.Active ? 'A' : ' ')}";
            string keyCombination = $"{(this.Ctrl ? "Ctrl+" : "")}{(this.Shift ? "Shift+" : "")}{(this.Alt ? "Alt+" : "")}{(this.WindowsKey ? "Win+" : "")}{this.Key}";

            // ReSharper disable once UseStringInterpolation
            return $"{prefix} {this.Action,-15}: {keyCombination}";
        }

        private void GlobalKey_HotkeyPressed(object sender, EventArgs e)
        {
            HotkeyActionCallback(this);
        }

        #region HotkeyActionCallback

        public static event EventHandler<HotkeyActionCallbackFailedEventArgs> ActionCallbackFailed;

        public static Hotkey LastHotkey { get; private set; }

        public DateTime LastPressTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// If the same hotkey press happens within this buffer time, it will be ignored.
        ///
        /// I came to 150 by pressing keys as quickly as possibly. The minimum time was less than 150
        /// but most values fell in the 150 to 200 range for quick presses, so 150 seemed the most reasonable
        /// </summary>
        private const int WAIT_BETWEEN_HOTKEY_PRESS = 150;

        private static void HotkeyActionCallback(Hotkey hotkey)
        {
            if (!ToastView.Current.IsInitComplete)
                return;

            // Ignore this keypress if it's been less than WAIT_BETWEEN_HOTKEY_PRESS since the last press
            if (DateTime.Now.Subtract(hotkey.LastPressTime).TotalMilliseconds < WAIT_BETWEEN_HOTKEY_PRESS)
                return;

            hotkey.LastPressTime = DateTime.Now;
            LastHotkey = hotkey;

            logger.DebugExt($"HotkeyActionCallback: {hotkey.Action}");

            try
            {
#if DEBUG
                if (hotkey.Action == ToastifyAction.ShowDebugView && DebugView.Current == null)
                    DebugView.Launch();
#endif

                if (hotkey.Action == ToastifyAction.CopyTrackInfo && Spotify.Instance.CurrentSong != null)
                {
                    Analytics.TrackEvent(Analytics.ToastifyEventCategory.Action, Analytics.ToastifyEvent.Action.CopyTrackInfo);
                    Clipboard.SetText(Spotify.Instance.CurrentSong.GetClipboardText(Settings.Current.ClipboardTemplate));
                }
                else if (hotkey.Action == ToastifyAction.PasteTrackInfo && Spotify.Instance.CurrentSong != null)
                {
                    Analytics.TrackEvent(Analytics.ToastifyEventCategory.Action, Analytics.ToastifyEvent.Action.PasteTrackInfo);
                    Clipboard.SetText(Spotify.Instance.CurrentSong.GetClipboardText(Settings.Current.ClipboardTemplate));
                    Win32API.SendPasteKey();
                }
                else
                    Spotify.Instance.SendAction(hotkey.Action);

                ToastView.Current.DisplayAction(hotkey.Action);
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();

                logger.ErrorExt("Exception with hooked key.", ex);
                ActionCallbackFailed?.Invoke(typeof(Hotkey), new HotkeyActionCallbackFailedEventArgs(hotkey, ex));

                Analytics.TrackException(ex);
            }
        }

        #endregion HotkeyActionCallback

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged(string info, bool checkIfValid = true)
        {
            if (checkIfValid)
                this.CheckIfValid();
            base.NotifyPropertyChanged(info);
        }

        #endregion INotifyPropertyChanged

        #region IXmlSerializable

        private enum Attribute
        {
            Null = 0,
            Enabled,
            Shift,
            Ctrl,
            Alt,
            WindowsKey,
            Key,
            Action
        }

        private static readonly Dictionary<Attribute, List<string>> nodeNames = new Dictionary<Attribute, List<string>>
        {
            { Attribute.Enabled,    new List<string> { "Enabled" } },
            { Attribute.Shift,      new List<string> { "Shift" } },
            { Attribute.Ctrl,       new List<string> { "Ctrl" } },
            { Attribute.Alt,        new List<string> { "Alt" } },
            { Attribute.WindowsKey, new List<string> { "WindowsKey", "Win", "WinKey" } },
            { Attribute.Key,        new List<string> { "Key" } },
            { Attribute.Action,     new List<string> { "Action" } }
        };

        /// <inheritdoc />
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement && !reader.HasAttributes)
                return;

            try
            {
                Dictionary<Attribute, bool> deserializedFlags = new Dictionary<Attribute, bool>();

                // First, try to read the attributes of the 'Hotkey' node.
                if (reader.MoveToFirstAttribute())
                {
                    do
                    {
                        string name = reader.LocalName;
                        string value = reader.Value;
                        this.ParseNode(name, value, deserializedFlags);
                    } while (reader.MoveToNextAttribute());

                    reader.MoveToElement();
                }

                // Then, try to read the child nodes (for compatibility with the old settings file).
                if (!reader.IsEmptyElement)
                {
                    reader.Read();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        string name = reader.LocalName;
                        string value = reader.ReadElementContentAsString();
                        this.ParseNode(name, value, deserializedFlags);
                    }
                }
            }
            finally
            {
                if (!reader.IsEmptyElement)
                    reader.ReadEndElement();
                else
                    reader.Skip();
            }
        }

        /// <inheritdoc />
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(nodeNames[Attribute.Action].First(), this.Action.ToString());
            writer.WriteAttributeString(nodeNames[Attribute.Enabled].First(), this.Enabled.ToString());
            writer.WriteAttributeString(nodeNames[Attribute.Shift].First(), this.Shift.ToString());
            writer.WriteAttributeString(nodeNames[Attribute.Ctrl].First(), this.Ctrl.ToString());
            writer.WriteAttributeString(nodeNames[Attribute.Alt].First(), this.Alt.ToString());
            writer.WriteAttributeString(nodeNames[Attribute.WindowsKey].First(), this.WindowsKey.ToString());
            writer.WriteAttributeString(nodeNames[Attribute.Key].First(), this.Key.ToString());
        }

        private void ParseNode(string localName, string value, IDictionary<Attribute, bool> deserializedFlags)
        {
            Attribute hotkeyAttribute = nodeNames
                .FirstOrDefault(kvp => kvp.Value.Contains(localName, StringComparer.InvariantCultureIgnoreCase))
                .Key;

            // The current node name does not match any of the known names.
            if (hotkeyAttribute == Attribute.Null)
                return;

            // The current node is a duplicate.
            if (deserializedFlags.ContainsKey(hotkeyAttribute) && deserializedFlags[hotkeyAttribute])
                return;

            // Parse the value.
            this.ParseNodeValue(hotkeyAttribute, value, deserializedFlags);
        }

        private void ParseNodeValue(Attribute attribute, string value, IDictionary<Attribute, bool> deserializedFlags)
        {
            if (deserializedFlags.ContainsKey(attribute))
                deserializedFlags[attribute] = true;

            switch (attribute)
            {
                case Attribute.Enabled:
                    this.Enabled = bool.Parse(value);
                    break;

                case Attribute.Shift:
                    this.Shift = bool.Parse(value);
                    break;

                case Attribute.Ctrl:
                    this.Ctrl = bool.Parse(value);
                    break;

                case Attribute.Alt:
                    this.Alt = bool.Parse(value);
                    break;

                case Attribute.WindowsKey:
                    this.WindowsKey = bool.Parse(value);
                    break;

                case Attribute.Key:
                    this.Key = (Key)Enum.Parse(typeof(Key), value);
                    break;

                case Attribute.Action:
                    ToastifyAction action;
                    if (!Enum.TryParse(value, true, out action))
                        action = ToastifyAction.None;
                    this.Action = action;
                    break;

                default:
                    return;
            }
        }

        #endregion IXmlSerializable

        #region Static functions

        private static Keys ConvertInputKeyToFormsKeys(Key key)
        {
            if (Enum.GetNames(typeof(Keys)).Contains(key.ToString()))
                return (Keys)Enum.Parse(typeof(Keys), key.ToString());

            return Keys.None;
        }

        #endregion Static functions
    }
}