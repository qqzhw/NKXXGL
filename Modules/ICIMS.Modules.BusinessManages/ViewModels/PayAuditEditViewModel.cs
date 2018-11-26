using AutoMapper;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BaseData;
using ICIMS.Model.BusinessManages;
using ICIMS.Modules.BusinessManages.Views;
using ICIMS.Service;
using ICIMS.Service.BusinessManages;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace ICIMS.Modules.BusinessManages.ViewModels
{

    public class PayAuditEditViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _unityContainer;
        private readonly IItemDefineService _itemDefineService;
        private readonly IFilesService _filesService;
        private readonly IPayAuditService _payAuditService;
        private readonly IBusinessAuditService _businessAuditService;
        private readonly IAuditMappingService _auditMappingService;
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand SearchItemCommand { get; private set; }
        public DelegateCommand SearchContractCommand { get; private set; }
        public DelegateCommand SearchPaymentCommand { get; private set; }
        public DelegateCommand UploadCommand { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public PayAuditEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, PayAuditList data, IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService;
            _title = "项目立项";
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItem);
            SearchContractCommand = new DelegateCommand(OnSelectedContract);
            SearchPaymentCommand= new DelegateCommand(OnSelectedPaymentType);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            _payAudit = new PayAudit();
            _filesManages = new ObservableCollection<FilesManage>();
            _buinessAudits = new ObservableCollection<BusinessAudit>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            BindData(data);
        }

        private void OnSelectedPaymentType()
        {
            
        }

        /// <summary>
        /// 选择合同
        /// </summary>
        private void OnSelectedContract()
        {

            var view = _unityContainer.Resolve<SelectedContract>();
            var notification = new Notification()
            {
                Title = "项目立项列表",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedContractType;
                    var viewModel = selectView.DataContext as SelectedContractTypeModel;
                    ContractCategory = viewModel.SelectedItem;
                }

            });
            view.BindAction(notification.Finish);
        }

        internal void BindData(PayAuditList info)
        {
            if (info.PayAudit == null)
                return; 
            PayAudit = info.PayAudit;
            GetFiles(PayAudit);
        }
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }
        private async void GetFiles(PayAudit payAudit)
        {
            FilesManages.Clear();
            var items = await _filesService.GetAllFiles(payAudit.Id, "ItemDefine");
            FilesManages = new ObservableCollection<FilesManage>(items.Items);
        }
        private ObservableCollection<BusinessAudit> _buinessAudits;
        public ObservableCollection<BusinessAudit> BuinessAudits
        {
            get { return _buinessAudits; }
            set { SetProperty(ref _buinessAudits, value); }
        }
        private ObservableCollection<AuditMapping> _auditMappings;
        public ObservableCollection<AuditMapping> AuditMappings
        {
            get { return _auditMappings; }
            set { SetProperty(ref _auditMappings, value); }
        }
        
        /// <summary>
        /// 上传附件
        /// </summary>
        private void OnUploadedFiles()
        {
            if (_payAudit.Id < 1)
            {
                MessageBox.Show("请先保存立项");
                return;
            }
            var view = _unityContainer.Resolve<SelectedDocumentType>();
            var notification = new Notification()
            {
                Title = "文档分类",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    //选择文档类型
                    var selectView = callback.Content as SelectedDocumentType;
                    var viewModel = selectView.DataContext as SelectedDocumentTypeModel;
                    if (viewModel.SelectedItem == null)
                        return;
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    if (fileDialog.ShowDialog() == true)
                    {
                        var filePath = fileDialog.FileName;
                        var fileName = fileDialog.SafeFileName;
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityId", PayAudit.Id.ToString()));
                        keyValuePairs.Add(new KeyValuePair<string, string>("FileName", fileName));
                        keyValuePairs.Add(new KeyValuePair<string, string>("UploadType", viewModel.SelectedItem.Name));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityKey", "ItemDefine"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityName", "立项登记"));
                        var filemanage = await _filesService.UploadFileAsync(keyValuePairs, filePath, fileName);
                        FilesManages.Add(filemanage);
                    }
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);
        }

        private async void OnSave()
        {
        //    _payAudit.DefineAmount = 5000;
        //    _payAudit.DefineDate = DateTime.Now;
        //    _payAudit.ItemAddress = "成都";
        //    _payAudit.ItemCategoryId = 1;
        //    _payAudit.ItemDescription = "挥洒的阿萨德";
        //    _payAudit.ItemDocNo = "文号110";
        //    _payAudit.ItemName = "立项研究项目";
        //    _payAudit.Remark = "beizhu";
            _payAudit.UnitId = 1;
            await _payAuditService.CreateOrUpdate(_payAudit);

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
            
            InitBusinessAudits();
            LoadAuditMappings();
        }

        private async void InitBusinessAudits()
        {
            var items = await _businessAuditService.GetAllBusinessAudits(2);
            BuinessAudits.AddRange(items.Items);
        }
        private async void LoadAuditMappings()
        {
            var items = await _auditMappingService.GetAllAuditMappings(3, 2);
            _auditMappings.AddRange(items.Items);
        }
        private void OnSelectedItem()
        {
            var view = _unityContainer.Resolve<SelectItemCategoryView>();
            var notification = new Notification()
            {
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectItemCategoryView;
                    var viewModel = selectView.DataContext as SelectItemCategoryViewModel;
                   // this.ItemDefine.ItemCategoryId = viewModel.SelectedItem.Id;
                  //  this.ItemDefine.ItemCategoryName = viewModel.SelectedItem.Name;
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);

        }
        private PayAudit _payAudit;
        public PayAudit PayAudit
        {
            get { return _payAudit; }
            set { SetProperty(ref _payAudit, value); }
        }
        private PayAuditList _payAuditList;
        public PayAuditList PayAuditList
        {
            get { return _payAuditList; }
            set { SetProperty(ref _payAuditList, value); }
        }
        private Contract _contract;
        public Contract Contract
        {
            get { return _contract; }
            set { SetProperty(ref _contract, value); }
        }
        private ItemDefine _itemDefine;
        public ItemDefine ItemDefine
        {
            get { return _itemDefine; }
            set { SetProperty(ref _itemDefine, value); }
        }
        private PaymentTypeItem _paymentTypeItem;
        public PaymentTypeItem PaymentTypeItem
        {
            get { return _paymentTypeItem; }
            set { SetProperty(ref _paymentTypeItem, value); }
        }
         
    }

}
