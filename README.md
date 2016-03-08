# FrontView
New Name - fork of Yatse2

![Alt text](http://i68.tinypic.com/2jayyc6.png)

FrontView+ is an update and a new name for Yatse2 – Yatse2 was a program written by Tolriq for use on Home Theatre Windows Based PCs for Second LCD Screen information.  (see original thread here http://forum.kodi.tv/showthread.php?tid=68936&page=55 )

FrontView+ is an update on this older code (to .Net 4.5.1) adding further features and additional remote code to support Plex and Emby, with two additions for improved functionality with both Kodi and Emby.  (Kodi service and Emby Plugin)

There is no question that FrontView+ would not exist without Yatse and Tolriq code – many thanks for the open source nature of this code. FrontView+ remains open source with the code on Github.com

Basically if you have a PC case with a build-in LCD Screen eg. like these two:
![Alt text](http://i65.tinypic.com/2s80004.png)



**This** is the program to be running for your media-center. 
Nothing else in my experience does the job as well.
FrontView+ takes this blank screen, or the very old now unsupported iMon software and turns it into this:

![Alt text](http://i63.tinypic.com/2i8gd5g.jpg)

USAGE



*Use your second LCD Screen for Now Playing Information with fanart/Backdrops and remote control

*Touch Screen Remote control (if player supports)

*Shows Fanart as you browse Media Center – Fanart updates depending on the Browsing option (Kodi only – needs kodi.service installed)

*Shows Default Fanart on other media centers and if no extrafanart for selection

*Supports Fanart and CdART for Music Playback for all media centers

*Supports PVR Channel info 

*Supports sending of HTTP Commands at certain Playback events – enabling FrontView+ to trigger home automation commands

*Supports MPC-HC as external Player for Kodi usage

*Includes its own local SQL Database for browsing and starting playback of files

*Supports Kodi (full support – requires service.addon), Emby (full support – needs Emby Server Plugin), Plex (partial now Playing support only)

]

Relevant Fanart Example:

https://youtu.be/Io0AQPlM94E

Install Manual included in Setup file - for install instructions.
Manual Only:

https://www.mediafire.com/?v8232d1ww2166bb

**Update:**
Download from Mediafire: Link Below:
http://www.mediafire.com/download/lejmjwaz9dbbx2w/FrontView_Setup_1.107.exe

**Version 1.107**
- Reinstate DisableResolutionDetection: -- setting in settings.xml file
reinstates screen settings on screen change if <MinimiseAlways>false and <DisableResolutionDetection>false
if <DisableResolutionDetection>true - will resort to current behaviour where screen changes are ignored.

- Multiple Fixes for browsing fanart if changing from Kodi to Emby and back again 
KODI:
- Fix/Ignore theme.mp3 files - no nowplaying info for these files and keeps relevant fanart going -
(difficult to sort out but done I believe -Issue was keeping relevant Fanart going despite the playback event and loss of Path information)
Further Fixes for config File

**Version 1.105**
Bugfix for HTTPSend App exit related to name change
EMBY REMOTE: 
-	Add ignore theme.mp3 files playback for NowPlaying screen for EMBY Remote (?maybe needs option - can't think why)
-	And ignore backdrop/theme.mp4 files
-	Add support for 'Video' Mediatypes in NowPlaying screen
-	Updated EmbySever Plugin to 1.0.0.18/19 Classes
Fix Longstanding bug Fade In/Fade Out on Mute

**Update**
to Emby-Server Plugin to version 1.0.0.16:
Fixes Jquery issues on pageload/current selected item always shown correctly
Adds Escaping Foreign Characters as needed (customisable file FrontViewForeignCharacters.txt in plugins/configuration directory)

**FrontView 1.104**
- More Light Weight checking for Server Running/Shut down/Restart
- Remove Remote Control Buttons if Client does not support remote Control

**Release FrontView+ 1.102**
Bug Fix - EMBY - allow server restart without FrontView Freeze until completed.

**Release 1.101**
Bug Fix - for EventClient Emby Remote
Add Support for Kodi 16 Jarvis (name change only needed)

**FrontView 1.100**
- Adds Full Emby and Plex (simple support)
- Not major changes to other code base (other than few error fixes) if not using these remotes .


Setup:

EMBY:

Need to install FrontView+ Plugin for EMBY - simple dll which goes into EmbyServer plugin directory
(this provides ability to select Device controlling with Yatse and also provides additional Apiendpoints to streamline Yatse3 interaction with Emby Server)
It will no longer work without this plugin.
File:MediaBrowser.Plugins.Yatse.dll  - latest version 1.0.0.19
Copies to Emby Server.  On Emby Catalog/MetaData section.
%appdata%\Emby-Server\plugins

Usage:
Within FrontView+ 
Setup new Remote
Select Emby as Type from remote screen.
Type in server address, (usual) port 8096
Emby Username and/or Password - click connect/verify save.

Functioning:
Remote control - only if EMBY PlaybackClient supports remoteControl - NB new Theatre does not as yet
Updates FrontView+ database - for TV/Movies/Music and playback functions correctly
Now Browsing Fanart - only generic fanart supported - Emby server provides very little browsing or NowViewing information
NowPlaying Screen, Music/TV/Movies, Fanart functioning.

*---OLD Yatse3Socket Prior BUILD Changelog Below ---*

*Build 180*

- Extra HTTPSend settings screen
- Changes to capture Power On (easy to do) and Power Off (marginally harder)  Tried to capture all power down events.
- Some Logic changes to empty URLs and malformed URLs

*Build 175*

Added Variable Support to HttpSend - List of Variables in Settings page
Enables creation of url with filename, titles, plot, progress, time etc to be sent.
To home automation, or internet, or update database online etc.etc.

*Usage: (indigo example)
http://192.168.1.6:8176/variables/title?_method=put&value=%HTTPTITLE%
also can be used multiple times
http://192.168.1.6:8176/variables/title?_method=put&value=Episode%20number%20is%20%HTTPEPISODENO%and%20plot%20%HTTPPLOT%
etc.
Should try to keep url formatting (replace space with %20)  Should escape it out, but won't hurt*


*Build 170*

Major Addition HTTP Sending on Playback changed conditions.

See Settings Page for setup
Enter Username, password
and Page you wished http_get on these events.

Ideally used for automation - turn lights off etc on certain playback events.

Essentially moves these playback events away from Kodi so can still occur with external players.

Adds Manual to install - Updated Manual to Build 170


*Build 150*

* Checks for file in use (slow download or just not finished) before swapping default fanart over.

*Build 151*

* Some changes to support MPC-HC as external player. Need to select and enable web interface in MPC-HC - default port only.
* Need to change Yatse %appdata%/Yatse 3 Socket/Yatse.xml file setting true

*Build 155*

* Further external player support for MPC-HC (nothing additional required)
* Multiple Music Databases supported for extrafanart location
* Checks for existence of MUSIC SOURCE/Artist/extrafanart directory - if true uses this source. (NB limited testing)

*Build 156*

* Some Bug fixes, now Multiple music sources now works for music fanart display AND nowplaying screen.
*Build 160*

* New Installer.
* Change Skin location to %appdata% folder.
* Other locations remain the same. Importantly the Yaste2.xml file remains in %appdata%/Yatse 3 Socket


FAQs:

1. Fanart appears and I can’t see the settings – go to Kodi/Settings menu - will disable Fanart display
2. I have no Fanart
Need to use addon artwork downloader and artwork organizer – download artwork – which is then stored by these addons in the media folders directory- \extrafanart. This will be the fanart that is displayed as you browse the directory.
3. No fanart appears - check log at %appdata% Yatse 3 Socket Logs
4. Check Source.xml and Database need to be the same i.e both 192.168.1.1/files or //server//files; not mixture of IP and UNC paths.





