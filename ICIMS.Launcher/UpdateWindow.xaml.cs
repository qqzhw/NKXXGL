using ICIMS.Launcher.AutoUpdater;

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;


namespace ICIMS.Launcher
{
    /// <summary>
    /// UpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow 
    {
        private bool isFinished = false; 
        private WebClient clientDownload = null;
        private long total = 0;
        private long nDownloadedTotal = 0;
        private DownloadFileInfo downloadfileinfo = null;
        public UpdateWindow(DownloadFileInfo downloadfileinfo)
        {
            InitializeComponent();
            this.downloadfileinfo = downloadfileinfo;
            this.Loaded += UpdateWindow_Loaded;
        }

        void UpdateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcDownload));
            
        }
        private void ProcDownload(object obj)
        {
            string tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFileInfo.TEMPFOLDERNAME);
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }
            //Download
            clientDownload = new WebClient();         
            clientDownload.Credentials =CredentialCache.DefaultCredentials;
            //End added 
            clientDownload.DownloadProgressChanged += (sender, e) =>
            {
                try
                {
                    total = e.TotalBytesToReceive;
                    this.SetProcessBar(e.ProgressPercentage, (int)((nDownloadedTotal + e.BytesReceived) * 100 / e.TotalBytesToReceive));
                }
                catch
                {

                }

            };

            clientDownload.DownloadFileCompleted += (sender, e) =>
            {
                try
                {
                   
                    DownloadFileInfo dfile = e.UserState as DownloadFileInfo;
                    nDownloadedTotal += dfile.Size;
                   // this.SetProcessBar(0, (int)(nDownloadedTotal * 100 / total));
                    clientDownload.Dispose();
                    clientDownload = null;
                    Exit(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("下载文件失败!",ex.Message);
                    Exit(true);
                }

            };


            clientDownload.DownloadFileAsync(new Uri(downloadfileinfo.DownloadUrl), System.IO.Path.Combine(tempFolderPath, downloadfileinfo.FileFullName), downloadfileinfo);

        }

        private void SetProcessBar(int current, int total)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.prBarCurrent.Value = current;
                this.txtPercent.Text = current + "%";
            }));
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
                 
            }
            base.OnClosing(e);
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
