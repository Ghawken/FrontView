# Yatse2-1
YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)

An update to Yatse2 (Tolriq) Which is no longer supported.

See Forum for uptodate info.
http://forum.kodi.tv/showthread.php?tid=68936&page=54

Hi (slightly more than one!)

Pictures talk more than words!


[img]http://i375.photobucket.com/albums/oo191/glennnz/Fanart%20Socket%20Example_zpsqldve04d.png[/img]

[img]http://i375.photobucket.com/albums/oo191/glennnz/Fanart%20Socket%20Example%20TV_zpsquviyfe3.png[/img]

[img]http://i375.photobucket.com/albums/oo191/glennnz/Fanart%20Socket%20Example%20Movie%202_zpsppbzdxjb.png[/img]



Binaries to install Windows 32 - see here:
https://github.com/Ghawken/Yatse2-1/releases/tag/Socket

Okay.
Will update with more instructions when can.

But have made addition  to Yatse2 with addition of socket to kodi data connection, allowing Yatse2 to be informed about browsing menu choice/tv show etc - so fanart can be selected as required. 

Uses extrafanart directories from within media folder structures.
NB:  needs artwork.downloader to download fanart for TV and movies to extrafanart directory within media folder.  

eg.
TVShowRoot \ TV Show \ extrafanart \ (fanart here - via artwork.downloader addon)
MovieRoot \ Movie Name \ extrafanart \

Needs - service.Yatse2 to be added to %appdata%\Kodi\addons directory.  Copy the directory and paste the directory.
Then go to Kodi, add-ons, Service and configure/enable Yatse2 service.
Defaults to localIP and port 5000 (these can be changed if needed but hopefully not needed too)
This service connects to Yatse2 in the background. Fairly robust connection/very low overhead.

New Yatse2 Socket version of application. (Has a few other fixes/checks/logging)
NB: Still needs normal remote to Kodi connection configured as prior, but also the socket settings.  
Defaults as above:
But can be change in the Yatse2.xml file below:
  [code]<IPPort>5000</IPPort>
  <IPAddress>127.0.0.1</IPAddress>
[/code]

Also New Setting for depth of directories before extrafanart folder: (somewhat hard to explain but used to deal with some TV shows having season1/2 like deeper directories) yatse2socket only uses the (set) number of directories truncating the rest.
[code]
  <FanartNumberDirectories>3</FanartNumberDirectories>
[/code]

eg.
If Source root is \\server\tvshows
Path to extrafanart - \\server\tvshows\showname\extrafanart
Set this to 3
(If browse to \\server\tvshows\showname\season 1 - only the first 3 directories will be used with extrafanart then added - giving \\server\tvshows\showname\extrafanart )

Eg. And movies
\\server\Movies\The Matrix\extrafanart

Presently same depth setting is used for both Movie and TV show structure (suits me - but probably can change)
Used to enable ongoing fanart even when browsing deeper - e.g Season1/Season2 separate directories.

Also when browsing image folders - Yatse2 will start slideshow of selected image folder that you are browsing if correct permissions set for access.

Issues:
Works well with smb:// file directory structure in kodi database - which is my setup. 
If only local files or other network connection may not correctly function.
If have this issue:
Let me know: " Fanart Directory from Socket" log message.
eg msg like this in log if debug and trace enabled
'Fanart Directory from Socket = smb://192.168.1.110/tvss/Silicon Valley/'

Also Debug and Trace enabled - turn off once working!

Glenn















Hi

Welcome to the newer Yatse2

For those not familiar Yatse2 was created as a remote and 'nowplaying' info service for Xbmc and now Kodi.

It was created by Tolriq who has continued with Android remote control called Yatse.

Yatse2 fulls a huge need of info on screen screen TFT for those with HTPC cases that include a second TFT/Monitor.
(Note this differs from LCD/VFD displays which are small one or two line single color devices, there is ongoing support for these through other software.  Not of the VFD/LCD software supports a TFT/Full screen monitor like Yatse)

I've made some minor modifications to the original code
The Binaries are included in the release - sorry no install
Just copy the Program File and run (does need .net installed)

Most Settings are available through the Yatse2.xml file:
Should be straightforward.

My changes
- Undims on Pause
- MinimiseAlways Setting - in yatse2.xml file
If enable will always minimise Yatse2 to taskbar if not playing.
Ideal if wish to use Yatse2 for nowplaying information - but would like other information at other times.
ie. webpage, other software etc.
If double click taskbar icon - MinimiseAlways is disabled to enable settings to be altered.
If double click again - and Yatse2 shrinks MinimiseAlways is reenabled.
- Minor bug fixes - minimising how many times it checks for dim/undim



- Fanart
Moderated change to JSON and Display
If like me you use your displaying only to look pretty and not as a remote - the default screen was a bit average
I have modifed the JSON code to let Yatse2 know what menu is being browsed by the user and to play fanart accordingly.
Unfortunately I was hopeful to distinguish between TV and Movie menus but I believe this is not possible currently through JSON.

None-the-less
New Setting: Fanart - Enable
This will when not playing always play a slide show of your fanart.
The Directories are selected through the Yatse2.xml file as below.
The must be within %appdata%\Kodi\userdata

eg. as below

  <FanartDirectoryTV>addon_data\script.artworkorganizer\TVShowFanart\</FanartDirectoryTV>
  <FanartDirectoryMovie>addon_data\script.artworkorganizer\ArtistFanart\</FanartDirectoryMovie>
  <FanartDirectoryWeather>addon_data\skin.aeonmq5.extrapack\backgrounds_weather\</FanartDirectoryWeather>
  <FanartDirectoryMyImages>addon_data\script.artworkorganizer\OwnFanart\</FanartDirectoryMyImages>
  <FanartDirectoryMusic>addon_data\script.artworkorganizer\ArtistFanart\</FanartDirectoryMusic>
  
 FanartDirectoryTV - Defaults when on main menu
 FanartDirectoryMovie - Defaults when browswing Video folder, unfortunately also TV
 FanartDirectoryWeather - Weather images when in weather menu
 FanartDirectoryMyImages - if in pictures - show these images
 FanartDirectoryMusic - if browsing Music
 
 If no images in a directory - will default to control screen.
 Also if browsing settings menu will default to control screen if need access to change or disable settings.
 
 To disable Fanart - go to Settings menu on Kodi - enter - Fanart will stop, and can be disabled from Settings within Yatse2
 
