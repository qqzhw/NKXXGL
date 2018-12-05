using ICIMS.Core.Events;
using ICIMS.Model.BusinessManages;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Saraff.Twain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Unity.Attributes;
using static Saraff.Twain.Twain32;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
     public class ScanFileViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private string mRunPath;
        private string mImagePath;
        private ImageInfo CurrentImage;
        public DelegateCommand<object> DeleteCcommand { get; private set; }
        public DelegateCommand ScanCommand { get; private set; }

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
            ScanCommand = new DelegateCommand(OnScanFile);
           // XferEnvironment.MemXferBufferSize = 64 * 1024U; /*64K*/
        }
        /// <summary>
        /// 扫描文件
        /// </summary>
        private void OnScanFile()
        {
            try
            {
                float dpi = 300f;
                switch (SelectDpi)
                {
                    case "标清":
                        dpi = 200f;
                        break;
                    case "高清":
                        dpi = 400f;
                        break;
                    case "超清":
                        dpi = 600f;
                        break;
                    case "超高清":
                        dpi = 1200f;
                        break; 
                }
                 
                mTwain.Capabilities.XResolution.Set(dpi);
                mTwain.Capabilities.YResolution.Set(dpi);
                if (SelectDpi== "黑白")
                {
                    mTwain.Capabilities.PixelType.Set(TwPixelType.BW);
                }
                else
                {
                    mTwain.Capabilities.PixelType.Set(TwPixelType.RGB);
                }
                mTwain.ShowUI = true;
                mTwain.Acquire();
            }
            catch (Exception ex)
            {
               // msgLbl1.AddMsg(ex.Message, Color.Red);
            }
            
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
            PageCount++;
            string empty = string.Empty;
            empty = $"Scan{PageCount.ToString().PadLeft(3, '0')}";
            string filename = mImagePath + empty + ".png";
            e.Image.Save(filename, ImageFormat.Png);
            NewScanPage = true;
            GetPageCount();
            //this._Dispose();
            //if (this.XferEnvironment.PendingXfers > 0)
            //{
            //    this._Acquire();
            //}

          //  (sender as Twain32).OnEndXfer();
        }
        private void OpenPDF(iTextSharp.text.Rectangle PageSize)
        {
            //CurrentImage.PDF.FileStream = new FileStream(CurrentImage.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            //CurrentImage.PDF.iTextDocument = new Document(PageSize, 0f, 0f, 0f, 0f);
            //CurrentImage.PDF.iTextWriter = PdfWriter.GetInstance(CurrentImage.PDF.iTextDocument, CurrentImage.PDF.FileStream);
            //CurrentImage.PDF.iTextDocument.Open();
        }

        private void AddPDFPage(ref Bitmap bmp)
        {
            //CurrentImage.PDF.iTextDocument.NewPage();
            //MemoryStream memoryStream = new MemoryStream();
            //bmp.Save(memoryStream, ImageFormat.Png);
            //iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(memoryStream.ToArray());
            //instance.ScaleToFit(CurrentImage.PDF.iTextDocument.PageSize.Width, CurrentImage.PDF.iTextDocument.PageSize.Height);
            //CurrentImage.PDF.iTextDocument.Add(instance);
        }

        private void ClosePDF()
        {
            //try
            //{
            //    if (CurrentImage.PDF.iTextDocument != null && CurrentImage.PDF.iTextDocument.IsOpen())
            //    {
            //        CurrentImage.PDF.iTextDocument.Close();
            //    }
            //}
            //catch (Exception projectError)
            //{
            //    ProjectData.SetProjectError(projectError);
            //    ProjectData.ClearProjectError();
            //}
            //try
            //{
            //    if (CurrentImage.PDF.iTextWriter != null)
            //    {
            //        CurrentImage.PDF.iTextWriter.Close();
            //    }
            //}
            //catch (Exception projectError2)
            //{
            //    ProjectData.SetProjectError(projectError2);
            //    ProjectData.ClearProjectError();
            //}
            //try
            //{
            //    if (CurrentImage.PDF.FileStream != null)
            //    {
            //        CurrentImage.PDF.FileStream.Close();
            //    }
            //}
            //catch (Exception projectError3)
            //{
            //    ProjectData.SetProjectError(projectError3);
            //    ProjectData.ClearProjectError();
            //}
            //CurrentImage.PDF.iTextDocument = null;
            //CurrentImage.PDF.iTextWriter = null;
            //CurrentImage.PDF.FileStream = null;
            //CurrentImage.FileName = null;
        }

         

        private Bitmap ConvertToBW(Bitmap Original)
        {
            Bitmap bitmap = null;
            if (Original.PixelFormat != PixelFormat.Format32bppArgb)
            {
                bitmap = new Bitmap(Original.Width, Original.Height, PixelFormat.Format32bppArgb);
                bitmap.SetResolution(Original.HorizontalResolution, Original.VerticalResolution);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawImageUnscaled(Original, 0, 0);
                }
            }
            else
            {
                bitmap = Original;
            }
            goto IL_0061;
            IL_0061:
            Bitmap bitmap2 = bitmap;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap2.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            checked
            {
                int num = bitmapData.Stride * bitmapData.Height;
                byte[] array = new byte[num - 1 + 1];
                Marshal.Copy(bitmapData.Scan0, array, 0, num);
                bitmap.UnlockBits(bitmapData);
                Bitmap bitmap3 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format1bppIndexed);
                Bitmap bitmap4 = bitmap3;
                rect = new System.Drawing.Rectangle(0, 0, bitmap3.Width, bitmap3.Height);
                BitmapData bitmapData2 = bitmap4.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
                num = bitmapData2.Stride * bitmapData2.Height;
                byte[] array2 = new byte[num - 1 + 1];
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                byte b = 0;
                int num5 = 128;
                int height = bitmap.Height;
                int width = bitmap.Width;
                int num6 = 500;
                int num7 = height - 1;
                for (int i = 0; i <= num7; i++)
                {
                    num2 = i * bitmapData.Stride;
                    num3 = i * bitmapData2.Stride;
                    b = 0;
                    num5 = 128;
                    int num8 = width - 1;
                    for (int j = 0; j <= num8; j++)
                    {
                        num4 = unchecked((int)array[checked(num2 + 1)]) + unchecked((int)array[checked(num2 + 2)]) + unchecked((int)array[checked(num2 + 3)]);
                        if (num4 > num6)
                        {
                            b = (byte)unchecked((uint)(b + checked((byte)num5)));
                        }
                        if (num5 == 1)
                        {
                            array2[num3] = b;
                            num3++;
                            b = 0;
                            num5 = 128;
                        }
                        else
                        {
                            num5 >>= 1;
                        }
                        num2 += 4;
                    }
                    if (num5 != 128)
                    {
                        array2[num3] = b;
                    }
                }
                Marshal.Copy(array2, 0, bitmapData2.Scan0, num);
                bitmap3.UnlockBits(bitmapData2);
                return bitmap3;
            }
            IL_005c:
            goto IL_0061;
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
            _scancolor.Add("彩色");
            _scancolor.Add("黑白");
            _scanfileTypes.Add("PNG");
            _scanDpi.Add("标清");
            _scanDpi.Add("高清");
            _scanDpi.Add("超清");
            _scanDpi.Add("超高清");
            //扫描设备绑定
            for (int i = 0; i < mTwain.SourcesCount; i++)
            {
                ScanDevices.Add(mTwain.GetSourceProductName(i));
            }
        }
        //选择清晰度
        private string _selectDpi;
        public string SelectDpi
        {
            get { return _selectDpi; }
            set { SetProperty(ref _selectDpi, value); }
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
        //选择颜色
        private string _selectcolor;
        public string SelectColor
        {
            get { return _selectcolor; }
            set { SetProperty(ref _selectcolor, value); }
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
