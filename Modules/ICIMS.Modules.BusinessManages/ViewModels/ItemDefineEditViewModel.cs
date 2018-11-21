using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BusinessManages;
using ICIMS.Modules.BusinessManages.Views;
using ICIMS.Service.BusinessManages;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Unity;
using Unity.Attributes; 
using AutoMapper;
using System.Collections.ObjectModel;
using ICIMS.Service;
using System.Windows;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
    public class ItemDefineEditViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _unityContainer;
        private readonly IItemDefineService _itemDefineService;
        private readonly IFilesService _filesService;
        private readonly IWebApiClient _webApiClient;
        public DelegateCommand SaveCommand { get; private set; } 
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand SearchItemCommand { get; private set; }
        public DelegateCommand UploadCommand { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ItemDefineEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _webApiClient = webApiClient;
            _title = "项目立项";
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItemCategory);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
        }

        internal  void BindData(ItemDefineList info)
        {
            //ItemDefine.AuditDate = info.AuditDate;
            //ItemDefine.AuditUserId = info.AuditUserId;
            //ItemDefine.AuditUserName = info.AuditUserName;
            //ItemDefine.BudgetId = info.BudgetId;
            //ItemDefine.BudgetName = info.BudgetName;
            //ItemDefine.DefineAmount = info.DefineAmount;
            //ItemDefine.DefineDate = info.DefineDate;
            //ItemDefine.Id = info.Id;
            //ItemDefine.IsAudit = info.IsAudit;
            //ItemDefine.IsFinal = info.IsFinal;
            //ItemDefine.ItemAddress = info.ItemAddress;
            //ItemDefine.ItemCategoryId = info.ItemCategoryId;
            //ItemDefine.ItemCategoryName = info.ItemCategoryName;
            //ItemDefine.ItemDescription = info.ItemDescription;
            //ItemDefine.ItemDocNo = info.ItemDocNo;
            //ItemDefine.ItemName = info.ItemName;
            ItemDefine = Mapper.Map<ItemDefine>(info);
            GetFiles(ItemDefine);
        }
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage>  FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }
        private async void GetFiles(ItemDefine itemDefine)
        {
            FilesManages.Clear();
            var items =await _filesService.GetAllFiles(itemDefine.Id, "ItemDefine");
            FilesManages = new ObservableCollection<FilesManage>(items.Items);
        }

        internal void BindData()
        {

        }
        /// <summary>
        /// 上传附件
        /// </summary>
        private  void OnUploadedFiles()
        {
            if (ItemDefine.Id<1)
            {
                MessageBox.Show("请先保存立项");
                return;
            }
            var view = _unityContainer.Resolve<SelectedDocumentType>();
            var notification = new Notification()
            {
                Title="文档分类",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, async(callback) => {
                if (callback.DialogResult == true)
                {
                    //选择文档类型
                    var selectView = callback.Content as SelectedDocumentType;
                    var viewModel = selectView.DataContext as SelectedDocumentTypeModel;
                    if (viewModel.SelectedItem == null)
                        return;
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    if(fileDialog.ShowDialog()==true)
                    {
                        var filePath = fileDialog.FileName;
                        var fileName = fileDialog.SafeFileName;
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityId", ItemDefine.Id.ToString()));
                        keyValuePairs.Add(new KeyValuePair<string, string>("FileName", fileName));
                        keyValuePairs.Add(new KeyValuePair<string, string>("UploadType", viewModel.SelectedItem.Name));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityKey", "ItemDefine"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityName", "立项登记"));
                        var filemanage =await _filesService.UploadFileAsync(keyValuePairs, filePath, fileName);
                        FilesManages.Add(filemanage);
                    }
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);
        }
        
        private async void OnSave()
        {
            ItemDefine.DefineAmount = 5000;
            ItemDefine.DefineDate = DateTime.Now;
            ItemDefine.ItemAddress = "成都";
            ItemDefine.ItemCategoryId = 1;
            ItemDefine.ItemDescription = "挥洒的阿萨德";
            ItemDefine.ItemDocNo = "文号110";
            ItemDefine.ItemName = "立项研究项目";
            ItemDefine.Remark = "beizhu";
            ItemDefine.UnitId = 1;
           await _itemDefineService.CreateOrUpdate(ItemDefine);
            
        }
        private void OnSubmit()
        {
            throw new NotImplementedException();
        }

        private void OnCancel()
        {
            throw new NotImplementedException();
        }

        private void OnBack()
        {
            throw new NotImplementedException();
        }

        private void OnShowLog()
        {
            throw new NotImplementedException();
        }

      

        [InjectionMethod]
        public async void Init()
        {
            _itemDefine = new ItemDefine();
            _filesManages = new ObservableCollection<FilesManage>();
        }
        private void OnSelectedItemCategory()
        { 
            var view = _unityContainer.Resolve<SelectItemCategoryView>();
            var notification = new Notification()
            {
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => {
                if (callback.DialogResult==true)
                {
                    var selectView = callback.Content as SelectItemCategoryView;
                    var viewModel = selectView.DataContext as SelectItemCategoryViewModel;
                    this.ItemDefine.ItemCategoryId = viewModel.SelectedItem.Id;
                    this.ItemDefine.ItemCategoryName=viewModel.SelectedItem.Name;
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);

        }
        private ItemDefine _itemDefine;
        public ItemDefine ItemDefine
        {
            get { return _itemDefine; }
            set { SetProperty(ref _itemDefine, value); }
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
            _closeCommand ?? (_closeCommand = new DelegateCommand(() => {
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
    }
   
    
}
