using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Principal;
using System.Windows;

namespace AsiManager
{
    // By Stuyk
    public partial class MainWindow : Window
    {
        public readonly string _nopath = Directory.GetCurrentDirectory();
        public readonly string _inactivefiles = "Inactive Files";
        public readonly string _asi = "*.asi";
        public List<string> currentActiveFiles = new List<string>();
        public List<string> currentInactiveFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(_inactivefiles);
            buttonSaveFiles.IsEnabled = false;

            if (IsAdministrator() == false)
            {
                MessageBox.Show("To prevent file moving issues, please run this program as Administrator.", "ASI-Manager by Stuyk");
                Application.Current.Shutdown();
            }
            else if (!File.Exists(_nopath + "/gta_sa.exe"))
            {
                MessageBox.Show("Error: Place ASI-Manager in your main directory.", "ASI-Manager by Stuyk");
                Application.Current.Shutdown();
            }
            else if (!File.Exists(_nopath + "/scripts/global.ini"))
            {
                MessageBox.Show("Missing Silent's ASI Loader. \r\nGoogle it for the latest version.", "ASI-Manager by Stuyk");
            }
        }

        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void moveFile(bool isDisabled, string filename)
        {
            if(isDisabled == true)
            {
                string combinedPath = Path.Combine(_nopath, _inactivefiles);
                combinedPath = Path.Combine(combinedPath, filename);
                string destPath = Path.Combine(_nopath, filename);
                File.Move(combinedPath, destPath);
            }
            else
            {
                string combinedPath = Path.Combine(_nopath, _inactivefiles);
                combinedPath = Path.Combine(combinedPath, filename);
                string destPath = Path.Combine(_nopath, filename);
                File.Move(destPath, combinedPath);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            buttonSaveFiles.IsEnabled = true;
            string[] liveFiles = Directory.GetFiles(_nopath, _asi);
            string[] deadFiles = Directory.GetFiles(_inactivefiles, _asi);

            foreach (var file in liveFiles)
            {
                string parse = Path.GetFileName(file);
                currentActiveFiles.Add(parse);
                activeFiles.Items.Add(parse);
            }

            foreach (var file in deadFiles)
            {
                string parse = Path.GetFileName(file);
                currentInactiveFiles.Add(parse);
                inactiveFiles.Items.Add(parse);
            }

            button.IsEnabled = false;
        }

        private void buttonActivateFile_Click(object sender, RoutedEventArgs e)
        {
            if (inactiveFiles.SelectedIndex != -1) {
                var selectedFile = inactiveFiles.SelectedItem;
                inactiveFiles.Items.Remove(selectedFile);
                activeFiles.Items.Add(selectedFile);
                moveFile(true, selectedFile.ToString());
            }
        }

        private void buttonDisableFile_Click(object sender, RoutedEventArgs e)
        {
            if (activeFiles.SelectedIndex != -1) {
                var selectedFile = activeFiles.SelectedItem;
                activeFiles.Items.Remove(selectedFile);
                inactiveFiles.Items.Add(selectedFile);
                moveFile(false, selectedFile.ToString());
            }
        }

        private void buttonSaveFiles_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = true;
            buttonSaveFiles.IsEnabled = false;
            activeFiles.Items.Clear();
            inactiveFiles.Items.Clear();
            MessageBox.Show("Files have been moved, safe to exit now. \r\nhttp://www.twitter.com/stuykgaming/ \r\n@StuykGaming", "ASI-Manager");
        }
    }
}
