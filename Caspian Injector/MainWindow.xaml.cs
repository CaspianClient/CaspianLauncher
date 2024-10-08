﻿using Caspian;
using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using static Caspian.MC;

namespace Caspian_Injector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TextBlock _statusTextBlock;

        static string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Caspian");



        private SettingsManager settingsManager;

        public static string dllpath = folderPath + "\\dll.dll";
        public MainWindow()
        {
            InitializeComponent();
            _statusTextBlock = StatusTextBlock;



            

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            settingsManager = new SettingsManager();

            Logger.ClearLogFile();







            Logger.WriteLogToFile(@"
                 _____                 _             
                /  __ \               (_)            
                | /  \/ __ _ ___ _ __  _  __ _ _ __  
                | |    / _` / __| '_ \| |/ _` | '_ \ 
                | \__/\ (_| \__ \ |_) | | (_| | | | |
                 \____/\__,_|___/ .__/|_|\__,_|_| |_|
                                | |                  
                                |_|                  
            ");
            Logger.WriteLogToFile(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Logger.WriteLogToFile("\n\n\n\n");
            VersionManager.VersionInit();

            string dll = folderPath + "\\helper.dll";

            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri(Config.helperURL), dll);
            }
            catch (Exception ex)
            {
                Logger.log(ex.ToString(), LogLevel.Error, "Preload");

                return;
            }

        }





        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(settingsManager);
            settingsWindow.ShowDialog();
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Minecraft.init();

            if (Process.GetProcessesByName("Minecraft.Windows.exe").Length == 0)
            {
                Process procress = new Process();
                procress.StartInfo.Arguments = $"shell:AppsFolder\\{Config.UWPPakageName}!App";
                procress.StartInfo.FileName = "explorer.exe";
                procress.Start();

            check:
                int checks = 0;
                Process[] processes = Process.GetProcessesByName("Minecraft.Windows");

                await Task.Delay(100);
                if (checks > 100)
                {
                    return;
                }
                if (processes.Length == 0)
                {
                    Thread.Sleep(100);
                    checks++;
                    goto check;
                }
            }

            string mcVersion = MC.Version.GetMinecraftVersion();
            string matchedKey = VersionManager.versions.Keys.FirstOrDefault(key => mcVersion.StartsWith(key));

            settingsManager.LoadSettings();

            if (!settingsManager.Settings.UseCustomDll)
            {
                WebClient client = new WebClient();

                if (matchedKey != null && VersionManager.versions.TryGetValue(matchedKey, out string dwurl))
                {
                    Logger.log("Downloading DLL", LogLevel.Info, "Downloader");

                    try
                    {
                        client.DownloadFile(new Uri(dwurl), dllpath);
                    }
                    catch (Exception ex)
                    {
                        Logger.log(ex.ToString(), LogLevel.Error, "Downloader");
                        return;
                    }

                    Logger.log("Injecting...", LogLevel.Info, "Main");
                    await Minecraft.WaitForModules(dllpath);
                }
                else
                {
                    Logger.log($"Can't find dll for {mcVersion}", LogLevel.Info, "Downloader");
                }
            }
            else {
                Logger.log("Injecting...", LogLevel.Info, "Main");
                await Minecraft.WaitForModules(settingsManager.Settings.DllPath);

            }

        }
    }
}