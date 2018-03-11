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
* Download the latest release of Toastify from <a id="inst-latest" href="{{ site.github.repository_url }}/releases/latest">here</a>.
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


[//]: # (Links)

[toastify@github]: <{{ site.github.repository_url }}>
[toastify-showcase]: <https://raw.githubusercontent.com/{{ site.repository }}/gh-pages/images/toastify-showcase.png>
[issues]: <{{ site.github.repository_url }}/issues>
[SpotifyAPI-NET]: <https://github.com/JohnnyCrazy/SpotifyAPI-NET>
