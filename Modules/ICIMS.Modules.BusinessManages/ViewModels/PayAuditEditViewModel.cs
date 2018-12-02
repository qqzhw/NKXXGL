using Aspose.Words;
using Aspose.Words.Replacing;
using AutoMapper;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BaseData;
using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
using ICIMS.Modules.BusinessManages.Views;
using ICIMS.Service;
using ICIMS.Service.BaseData;
using ICIMS.Service.BusinessManages;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        private readonly IVendorService _vendorService;
        private readonly IFundFromService _fundFromService;
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
        public DelegateCommand AddMoneyCommand { get; private set; }

        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public PayAuditEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, PayAuditList data, IItemDefineService itemDefineService, IFilesService filesService, IPayAuditService payAuditService, IVendorService vendorService, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, UserModel userModel, IFundFromService fundFromService)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService;
            _payAuditService = payAuditService;
            _vendorService = vendorService;
            _fundFromService = fundFromService;
            _title = "支付审核";
            _userModel = userModel;
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItem);
            SearchContractCommand = new DelegateCommand(OnSelectedContract);
            SearchPaymentCommand= new DelegateCommand(OnSelectedPaymentType);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            AddMoneyCommand = new DelegateCommand(OnAddFundFrom);
            _filesManages = new ObservableCollection<FilesManage>();
            _buinessAudits = new ObservableCollection<BusinessAudit>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            _fundItems = new ObservableCollection<FundItem>();
            BindData(data);
        }

        private void OnAddFundFrom()
        {
            var ss = SelectFundItem;
            int s = 0;
        }

        private void OnSelectedPaymentType()
        {
            var view = _unityContainer.Resolve<SelectedPaymentType>();
            var notification = new Notification()
            {
                Title = "支付类型",
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedPaymentType;
                    var viewModel = selectView.DataContext as SelectedPaymentTypeModel;
                     PaymentTypeItem = viewModel.SelectedItem;
                }

            });
            view.BindAction(notification.Finish);
        }

        /// <summary>
        /// 选择合同
        /// </summary>
        private void OnSelectedContract()
        {

            var view = _unityContainer.Resolve<SelectedContractView>();
            var notification = new Notification()
            {
                Title = "合同列表",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedContractView;
                    var viewModel = selectView.DataContext as SelectedContractViewModel;
                    Contract = viewModel.SelectedItem;
                    PayAudit.ContractTotalAmount = Contract.ContractAmount;
                    GetVendorById(Contract.VendorId);
                }

            });
            view.BindAction(notification.Finish);
        }

        internal void BindData(PayAuditList info)
        {
            if (info.PayAudit == null)
            {
                UnitName = _userModel.UnitName;
                _payAudit = null ?? new PayAudit();
                _itemDefine = null ?? new ItemDefine();
                _payAuditDetails = null ?? new ObservableCollection<PayAuditDetail>(); 
                return;
            }
            PayAuditList = info;
            PayAudit = info.PayAudit;
            _payAuditDetails = null ?? new ObservableCollection<PayAuditDetail>();
            PayAuditDetails.Add(new PayAuditDetail()
            {
                Amount = 1000,
                FundName = "市局",
                Remark = "市局款项",
            });
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
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityKey", "PayAudit"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityName", "支付审核"));
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
            _payAudit.ContractTotalAmount=Contract.ContractAmount;
            _payAudit.ContrctId=Contract.Id;
            _payAudit.ItemDefineId=ItemDefine.Id;
            _payAudit.ItemTotalAmount=ItemDefine.DefineAmount;
            _payAudit.PaymentTypeId=PaymentTypeItem.Id ;
            //_payAudit.p = "文号110";
            //_payAudit.ItemName = "立项研究项目";
            //_payAudit.Remark = "beizhu";
           
            _payAudit.PayAuditDetails = PayAuditDetails;
            _payAudit.UnitId = _userModel.UnitId;
            var item=await _payAuditService.CreateOrUpdate(_payAudit);
            if (item.Id > 0)
            {
                PayAudit.Id = item.Id;
            }

        }
        private void OnSubmit()
        {
            OnExportFlowDocumentCmd(null);
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
            LoadFundFrom();//加载资金来源
        }

        private async void GetVendorById(int Id)
        {
            if (Contract != null)
            { 
                var item = await _vendorService.GetById(Contract.VendorId);
                VendorItem = item;
            }
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
        private async void LoadFundFrom()
        {
            var result = await _fundFromService.GetPageItems();
            if (result.datas.Count>0)
            {
                FundItems.AddRange(result.datas);
            }            
        }
        private void OnSelectedItem()
        {
            var view = _unityContainer.Resolve<SelectedItemDefineView>();
            var notification = new Notification()
            {
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedItemDefineView;
                    var viewModel = selectView.DataContext as SelectedItemDefineViewModel;
                     this.ItemDefine.Id = viewModel.SelectedItem.Id;
                  this.ItemDefine.ItemName = viewModel.SelectedItem.ItemName;
                    PayAudit.ItemTotalAmount = ItemDefine.DefineAmount;
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);

        }
        private  void OnExportFlowDocumentCmd(object o)
        {
            try
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xxlz-template.docx");

                var file1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xxlz-template2.docx");

                var content = "evento";
                // Open the document.
                Document doc = new Document(file);


                // Replace the text in the document.
                doc.Range.Replace("$Title$", DateTime.Now.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));
                string cmdNo = string.Format("成联指【{0}】号", 511);
                doc.Range.Replace("$CmdNo$", cmdNo, new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$HostTypeName$","", new FindReplaceOptions(FindReplaceDirection.Forward));

                doc.Range.Replace("$EventSourceFiredTime$", DateTime.Now.ToString("yyyy年MM月dd日 HH点mm分"), new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$InfoContent$", "currentEventInfo.EventInfoContent", new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$EventSourcePerson$", "currentEventInfo.Clrxm", new FindReplaceOptions(FindReplaceDirection.Forward));

                doc.Range.Replace("$EventSourceDepartment$"," currentEventInfo.EventSourceDepartment", new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$Content$", content, new FindReplaceOptions(FindReplaceDirection.Forward));
                

                doc.Range.Replace("$EndDescription$", "", new FindReplaceOptions(FindReplaceDirection.Forward));



                // Save the modified document.
                doc.Save(file1);



                OpenDoc(file1);
            }
            catch (Exception ex)
            {
            }
        }

        private void OpenDoc(string fileName)
        {
            using (var proc = new Process())
            {
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.Arguments = fileName;
                proc.Start();
            }
        }
        private string _unitName;
        public string UnitName
        {
            get { return _unitName; }
            set { SetProperty(ref _unitName, value); }
        }
        #region 属性

        #endregion
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
        private ContractList _contract;
        public ContractList Contract
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

        private VendorItem _vendorItem;
        public VendorItem VendorItem
        {
            get { return _vendorItem; }
            set { SetProperty(ref _vendorItem, value); }
        }
        private ObservableCollection<PayAuditDetail> _payAuditDetails;
        public ObservableCollection<PayAuditDetail> PayAuditDetails
        {
            get { return _payAuditDetails; }
            set { SetProperty(ref _payAuditDetails, value); }
        }
        private ObservableCollection<FundItem> _fundItems;
        public ObservableCollection<FundItem> FundItems
        {
            get { return _fundItems; }
            set { SetProperty(ref _fundItems, value); } 
        }
        private FundItem _selectFundItem;
        public  FundItem SelectFundItem
        {
            get { return _selectFundItem; }
            set { SetProperty(ref _selectFundItem, value); }
        }
    }

}
