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

			string MaterialSrc = $"{this.TeamFortress2Folder}\\tf\\materialsrc";

			// Create materialsrc (ensure it exists)
			Directory.CreateDirectory(MaterialSrc);

			// Save image as .tga (cast using TGASharpLib)
			TGA TGASquareImage = (TGA)SquareImage;
			TGASquareImage.Save($"{MaterialSrc}\\temp.tga");

			// Convert using VTEX
			VTEXConvert(MaterialSrc, "temp");

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
			File.Delete($"{MaterialSrc}\\temp.tga");
			File.Delete($"{MaterialSrc}\\temp.txt");
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

		private void VTEXConvert(string FolderPath, string FileNameNoExt)
		{
			// VTEX Args
			// https://developer.valvesoftware.com/wiki/Vtex_compile_parameters
			File.WriteAllLines($"{FolderPath}\\{FileNameNoExt}.txt", new string[] {
				"pointsample 1",
				"nolod 1",
				"nomip 1"
			});

			// VTEX CLI Args
			string[] Args = {
				$"\"{FolderPath}\\{FileNameNoExt}.tga\"",
				"-nopause",
				"-game",
				$"\"{this.TeamFortress2Folder}\\tf\\\"",
			};

			string ArgsString = String.Join(" ", Args);
			Process.Start($"{this.TeamFortress2Folder}\\bin\\vtex.exe", ArgsString).WaitForExit();
		}
	}
}
