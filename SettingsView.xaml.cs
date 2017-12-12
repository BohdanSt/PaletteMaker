using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaletteMaker
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : System.Windows.Controls.UserControl
    {
        RegistryKey regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        string folderPath = Directory.GetCurrentDirectory();

        string configFile = Directory.GetCurrentDirectory() + "\\PaletteMaker.config";

        public SettingsView()
        {
            InitializeComponent();

            InitializeSettings();
        }

        private void InitializeSettings()
        {
            if (regKey.GetValue("PaletteMaker") == null)
            {
                checkBoxIsAutoRun.IsChecked = false;
            }
            else
            {
                checkBoxIsAutoRun.IsChecked = true;
            }

            string path = "";
            try
            {
                StreamReader fileReader = new StreamReader(configFile);
                path = fileReader.ReadLine();
                fileReader.Close();
            }
            catch
            {
                StreamWriter fileWriter = new StreamWriter(configFile, false);
                fileWriter.WriteLine(folderPath);
                fileWriter.Close();
            }

            if (System.IO.Directory.Exists(path))
            {
                folderPath = path;
            }

            textBlockFolder.Text = folderPath;
        }

        private void AutoRunChange(object sender, RoutedEventArgs e)
        {
            if (checkBoxIsAutoRun.IsChecked == true)
            {
                regKey.SetValue("PaletteMaker", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
            else
            {
                regKey.DeleteValue("PaletteMaker", false);
            }
        }

        private void textBlockFolder_LostFocus(object sender, RoutedEventArgs e)
        {
            if (System.IO.Directory.Exists(textBlockFolder.Text))
            {
                folderPath = textBlockFolder.Text;

                StreamWriter fileWriter = new StreamWriter(configFile, false);
                fileWriter.WriteLine(folderPath);
                fileWriter.Close();
            }
            else
            {
                textBlockFolder.Text = folderPath;
                System.Windows.MessageBox.Show("Deafult folder", "Selected folder doesn't exist.");
            }
        }

        private void buttonFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = fbd.SelectedPath;
                textBlockFolder.Text = folderPath;

                StreamWriter fileWriter = new StreamWriter(configFile, false);
                fileWriter.WriteLine(folderPath);
                fileWriter.Close();
            }
        }
    }
}
