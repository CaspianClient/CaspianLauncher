using Caspian_Injector;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;


namespace Caspian
{
    class MC
    {
        public class Minecraft
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Caspian");
            private static Process Process;

            public static void init()
            {
                var mcIndex = Process.GetProcessesByName("Minecraft.Windows");
                if (mcIndex.Length > 0)
                {
                    Process = mcIndex[0];

                }
            }

            public static async Task WaitForModules(string dllpath)
            {
                await Task.Run(async () =>
                {
                    int mpc = 0;

                    while (Process == null)
                    {
                        init();
                        await Task.Delay(100);
                    }

                    while (true)
                    {
                        Process.Refresh();

                        if (Process.Modules.Count > 155)
                        {
                            string i = Insertion.Insert(dllpath).ToString();
                            Process = null;
                            if (i.Equals("SUCCESS"))
                            {
                                Logger.log("Enjoy :)", LogLevel.Info, "Injector");
                            }
                            else
                            {
                                Logger.log(i, LogLevel.Error, "Injector");
                            }
                            
                            break;
                        }
                        else
                        {
                            // this is simple antilogspam
                            if (mpc != Process.Modules.Count)
                            {
                                mpc = Process.Modules.Count;
                                Logger.log(Process.Modules.Count.ToString(), LogLevel.Info, "MC Modules");
                            }
                        }

                        await Task.Delay(100);
                    }
                });
            }
        }

        public enum DllReturns
        {
            SUCCESS = 0,
            ERROR_PROCESS_NOT_FOUND = 1,
            ERROR_PROCESS_OPEN = 2,
            ERROR_ALLOCATE_MEMORY = 3,
            ERROR_WRITE_MEMORY = 4,
            ERROR_GET_PROC_ADDRESS = 5,
            ERROR_CREATE_REMOTE_THREAD = 6,
            ERROR_WAIT_FOR_SINGLE_OBJECT = 7,
            ERROR_VIRTUAL_FREE_EX = 8,
            ERROR_CLOSE_HANDLE = 9,
            ERROR_UNKNOWN = 10,
            ERROR_NO_PATH = 11,
            ERROR_NO_ACCESS = 12,
            ERROR_NO_FILE = 13
        }

        static class DLLImports
        {

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("helper.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int AddTheDLLToTheGame(string path);
        }

        public class Insertion
        {
            public static DllReturns Insert(string path)
            {
                string dllPath = System.IO.Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Caspian"), "helper.dll");

                DLLImports.LoadLibrary(dllPath);
                return (DllReturns)DLLImports.AddTheDLLToTheGame(path);
            }
        }
        public class Version()
        {
            public static string GetMinecraftVersion()
            {
                try
                {

                    string mcExecutable = Process.GetProcessesByName("Minecraft.Windows").FirstOrDefault()?.MainModule.FileName;
                    string mcVersion = FileVersionInfo.GetVersionInfo(mcExecutable).FileVersion;

                    return mcVersion;
                }
                catch (Exception e)
                {
                    Logger.log(e.ToString(), LogLevel.Error, "MC VErsion");
                    return "";
                }
            }
        }
    }
}
