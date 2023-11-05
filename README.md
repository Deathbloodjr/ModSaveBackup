# TdmxModTemplate
 A template for creating mods for TDMX.

# Requirements
 Visual Studio 2022\
 A 1.3.0+ version of the game to compile IL2CPP versions, Mono is buildable without any version of the game.

# How to use
Download the source code.\
Change a few instances of "TdmxModTemplate" to whatever you will name your mod <ModName>\
That would be:
- The main directory of this
- TdmxModTemplate.sln
   - Also open the .sln file in a text editor to change "TdmxModTemplate", "TdmxModTemplate\TdmxModTemplate.csproj", replacing TdmxModTemplate with <ModName>
- TdmxModTemplate folder
- Within that folder, TdmxModTemplate.csproj

Now open the solution itself\
In the .csproj file (double click the <ModName> project in solution explorer)\
- Change AssemblyName to com.<YourName>.<ModName>
- Change the description to whatever your mod does (Or remember to do this later)
- Change RootNamespace and PackageId to ModName
- Change PackageVersion to a fitting version in the format of x.x.x (or whatever your preference is)
- If you plan on building this for IL2CPP
   - Toward the top, there should be a section for 'Release-IL2CPP' and for 'Debug-IL2CPP'
   - Under these, set the GameDir to your IL2CPP Taiko build location

In Plugin.cs\
- Change the namespace to what you selected as RootNamespace earlier
   - You can change the namespaces in the 2 example patches as well, to RootNamespace.Patches
- Change ModName to your ModName

 If this is your first mod, you'll need to set up your NuGet Package source to include BepInEx.\
 - Go to Project at the top -> Manage NuGet Packages...
 - Click the gear at the top right next to Package source: 
 - Click the plus, change the name to BepInEx, and set the source to "https://nuget.bepinex.dev/v3/index.json"

 Now you should be good to start coding!\
 Just add whatever files you want to the Patches folder, and remove my example patches before releasing.
 
 Also good to know but idk where to put it in this readme so it goes here\
 The FodyWeavers.xml file can be used to build other packages into your mod .dll, so everything can be in 1 file with no dependencies\
 I'm not 100% sure how it works when there could be conflicts.


Feel free to check out some of [My other mods](https://docs.google.com/spreadsheets/d/1fuAAfK-0Vw74TwxXF5WVy1fh1ADsVzUkDd7dOHc7EdQ) if you want mod references
