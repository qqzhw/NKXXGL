using ICIMS.Core.Events;
using ICIMS.Model.BusinessManages;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Saraff.Twain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Unity.Attributes;
 

namespace ICIMS.Modules.BusinessManages.ViewModels
{
     public class ScanFileViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private string mRunPath;
        private string mImagePath;

        public DelegateCommand<object> DeleteCcommand { get; private set; }

        private string PDFFileName = $"{Guid.NewGuid().ToString()}.PDF";
        private Twain32 mTwain;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ScanFileViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _title = "文件扫描";
            mTwain = new Twain32();
            _filesManages = new ObservableCollection<FilesManage>();
            _scancolor = new List<string>();
            mRunPath = AppDomain.CurrentDomain.BaseDirectory;
            mImagePath = mRunPath + "ScanFile\\";
            DeleteCcommand = new DelegateCommand<object>(OnDeleteFile);
        }

        private void OnDeleteFile(object obj)
        {
            //
        }

        public ICommand SelectedCommand { get; private set; }
 
        [InjectionMethod]
        public void Init()
        {
            if (!Directory.Exists(mImagePath))
            {
                Directory.CreateDirectory(mImagePath);
            }
            //删除当前目录下文件
            DirectoryInfo directoryInfo = new DirectoryInfo(mImagePath);
            FileInfo[] files = directoryInfo.GetFiles();
            List<string> list = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i].FullName);
            }
            GetPageCount();
            mTwain.Language = TwLanguage.CHINESE_SINGAPORE;
            mTwain.IsTwain2Enable = false;
            mTwain.OpenDSM();
            InitMenu();
            mTwain.EndXfer += twEndXfer;
        }

        private void twEndXfer(object sender, Twain32.EndXferEventArgs e)
        {
             
        }

        private void GetPageCount()
        {
            QryFile();
        }

        private void QryFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(mImagePath);
            FileInfo[] files = directoryInfo.GetFiles("*.png");
            DataRow[] array = null;
            DataRow dataRow = null;
            for (int i = 0; i < files.Length; i++)
            {
                array = dtFile.Select($"扫描文件名='{files[i].Name}'");
                if (array.Length == 0)
                {
                    dataRow = dtFile.NewRow();
                    dataRow["AutoID"] = i + 1;
                    dataRow["扫描文件名"] = files[i].Name;
                    dataRow["FullName"] = files[i].FullName;
                    dtFile.Rows.Add(dataRow);
                }
            }
            //已扫描页 = dtFile.Rows.Count;
            //uhm_v附件.DataSource = dtFile;
        }

        private void InitMenu()
        { 
            //扫描设备绑定
            for (int i = 0; i < mTwain.SourcesCount; i++)
            {
                ScanDevices.Add(mTwain.GetSourceProductName(i));
            }
        }
        private List<string> _scanDpi;
        public List<string> ScanDpi
        {
            get { return _scanDpi; }
            set { SetProperty(ref _scanDpi, value); }
        }
        private List<string> _scancolor;
        public List<string> ScanColors
        {
            get { return _scancolor; }
            set { SetProperty(ref _scancolor, value); }
        }
        private List<string> _scanfileTypes;
        public List<string> ScanFileTypes
        {
            get { return _scanfileTypes; }
            set { SetProperty(ref _scanfileTypes, value); }
        }
        private List<string> _scandevices;
        public List<string> ScanDevices
        {
            get { return _scandevices; }
            set { SetProperty(ref _scandevices, value); }
        }

        private DataTable dtFile = new DataTable();
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }
   

        private bool _newScanPage = false; 
        public bool NewScanPage
        {
            get { return _newScanPage; }
            set { SetProperty(ref _newScanPage, value); }
        }

        private int _pageCount = 0;
        public int PageCount
        {
            get { return _pageCount; }
            set { SetProperty(ref _pageCount, value); }
        }


        #region 通用属性
        private void navigationCallback(NavigationResult result)
        {

        }
         

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }


        //whether the tab is selected;
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        //To do:define the UI for tabcontrol's content;
        public virtual UserControl View { get; set; }


        //The command when clicking Close Button;
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
            {
                if (ConfirmToClose())
                {
                    _eventAggregator.GetEvent<TabCloseEvent>().Publish(View);
                }
            }));

        //It can be overwrite in inherited class to ask for confirming to closing the tab;
        protected virtual bool ConfirmToClose()
        {
            return true;
        }

        #endregion
 
    }
}
