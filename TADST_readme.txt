Tophes Arma Dedicated Server Tool (TADST) v2.7
by Tophe of Östgöta Ops
==================================================================

The Armed Assault series allows an amazing array of settings to freely customize dedicated servers. 
This means you will have to edit quite a few config files to get it running the way you like.

TADST is a tool that will greatly simplify this. All you need to do is to choose the settings 
you like. Then the application will create all necessary files according to your settings and 
start the server with the appropriate arguments.

All necessary files will be created in a folder called 'TADST'.
You can save different profiles to easily switch between server setups.

In the folder TADST a new folder for each profile will be created. 
This folder contains all files associated to the profile. 

For support, suggestions, bug reports etc.please use the thread at BIS forum:
http://forums.bistudio.com/showthread.php?t=104991




INSTALLATION
=============
Copy the file 'TADST.exe' to your ArmA game folder. 

* Note that it's important that the file is in the game root folder
  i.e. C:\Games\Steam\SteamApps\common\Arma 3

* If you are upgrading from an earlier version of TADST you should delete 
  your old TADST folder so the program can create new settings files.

You also need to have .NET installed.




FUTURE PLANS AND IDEAS
======================
The TADST is a work in progress. 
Plans and dreams for upcoming releases:

*  Options for allowedFileExtensions, allowedPreprocessFileExtensions, allowedHTMLLoadExtensions, allowedHTMLLoadURIs
*  Options to automatically launch headless clients
*  Steam server update thingy
*  System for setting Mission Parameters.
*  Start as service.
*  Process monitoring, restart server on crash.
*  Remote server/client protocol (probably wont't happen, but hey)



SUPPORT TADST
==============
If you like TADST you can support me by donating som coffee-money or follow 
my music project Meadows ever Bleeding on facebook. http://meadows.se
Info about both is found at the 'What?'-button of the application


THANKS
======
To Bohemia for keeping it real
To Australian Tactical Combat League, Rune "TheFlyingCorpse" Darrud, Dave, Bill, Krijn and Rafael for generous donations
To The Arma community for everything
Östgöta Ops for testing, playing and supporting






VERSION HISTORY
================
2.7   - 2016.01.03	New: Added serverCommandPassword
                    New: Information about which ports to open 
                    New: Checkbox to enable Upnp.
                    New: Allowed File Pathing selection

                    Improved: Headless client settings now have settings for localClient as well.
                    
                    Fixed: Bunch of minor bugs
                    Fixed: HeadlessClient config entries was a bit messed up
                    
                    Removed: Required Secure ID, not used anymore
                    Removed: Steamport settings, not needed anymore. Hardcoded from gameport.                    

2.6   - 2014.07.05	 New: Added enableHT option in Performance tab
					 
					 Improved: Mission tab now show unpacked missions in MPMIssions folder
					 Improved: Open RTP-button now use associated program to open RPT file.
					 
					 Fixed: Program would crash i mission move down button was clicked without a selected mission
					 Removed: Ability to define a custom Steam query port on servers, since it's obsolete since hotfix 1.22
	  	

2.5   - 2014.07.04	 Improved: Doubleclicking Maximum Packet Size now sets it to default
                     Improved: Added tooltips to all controls
						
					 Fixed: Removed Reporting IP functions, since it is deprecated.
                     Fixed: Updated difficulty settings to reflect current version
                     Fixed: Profile folders didn't get deleted when deleting profile.
					 Fixed: IP's entered in headless client wasn't saved to profile.


2.4   - 2013.10.02   New: Cleaned up the interface, added icons and some color to the interface
                     New: Set the difficulty level on each cycled mission individually
                     
                     Fixed: Mission folder wouldn't scan when program starts

2.3   - 2013.09.27   New: Added Maximum Packet Size option (class sockets{maxPacketSize = <limit>})        
                     New: Added Required Secure Id option
                     new: Server now supports headless clients
                     New: Doubleclick password fields to autogenerate readable passwords (just for fun).

                     Improved: Dabbling with some colors to make it less depressing, let me know if you hate it.   
                     Improved: Arma2/Arma2AO beta servers now uses the correct -beta command line   
                     Improved: Steam ports moved to top of config file   
 
                     Fixed: Persistens Battlefield settings
                     Fixed: BattlEye checkbox fixes
                     Fixed: Default difficulty in profile
                     Fixed: Required Build now collects Arma3.exe build, it also saves to profile.
                     Fixed: Kick duplicates checkbox state now saves to profile
                     Fixed: Tooltips checkbox state now saves to profile
                     Fixed: Launch As Is checkbox state s now save to profile. 
                     Fixed: Terrain Grid didn't transfer to config file, fixed now.
                     Fixed: View distance didn't work, working now.
                     Fixed: Server scripting textboxes are now saved and loaded correctly


2.2   - 2013.07.04   New: Steam Port and Steam Query Port settings for Arma 3

                     Improved: Textboxes change to default/suggested values on double click
                     Improved: Brighter background in the netlog/rpt monitor text field
                     Improved: Forum link in the info popup

                     Fixed: Locality bug for decimals made the program crash
                     Fixed: Invalid input in several text fields made TADST crash.

2.1   - 2013.07.03   Fixed: Saved default profile not loading when launching TA2DST
                     Fixed: Program would crash when numeric text fields were empty

2.0   - 2013.06.29 - New: Completely rewritten from the ground up. Less buggy and allows for much faster release cycles. 
                     New: Added 'Launch as is' option, to launch with manually edited config files.
                     New: TADST save file is now binary, a single file for all profiles.                        
                     New: Edit and add custom reporting IP's.
                     New: Reset button in performance settings.
                     New: Added signatures version option.
                     New: Added sort button to missions and folder tab
                     New: Mods preview box replaced with Startup Parameters preview box
                     New: Preview button will generate all config files and navigate to them
                     New: Netlog monitor
                     New: 'Always on top' checkbox to the monitor forms
                     New: Arma 3 reporting IP added to the list
                     New: A3 profile file is created on launch (untested)

                     Improved: Added 'about'-info
                     Improved: Difficulty reset button now only affect selected difficulty.
                     Improved: Mods output box now has horizontal scroller.     
                     Improved: Tweaked the profile handling system a bit.
                     Improved: Tweaked live log monitor, window will only update when file has changed.
                     Improved: Updated and added some information texts and tool tips.
                     Improved: Mission filter now filters by available islands (from file names)
                     Improved: Refreshing missions and mods will no longer reset order
                     Improved: Drag and drop ordering of missions and mods scrolls and works better
                     Improved: Config files are more tidy and readable
                     Improved: Double click to quickset Server Name and Server File
                     Improved: Tweaked export function a bit
                     Improved: Tweaked drag and drop adding of mods

                     Fixed: Max players only went to 64, limit removed
                     Fixed: MOTD max interval limit removed
                     Fixed: VoN Codec quality now goes to 20
                     Fixed: TADST profile is no longer automatically saved on server launch
                     Fixed: Deleting profiles removes the old folder

                     
1.4   - 2012.07.09 - New: Live RPT log monitoring (needs som fine tuning).
                     New: Export function for config files and parameters.
                     New: Added "Minimum Error To Send Near" in performance settings.
                     New: Added option for "requiredBuild=xxxxx", with automatic build retriever.
                     New: Drag and drop folders functionality for mods.
                     New: Drag and drop ordering for mods and missions.
                     New: Checked missions and mods now stay checked after refresh.
			                     
                     Fixed: Expert settings didn't work.
                     Fixed: Initial start would create a TA2DST folder on HD root.
                     Fixed: Couldn't save settings while server was running.
 			

1.3   - 2011.02.09 - New: New function to set mission loading order.

                     Improved: Mod and Mission loading order is now saved in the profile

                     Fixed: Voting bug fixed. Disabling voting now works.
                     Fixed: MOTD maximum interval is increased to 999 seconds

                     
1.2   - 2010.11.06 - New: Set mod loading order.


1.1b  - 2010.11.05 - New: Added checkbox to disable/enable server voting.
                     Fixed: Bug with 'Precision Enemy' value.

      
1.0b  - 2010.08.15 - New: Support for beta servers
                     New: Added mod tab.	
                     New: Added parameters text box.
                     New: Added preview.

                     Fixed: Several minor bugs fixed.


0.9b2 - 2010.08.09   New: Added profiles. 

                     Fixed: Added error-catches for an unidentified bug that gives some users
                            an error the first time they run the program.
                     Fixed: Several minor bugs ironed out. 

                     
0.9b  - 2010.08.08 - Initial beta release. 




LEGAL STUFF, DISCLAIMER
======================
Not that anything could happen when using this but still...

This software is available as is - you use it completely at your own risk. 
The author of this software accept no responsibility for loss of data, 
property, sleep or sanity as result of using it.

This software may not be used for commercial purposes without the written consent of the author.

Copyright 2014, Christoffer Wadensten, Sweden. cwadensten@gmail.com