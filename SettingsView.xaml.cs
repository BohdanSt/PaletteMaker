using Microsoft.Win32;
using PaletteMaker.ImageProcessing;
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

        string configFile = Directory.GetCurrentDirectory() + "\\PaletteMaker.config";

        Dictionary<ColorModel, string> colorModels = new Dictionary<ColorModel, string>();

        public static string folderPath = Directory.GetCurrentDirectory();

        public static ColorModel selectedColorMode = ColorModel.BGR;

        public SettingsView()
        {
            InitializeComponent();

            InitializeSettings();

            InitializeColorModelCombobox();
        }

        private void InitializeColorModelCombobox()
        {
            colorModels.Add(ColorModel.BGR, "RGB");
            colorModels.Add(ColorModel.HLS, "HLS");
            colorModels.Add(ColorModel.HSV, "HSV");
            colorModels.Add(ColorModel.LAB, "LAB");
            colorModels.Add(ColorModel.Grayscale, "Grayscale image");

            comboboxColorModel.ItemsSource = colorModels;
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
                using (StreamReader fileReader = new StreamReader(configFile))
                {
                    path = fileReader.ReadLine();

                    string colorModel = fileReader.ReadLine();
                    int value;
                    if (int.TryParse(colorModel, out value))
                    {
                        if (Enum.IsDefined(typeof(ColorModel), value))
                        {
                            selectedColorMode = (ColorModel)value;
                        }
                    }

                    fileReader.Close();
                }
            }
            catch
            {
                UpdateConfigFile();
            }

            if (System.IO.Directory.Exists(path))
            {
                folderPath = path;
            }

            textBlockFolder.Text = folderPath;
        }

        private void UpdateConfigFile()
        {
            using (StreamWriter fileWriter = new StreamWriter(configFile, false))
            {
                fileWriter.WriteLine(folderPath);
                fileWriter.WriteLine((int)selectedColorMode);
                fileWriter.Close();
            }
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

                UpdateConfigFile();
            }
            else
            {
                textBlockFolder.Text = folderPath;
                System.Windows.MessageBox.Show("Deafult folder", "Selected folder doesn't exist.");
            }
        }

        private void buttonFolder_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    folderPath = fbd.SelectedPath;
                    textBlockFolder.Text = folderPath;

                    UpdateConfigFile();
                }
            }
        }

        private void comboboxColorModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColorMode = (ColorModel)comboboxColorModel.SelectedValue;

            UpdateConfigFile();
        }
    }
}
