# Sims2Tools

##DBPF Library
DBPF Library is a set of classes for reading "The Sims 2" DBPF (.package) files.  It can extract both uncompressed and compressed resources of types BCON, BHAV, CTSS, GLOB, OBJD, OBJf, STR#, TPRP, TRCN, TTAB, TTAs and VERS - ie all the ones of use to coding modders.

##Utils Library
UtilsTools Library is a set of classes to support development of applications that use the DBPF Library, providing classes for registery persistence, MRU lists and common dialogs (about, configuration and progress)

##DBPF Viewer
DBPF Viewer is a simple application to display the contents of a DBPF (.package) file.  It was written primarilary as a testing application for the DBPF Library, but also supports exporting resources as XML (either to the clipboard or to a file)

##BHAV Finder
The Simantics Resource Finder in SimPE (Tools -> PJSE -> Simantics Resource Finder...) is very good at what it does, but doesn't quite measure up if you want to dig into the Maxis game code, for example, when searching for code snippets that use primitives in a certain way, or link to specific strings (eg animations).

BHAV Finder is my attempt to make spelunking in the game code more productive.  Recently, I have wanted to be able to answer questions such as, which BHAVs ...
* use the old version of the relationship primitive
* use TNS Style 0x05 [(see here)](https://www.picknmixmods.com/Sims2/Notes/TnsStyle5/TnsStyle5.html)
* create the "Token - Poison Ivy" object
* access the Lot Inventory
* call "Want Satisfy - Kiss" in the SofaSocialGlobals
* use the firefly jar animations

BHAV Finder was written to provide the answers.

![BHAV Finder App](https://www.picknmixmods.com/Sims2/Notes/BhavFinder/Answer1_1.jpg)

##HCDU Plus
The Hack Conflict Detection Utility (HCDU) is an essential tool when using mods, but it does have some shortcomings
1. it doesn't consider STR#, OBJD, OBJf or other resources
1. it can't be told which folder to start in
1. it can't be told to ignore known conflicts (eg all the InTeenimater flavour paks)
1. it reports conflicts at the resource (BHAV, BCON and STR#) level and not at the package level (most users can't do anything about resource level conflicts so it's enough to know that "InTeenimater_FlavorPak_BackToSchool.package" conflicts with "InTeenimater_B.package" without listing the eleven resources that conflict)
	  
HCDU Plus is my attempt to remedy these  
