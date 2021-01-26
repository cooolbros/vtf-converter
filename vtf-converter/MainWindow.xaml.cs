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
		string TeamFortress2Folder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2";

		string SelectedFilePath;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SelectFileButton_Click(object sender, RoutedEventArgs e)
		{
			using (var fbd = new System.Windows.Forms.OpenFileDialog())
			{
				fbd.ShowDialog();
				this.SelectedFilePath = fbd.FileName;
				PreviewImage.Source = new BitmapImage(new Uri(fbd.FileName));
				OutFileLabel.Visibility = Visibility.Visible;
				FilePathInput.Visibility = Visibility.Visible;
				ConvertButton.Visibility = Visibility.Visible;
			}
		}

		private void ConvertButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				VTF VTFConverter = new VTF(this.TeamFortress2Folder);
				VTFConverter.Convert(this.SelectedFilePath, (this.TeamFortress2Folder + "\\" + FilePathInput.Text));
				System.Windows.MessageBox.Show("Done!");
			}
			catch (Exception error)
			{
				System.Windows.MessageBox.Show(error.ToString());
			}
		}
	}
}