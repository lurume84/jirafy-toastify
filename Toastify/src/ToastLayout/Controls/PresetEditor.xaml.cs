using System.Collections.ObjectModel;
using System.Windows;

namespace Toastify.ToastLayout.Controls
{
    public partial class PresetEditor
    {
        public ObservableCollection<BaseRowEditor> Rows { get; set; }

        public PresetEditor()
        {
            AddNewRowItem addNewRowItem = new AddNewRowItem();
            addNewRowItem.BtnAddClicked += this.AddNewToastRowItem_BtnAddClicked;
            this.Rows = new ObservableCollection<BaseRowEditor> { addNewRowItem };

            this.InitializeComponent();
            this.DataContext = this;
        }

        private void AddNewToastRowItem_BtnAddClicked(object sender, RoutedEventArgs e)
        {
            this.Rows.Insert(this.Rows.Count - 1, new RowEditor());
        }
    }
}