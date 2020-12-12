# Sims2Tools
The Sims2Tools Project consists of two libraries and three applications
* DBFF Library
* Utils Library
* DBPF Viewer Application
* BHAV Finder Application
* HCDU Plus Application

## DBPF Library
The DBPF Library is a set of classes for reading "The Sims 2" DBPF (.package) files.  It can extract both uncompressed and compressed [resources](https://modthesims.info/wiki.php?title=List_of_Sims_2_Formats_by_Name) of types BCON, BHAV, CTSS, GLOB, OBJD, OBJf, STR#, TPRP, TRCN, TTAB, TTAs and VERS - ie all the ones of use to coding modders.

## Utils Library
The Utils Library is a set of classes to support development of applications that use the DBPF Library providing classes for registry persistence, MRU lists and common dialogs (about, configuration and progress)

## DBPF Viewer Application
The [DBPF Viewer](https://www.picknmixmods.com/Sims2/Notes/DbpfViewer/DbpfViewer.html) is a simple application to display the contents of a DBPF (.package) file.  It was written primarily as a testing application for the DBPF Library, but also supports exporting resources as XML (either to the clipboard or to a file)

![DBPF Viewer App](https://www.picknmixmods.com/Sims2/Notes/DbpfViewer/DbpfViewer01.jpg)

## BHAV Finder Application
The Simantics Resource Finder in [SimPE](https://modthesims.info/showthread.php?t=630456) (Tools -> PJSE -> Simantics Resource Finder...) is very good at what it does, but doesn't quite measure up if you want to dig into the Maxis game code, for example, when searching for code snippets that use primitives in a certain way, or link to specific strings (eg animations).

The [BHAV Finder](https://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html) application is my attempt to make spelunking in the game code more productive.  Recently, I have wanted to be able to answer questions such as, which BHAVs ...
* use the old version of the relationship primitive
* use TNS Style 0x05 [(see here)](https://www.picknmixmods.com/Sims2/Notes/TnsStyle5/TnsStyle5.html)
* create the "Token - Poison Ivy" object
* access the Lot Inventory
* call "Want Satisfy - Kiss" in the SofaSocialGlobals
* use the firefly jar animations

BHAV Finder was written to provide the answers.

![BHAV Finder App](https://www.picknmixmods.com/Sims2/Notes/BhavFinder/Answer3_1.jpg)

## HCDU Plus Application
The [Hack Conflict Detection Utility](http://www.leefish.nl/mybb/showthread.php?tid=2063) (HCDU) is an essential tool when using mods, but it does have some shortcomings
1. it doesn't consider STR#, OBJD, OBJf or other resources
1. it can't be told which folder to start in
1. it can't be told to ignore known conflicts (eg all the InTeenimater flavour paks)
1. it reports conflicts at the resource (BHAV, BCON and STR#) level and not at the package level (it's usually enough to know that "InTeenimater_FlavorPak_BackToSchool.package" conflicts with "InTeenimater_B.package" without giving the eleven resources that conflict)
	  
The [HCDU Plus](https://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html) application is my attempt to remedy these.

![HCDU Plus App](https://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus01.jpg)
