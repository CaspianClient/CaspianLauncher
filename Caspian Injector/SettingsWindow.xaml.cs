using Microsoft.Win32;
using System.Windows;

namespace Caspian_Injector
{
    public partial class SettingsWindow : Window
    {
        private SettingsManager settingsManager;

        public SettingsWindow(SettingsManager settingsManager)
        {
            InitializeComponent();
            this.settingsManager = settingsManager;

            settingsManager.LoadSettings();

            DllPathToggle.IsChecked = settingsManager.Settings.UseCustomDll;
            DllPathTextBox.Text = settingsManager.Settings.DllPath;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectDllPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                DllPathTextBox.Text = openFileDialog.FileName;
                settingsManager.Settings.DllPath = openFileDialog.FileName;
                settingsManager.SaveSettings();
            }
        }

        private void DllPathToggle_Checked(object sender, RoutedEventArgs e)
        {
            settingsManager.Settings.UseCustomDll = DllPathToggle.IsChecked == true;
            settingsManager.SaveSettings();
        }

    }
}
