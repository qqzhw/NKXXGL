
using ICIMS.Launcher.AutoUpdater;
using ICIMS.Launcher.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;


namespace ICIMS.Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow:IDisposable
    {
        private bool isFinished = false;
        private List<DownloadFileInfo> downloadFileList = null;
        private List<DownloadFileInfo> allFileList = null;
        private ManualResetEvent evtDownload = null;
        private ManualResetEvent evtPerDonwload = null;
        private WebClient clientDownload = null;
        private long total = 0;
        private long nDownloadedTotal = 0;
        public MainWindow(List<DownloadFileInfo> downloadList)
        {
            InitializeComponent();
            this.downloadFileList = downloadList;
            allFileList = new List<DownloadFileInfo>();
            foreach (DownloadFileInfo file in downloadList)
            {
                allFileList.Add(file);
            }
            this.Loaded += OnMainWindowLoaded;
        }

        void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            evtDownload = new ManualResetEvent(true);
            evtDownload.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcDownload));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!isFinished && MessageBoxResult.No == MessageBox.Show(ConstFileInfo.CANCELORNOT, ConstFileInfo.MESSAGETITLE, MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                if (clientDownload != null)
                    clientDownload.CancelAsync();

                evtDownload.Set();
                evtPerDonwload.Set();
            }
            base.OnClosing(e);
        }

        private void ProcDownload(object o)
        {
            string tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFileInfo.TEMPFOLDERNAME);
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }


            evtPerDonwload = new ManualResetEvent(false);

            foreach (DownloadFileInfo file in this.downloadFileList)
            {
                total += file.Size;
            }
            try
            {
                while (!evtDownload.WaitOne(0, false))
                {
                    if (this.downloadFileList.Count == 0)
                        break;

                    DownloadFileInfo file = this.downloadFileList[0]; 
                    this.ShowCurrentDownloadFileName(file.FileName);

                    //Download
                    clientDownload = new WebClient(); 
                    clientDownload.Credentials = CredentialCache.DefaultCredentials;
                    //End added 
                    clientDownload.DownloadProgressChanged += (sender,e) =>
                    {
                        try
                        {
                            this.SetProcessBar(e.ProgressPercentage, (int)((nDownloadedTotal + e.BytesReceived) * 100 / total));
                        }
                        catch
                        {

                        }

                    };

                    clientDownload.DownloadFileCompleted += (sender,e) =>
                    {
                        try
                        {
                            DealWithDownloadErrors();
                            DownloadFileInfo dfile = e.UserState as DownloadFileInfo;
                            nDownloadedTotal += dfile.Size;
                            this.SetProcessBar(0, (int)(nDownloadedTotal * 100 / total));
                            evtPerDonwload.Set();
                        }
                        catch (Exception)
                        {
                           
                        }

                    };

                    evtPerDonwload.Reset();

                    //Download the folder file
                    string tempFolderPath1 = CommonUnitity.GetFolderUrl(file);
                    if (!string.IsNullOrEmpty(tempFolderPath1))
                    {
                        tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFileInfo.TEMPFOLDERNAME);
                        tempFolderPath += tempFolderPath1;
                    }
                    else
                    {
                        tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFileInfo.TEMPFOLDERNAME);
                    }

                    clientDownload.DownloadFileAsync(new Uri(file.DownloadUrl), System.IO.Path.Combine(tempFolderPath, file.FileFullName), file);

                    //等待下载完成
                    evtPerDonwload.WaitOne();

                    clientDownload.Dispose();
                    clientDownload = null;

                    //移除已下载文件
                    this.downloadFileList.Remove(file);
                }

            }
            catch (Exception)
            {
                ShowErrorAndRestartApplication();              
            } 
            if (downloadFileList.Count > 0)
            {
                return;
            }

            //测试网络 
            DealWithDownloadErrors();

           
            foreach (DownloadFileInfo file in this.allFileList)
            {
                string tempUrlPath = CommonUnitity.GetFolderUrl(file);
                string oldPath = string.Empty;
                string newPath = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(tempUrlPath))
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                        newPath = Path.Combine(CommonUnitity.SystemBinUrl + ConstFileInfo.TEMPFOLDERNAME + tempUrlPath, file.FileName);
                    }
                    else
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                        newPath = Path.Combine(CommonUnitity.SystemBinUrl + ConstFileInfo.TEMPFOLDERNAME, file.FileName);
                    }

                    //xml文件不能下载
                    System.IO.FileInfo f = new FileInfo(newPath);
                    if (!file.Size.ToString().Equals(f.Length.ToString()) && !file.FileName.ToString().EndsWith(".xml"))
                    {
                        ShowErrorAndRestartApplication("dataSize");
                    }


                    //Added for dealing with the config file download errors
                    string newfilepath = string.Empty;
                    if (newPath.Substring(newPath.LastIndexOf(".") + 1).Equals(ConstFileInfo.CONFIGFILEKEY))
                    {
                        if (System.IO.File.Exists(newPath))
                        {
                            if (newPath.EndsWith("_"))
                            {
                                newfilepath = newPath;
                                newPath = newPath.Substring(0, newPath.Length - 1);
                                oldPath = oldPath.Substring(0, oldPath.Length - 1);
                            }
                            File.Move(newfilepath, newPath);
                        }
                    }
                    //End added

                    if (File.Exists(oldPath))
                    {
                        MoveFolderToOld(oldPath, newPath);
                       
                    }
                    else
                    {
                        //Edit for config_ file
                        if (!string.IsNullOrEmpty(tempUrlPath))
                        {
                            if (!Directory.Exists(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1)))
                            {
                                Directory.CreateDirectory(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1));


                                MoveFolderToOld(oldPath, newPath);
                            }
                            else
                            {
                                MoveFolderToOld(oldPath, newPath);
                            }
                        }
                        else
                        {
                            MoveFolderToOld(oldPath, newPath);
                          
                        }

                    }
                    if (newPath.Contains(".zip"))
                    {
                        var flag = UnZipFileHelper.UnZip(oldPath, AppDomain.CurrentDomain.BaseDirectory);
                        if (!flag)
                        {
                            ShowErrorAndRestartApplication();
                        }
                    }
                }
                catch(Exception ex)
                {
                    ShowErrorAndRestartApplication();
                }

            }

            //清理列表
            this.allFileList.Clear();

            if (this.downloadFileList.Count == 0)
                Exit(true);
            else
                Exit(false);

            evtDownload.Set();
        }

        //删除、移动旧文件
        void MoveFolderToOld(string oldPath, string newPath)
        {
            if (File.Exists(oldPath + ".old"))
                File.Delete(oldPath + ".old");

            if (File.Exists(oldPath))
                File.Move(oldPath, oldPath + ".old");

            
            try
            {
                File.Move(newPath, oldPath);
                File.Delete(oldPath + ".old");
            }
            catch (Exception )
            {
                int s = 99;
                
            }
            
        }
         
        private void ShowCurrentDownloadFileName(string name)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.txtCurrent.Text = name;
            }));
        }

        private void SetProcessBar(int current, int total)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.prBarCurrent.Value = current;
                this.prBarTotal.Value = total;
            }));
        }


        private void Exit(bool success)
        {
            this.isFinished = success;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.DialogResult = success;
                this.Close();
            }));

        }

        private void OnCancel(object sender, EventArgs e)
        { 
            evtDownload.Set();
            evtPerDonwload.Set();
            ShowErrorAndRestartApplication();
        }

        private void DealWithDownloadErrors()
        {
            try
            { 
                ClientConfig config = ClientConfig.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFileInfo.FILENAME));
                WebClient client = new WebClient();
                client.DownloadString(config.ServerUrl);
            }
            catch (Exception)
            { 
                ShowErrorAndRestartApplication();
            }
        }

        private void ShowErrorAndRestartApplication(string strInfo="")
        {
            this.Dispatcher.Invoke(new Action(() => {
                this.isFinished = true;
                this.DialogResult = false;
                string info = ConstFileInfo.NOTNETWORK;
                if (!string.IsNullOrEmpty(strInfo))
                {
                    
                    info = "更新失败,数据版本大小不一致!";
                }
                MessageBox.Show(info, ConstFileInfo.MESSAGETITLE, MessageBoxButton.OK, MessageBoxImage.Information);

                CommonUnitity.RestartApplication();
            }));
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            evtDownload.Set();
            evtPerDonwload.Set();
            this.DialogResult = false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MainWindow() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
