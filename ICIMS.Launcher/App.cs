using ICIMS.Launcher.AutoUpdater;
using ICIMS.Launcher.Properties;
using ICIMS.Launcher.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xaml;
using System.Xml;

namespace Pvirtech.Commander.Launcher
{
    
    public partial class App : Application
    {
        private static Mutex SingleInstanceMutex = new Mutex(true, "{A20EECB0-F337-49C6-8BB7-B3AD17063CA0}");
        public static bool SingleInstanceCheck()
        {
            if (!App.SingleInstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                Process thisProc = Process.GetCurrentProcess();
                Process process = Process.GetProcessesByName(thisProc.ProcessName).FirstOrDefault(delegate(Process p)
                {
                    if (p.Id != thisProc.Id)
                    {
                        return true;
                    }
                    return false;
                });
                if (process != null)
                {
                    IntPtr mainWindowHandle = process.MainWindowHandle;
                    if (NativeMethods.IsIconic(mainWindowHandle))
                    {
                        NativeMethods.ShowWindow(mainWindowHandle, 9);
                    }
                    NativeMethods.SetForegroundWindow(mainWindowHandle);
                }
                Application.Current.Shutdown(1);
                return false;
            }
            return true;
        }
        protected override 
        [STAThread]
        static void Main()
        {
            if (!App.SingleInstanceCheck())
            {
                return;
            }
            Splasher.Splash = new SplashScreen("");
            Splasher.ShowSplash();
            MessageListener.Instance.ReceiveMessage("正在检查更新... ");
            Thread.Sleep(500);
            try
            {
                var logInfoPath= AppDomain.CurrentDomain.BaseDirectory + "Log\\LogInfo";
                if (Directory.Exists(logInfoPath))
                    Directory.Delete(logInfoPath);

            }
            catch
            {
                
            }
            CheckUpdate();
           // new App();
        }
        private static void CheckUpdate()
        {
            bool bHasError = false;
            IAutoUpdater autoUpdater = new AutoUpdater();
            try
            {
                if (autoUpdater.IsHasUpdate())
                {
                    autoUpdater.Update();
                }
                else
                {
                    StartMainProgram();
                    ShutdownWindow();
                }
             
            }
            catch (WebException exp)
            {
                MessageBox.Show("网络出错，不能找到升级文件!"+exp.Message);
                bHasError = true;
            }
            catch (XmlException exp)
            {
                MessageBox.Show("读取XML文件失败!"+exp.Message);
                bHasError = true;              
            }                   
            catch (Exception exp)
            {
                bHasError = true;
                MessageBox.Show("升级失败，请稍后重试!"+exp.Message);
            }
            finally
            {
                if (bHasError == true)
                {
                    try
                    {
                        autoUpdater.RollBack();
                    }
                    catch
                    {

                    }
                    StartMainProgram();
                }
            }

        }
        public App()
        {
           // StartupUri = new System.Uri ( "MainWindow.xaml", UriKind.Relative );          
            Run();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
           // StartupUri = new System.Uri("MainWindow.xaml", UriKind.Relative);
            //启动相关程序 
            // StartMainProgram();
            // ShutdownWindow();
        }

        private static void ShutdownWindow()
        {
            if (Application.Current != null)
            {
                //关闭启动器                       
                Splasher.CloseSplash();
                Application.Current.Shutdown(1);
            }
            else
            {
                //shutdown this   
                Splasher.CloseSplash();
            }
        }

        /// <summary>
        /// 启动主程序
        /// </summary>
        public static void StartMainProgram()
        {
            using (Process process = new Process())
            {
                process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                process.StartInfo.FileName = Settings.Default.MainProgramName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Verb = "RunAs";
                try
                {
                    MessageListener.Instance.ReceiveMessage("正在启动主程序...");
                    Thread.Sleep(200);
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageListener.Instance.ReceiveMessage(string.Format("启动主程序失败：{0}", ex.Message));
                    Thread.Sleep(500);
                }
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            if (ex is XamlParseException && ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            MessageListener.Instance.ReceiveMessage(ex.Message);
        }
    }
}
