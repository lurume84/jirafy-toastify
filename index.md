![toastify-showcase][toastify-showcase]

Toastify adds global hotkeys and toast notifications to Spotify.

This application uses [SpotifyAPI-NET][SpotifyAPI-NET].

## Features
* Display the current playing track in a customizable toast-like popup
* Global hotkeys for media actions (Play/Pause, Next/Previous track, Volume Up/Down, Mute, Seek Forward/Backward)
* :heavy_exclamation_mark: Compatible with the Microsoft Store version of Spotify

## Requirements
* Windows (7, 8/8.1, 10)
* .NET Framework 4.5.*
* Spotify

## Installation
* Download the latest release of Toastify from <a id="inst-latest" href="{{ site.github.repository_url | append: '/releases/latest' }}">here</a>.
* Run "*ToastifyInstaller.exe*" to install Toastify.

**Note**: If you are updating from the original version of Toastify (v1.8.3), be sure to uninstall it completely before installing this version. You should also remove any file from the following directories, if they exist:
- "*%LocalAppData%\Toastify*" (i.e. "*C:\Users\\{UserName}\AppData\Local\Toastify*")
- "*%AppData%\Toastify*" (i.e. "*C:\Users\\{UserName}\AppData\Roaming\Toastify*")

## Notices
* Toastify is **not** a Spotify **replacement**. You need both running at the same time.
* **Windows 10**: In the latest versions of Windows, SmartScreen will probably prevent the installer from starting, because it is not signed.  
  You have two options: either *Don't run* the installer and forget about Toastify, or click on *More info* and *Run anyway*.  
  Whatever is your choice, I strongly advise you to **not** disable Windows SmartScreen — its warnings are meaningful.  

  If you have any concerns, feel free to take a look at the [code][toastify@github] (it's free :wink:) and ask questions in the [issues][issues] section.

## Donations
I dedicate most of my free time to improving and keeping Toastify alive. Although absolutely not necessary, if you'd like to support me and the project, you can buy me a coffee! :coffee:

<div id="donation-links-container">
  <div id="donation-kofi">
    <script type="text/javascript" src="https://ko-fi.com/widgets/widget_2.js"></script>
    <script type="text/javascript">kofiwidget2.init('Buy Me a Coffee', '#46b798', 'S6S8AH23');kofiwidget2.draw();</script>
  </div>
  
  <span class="unselectable">OR</span>
  
  <div id="donation-paypal">
    <form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_blank">
      <input type="hidden" name="cmd" value="_s-xclick">
      <input type="hidden" name="hosted_button_id" value="NEXYAS3KL2AH8">
      <input type="image" src="https://vgy.me/MZ6Iea.png" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!" title="Support Toastify through PayPal">
      <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1" style="display: none !important;">
    </form>
  </div>
</div>

[//]: # (Links)

[toastify@github]: <{{ site.github.repository_url }}>
[toastify-showcase]: <{{ '/img/toastify-showcase.png' | relative_url }}>
[issues]: <{{ site.github.repository_url | append: '/issues' }}>
[SpotifyAPI-NET]: <https://github.com/JohnnyCrazy/SpotifyAPI-NET>
