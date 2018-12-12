
using ICIMS.Launcher.AutoUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ICIMS.Launcher
{
    /// <summary>
    /// DownloadConfirm.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadConfirm 
    {
        private List<DownloadFileInfo> _downloadFileList = null; 
        public DownloadConfirm(List<DownloadFileInfo> downloadfileList)
        {
            InitializeComponent();
            _downloadFileList = downloadfileList;
            this.Loaded += DownloadConfirm_Loaded;
        }

        void DownloadConfirm_Loaded(object sender, RoutedEventArgs e)
        { 
            this.Activate();
            this.Focus();
        }
         
        private void btnOk_Click(object sender, RoutedEventArgs e)
        { 
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
