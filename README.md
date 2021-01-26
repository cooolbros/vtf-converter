# VTF Converter

A C#/WPF wrapper around Valve Corporation's vtex.exe VTF Converter

Installation:

```git clone https://github.com/cooolbros/vtf-converter.git```

Usage (GUI):

```dotnet run --project vtf-converter```

Usage (C#)

Include `vtf-converter/vtf.cs` in your project

```c#
string TeamFortress2Folder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2";
VTF VTFConverter = new VTF(TeamFortress2Folder);
VTFConverter.Convert("path/to/image.png", "path/to/texture.vtf");
```