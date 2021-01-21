using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
// using System.Windows.Shapes;

using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using TGASharpLib;

namespace vtf_converter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void FilePathButton_Click(object sender, RoutedEventArgs e)
		{
			using (var fbd = new System.Windows.Forms.OpenFileDialog())
			{
				fbd.ShowDialog();
				FilePathTitle.Content = fbd.FileName;

				Bitmap Image = new Bitmap(fbd.FileName);

				int ImageMaxSize = Math.Max(Image.Width, Image.Height);

				// Calculate larger power of 2
				int Power = 2;
				do
				{
					Power *= 2;
				}
				while (Power < ImageMaxSize);

				Bitmap SquareImage = new Bitmap(Power, Power);

				// Paint Image onto the SquareImage bitmap
				Graphics G = Graphics.FromImage(SquareImage);
				G.DrawImage(Image, 0, 0, Power, Power);

				string TempPath = Path.GetTempPath();

				SquareImage.Save(TempPath + "temp.png");

				TGA T = (TGA)SquareImage;

				System.IO.Directory.CreateDirectory("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf\\materialsrc\\");

				T.Save("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf\\materialsrc\\temp_tga.tga");


				string TeamFortress2Folder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\";

				string VtexLocation = TeamFortress2Folder + "bin\\vtex.exe";

				string Game = " -game \"" + TeamFortress2Folder + "tf" + "\"";

				string Params = "\"" + "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf\\materialsrc\\temp_tga.tga" + "\"" + Game;

				Process.Start(VtexLocation, Params);

			}
		}
	}
}
