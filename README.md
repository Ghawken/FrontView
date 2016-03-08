# FrontView
New Name - fork of Yatse2

[img]http://i68.tinypic.com/2jayyc6.png[/img]

[size=large]FrontView+ [/size]is an update and a new name for Yatse2 – Yatse2 was a program written by Tolriq for use on Home Theatre Windows Based PCs for Second LCD Screen information.  (see original thread here http://forum.kodi.tv/showthread.php?tid=68936&page=55 )

FrontView+ is an update on this older code (to .Net 4.5.1) adding further features and additional remote code to support Plex and Emby, with two additions for improved functionality with both Kodi and Emby.  (Kodi service and Emby Plugin)

There is no question that FrontView+ would not exist without Yatse and Tolriq code – many thanks for the open source nature of this code. FrontView+ remains open source with the code on Github.com

Basically if you have a PC case with a build-in LCD Screen eg. like these two:
[img]http://i65.tinypic.com/2s80004.png[/img]



[b]This is the program to be running for your media-center.  [/b]
Nothing else in my experience does the job as well.
[size=large]FrontView+ [/size]takes this blank screen, or the very old now unsupported iMon software and turns it into this:

[img]http://i63.tinypic.com/2i8gd5g.jpg[/img]

USAGE

[list]

[*]Use your second LCD Screen for Now Playing Information with fanart/Backdrops and remote control
[*]Touch Screen Remote control (if player supports)
[*]Shows Fanart as you browse Media Center – Fanart updates depending on the Browsing option (Kodi only – needs kodi.service installed)
[*]Shows Default Fanart on other media centers and if no extrafanart for selection
[*]Supports Fanart and CdART for Music Playback for all media centers
[*]Supports PVR Channel info 
[*]Supports sending of HTTP Commands at certain Playback events – enabling FrontView+ to trigger home automation commands
[*]Supports MPC-HC as external Player for Kodi usage
[*]Includes its own local SQL Database for browsing and starting playback of files
[*]Supports Kodi (full support – requires service.addon), Emby (full support – needs Emby Server Plugin), Plex (partial now Playing support only)
[/list]

[u][b]Relevant Fanart Example:[/b][/u]

[align=center][video=youtube]https://youtu.be/Io0AQPlM94E[/video][/align]

Install Manual included in Setup file - for install instructions.
Manual Only:
https://www.mediafire.com/?v8232d1ww2166bb

[b][u]Update: [/u][/b]

[b]Version 1.107[/b]
- Reinstate DisableResolutionDetection: -- setting in settings.xml file
reinstates screen settings on screen change if <MinimiseAlways>false and <DisableResolutionDetection>false
if <DisableResolutionDetection>true - will resort to current behaviour where screen changes are ignored.

- Multiple Fixes for browsing fanart if changing from Kodi to Emby and back again 
KODI:
- Fix/Ignore theme.mp3 files - no nowplaying info for these files and keeps relevant fanart going -
[size=small][i](difficult to sort out but done I believe -Issue was keeping relevant Fanart going despite the playback event and loss of Path information)[/i][/size]
Further Fixes for config File

Update:
Version 1.04
Support Kodi 16/Jarvis
Bug Fixes - for Emby server restart


Download from Mediafire: Link Below:
http://www.mediafire.com/download/lejmjwaz9dbbx2w/FrontView_Setup_1.107.exe
