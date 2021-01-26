using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using TGASharpLib;

namespace vtf_converter
{
	class VTF
	{
		string TeamFortress2Folder;
		public VTF(string TeamFortress2Folder)
		{
			this.TeamFortress2Folder = TeamFortress2Folder;
		}

		public void Convert(string InFile, string OutFile)
		{
			// Resize image to square of larger proportional
			Bitmap Image = new Bitmap(InFile);
			Bitmap SquareImage = ResizeImage(Image);

			// Convert to .tga
			TGA TGASquareImage = (TGA)SquareImage;
			string TGALocation = $"{this.TeamFortress2Folder}\\tf\\materialsrc\\temp.tga";
			TGASquareImage.Save(TGALocation);

			// Convert using VTEX
			VTEXConvert(TGALocation);

			// Path to VTEX output file
			string VTFLocation = $"{this.TeamFortress2Folder}\\tf\\materials\\temp.vtf";

			// Create absolute path to output folder and make directory
			string[] PathInfo = OutFile.Split(new char[] { '\\', '/' });
			PathInfo[PathInfo.Length - 1] = "";
			string FolderPath = String.Join("\\", PathInfo);
			Directory.CreateDirectory(FolderPath);

			// Copy vtf from vtex output to user defined path
			File.Copy(VTFLocation, OutFile, true);

			// Delete temporary tga and vtex output
			File.Delete(TGALocation);
			File.Delete(VTFLocation);
		}

		private Bitmap ResizeImage(Bitmap Image)
		{
			int ImageMaxSize = Math.Max(Image.Width, Image.Height);
			int Power = 2;
			do
			{
				Power *= 2;
			}
			while (Power < ImageMaxSize);

			// Paint G onto SquareImage Bitmap
			Bitmap SquareImage = new Bitmap(Power, Power);
			Graphics G = Graphics.FromImage(SquareImage);
			G.DrawImage(Image, 0, 0, Power, Power);

			return SquareImage;
		}

		private void VTEXConvert(string TGALocation)
		{
			string[] Args = {
				$"\"{TGALocation}\"",
				"-nopause",
				"-game",
				$"\"{this.TeamFortress2Folder}\\tf\\\"",
			};
			string ArgsString = String.Join(" ", Args);
			Process.Start($"{this.TeamFortress2Folder}\\bin\\vtex.exe", ArgsString).WaitForExit();

		}
	}
}

/*
ERROR: Usage: vtex [-outdir dir] [-quiet] [-nopause] [-mkdir] [-shader ShaderName] [-vmtparam Param Value] tex1.txt tex2.txt . . .
-quiet            : don't print anything out, don't pause for input
-warningsaserrors : treat warnings as errors
-nopause          : don't pause for input
-nomkdir          : don't create destination folder if it doesn't exist
-vmtparam         : adds parameter and value to the .vmt file
-outdir <dir>     : write output to the specified dir regardless of source filename and vproject
-deducepath       : deduce path of sources by target file names
-quickconvert     : use with "-nop4 -dontusegamedir -quickconvert" to upgrade old .vmt files
-crcvalidate      : validate .vmt against the sources
-crcforce         : generate a new .vmt even if sources crc matches
        eg: -vmtparam $ignorez 1 -vmtparam $translucent 1
Note that you can use wildcards and that you can also chain them
e.g. materialsrc/monster1/*.tga materialsrc/monster2/*.tga*/

// string TeamFortress2Folder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\";

// string VtexLocation = TeamFortress2Folder + "bin\\vtex.exe";

// string Game = " -game \"" + TeamFortress2Folder + "tf" + "\"";

// string Params = "\"" + "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf\\materialsrc\\temp_tga.tga" + "\"" + Game;

// Process.Start(VtexLocation, Params);