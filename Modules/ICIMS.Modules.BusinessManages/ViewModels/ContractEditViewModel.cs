using AutoMapper;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BaseData;
using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
using ICIMS.Modules.BusinessManages.Views;
using ICIMS.Service;
using ICIMS.Service.BusinessManages;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows;
using Unity;
using Unity.Attributes;
using Unity.Resolution;

namespace ICIMS.Modules.BusinessManages.ViewModels
{

    public class ContractEditViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _unityContainer;
        private readonly IItemDefineService _itemDefineService;
        private readonly IFilesService _filesService;
        private readonly IWebApiClient _webApiClient;
        private readonly IBusinessAuditService _businessAuditService;
        private readonly IAuditMappingService _auditMappingService;
        private readonly IContractService _contractService;
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand SearchItemCommand { get; private set; }
        public DelegateCommand CaractTypeCommand { get; private set; }
        public DelegateCommand UploadCommand { get; private set; }
        public DelegateCommand ScanCommand { get; private set; }
        public DelegateCommand SearchVendorCommand { get; private set; }
        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ContractEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, ContractList data,IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, IContractService contractService, UserModel userModel)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _webApiClient = webApiClient;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService;
            _contractService = contractService;
            _userModel = userModel;
            _title = "合同登记";
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItemCategory);
            CaractTypeCommand= new DelegateCommand(OnSelectedCaractType);
            SearchVendorCommand = new DelegateCommand(OnSelectedVendor);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            ScanCommand = new DelegateCommand(OnScanFile);
            _filesManages = new ObservableCollection<FilesManage>();
            _buinessAudits = new ObservableCollection<BusinessAudit>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            BindData(data);
        }

        /// <summary>
        /// 选择合同分类
        /// </summary>
        private void OnSelectedVendor()
        {
            var view = _unityContainer.Resolve<SelectedVendorView>();
            var notification = new Notification()
            {
                Title = "供应商列表",
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedVendorView;
                    var viewModel = selectView.DataContext as SelectedVendorViewModel;
                    VendorItem = viewModel.SelectedItem;
                }

            });
            view.BindAction(notification.Finish);
        }


        /// <summary>
        /// 选择合同分类
        /// </summary>
        private void OnSelectedCaractType()
        {
            var view = _unityContainer.Resolve<SelectedContractType>();
            var notification = new Notification()
            {
                Title = "合同分类",
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

        internal void BindData(ContractList info)
        {
            InitBusinessAudits();
            if (info.Id == 0)
            {
                //_contractCategory = new ContractItem();
                //_vendorItem = new VendorItem();
                //_itemDefine = new ItemDefine();
                _contract = null ?? new Contract();
                _contract.BeginTime = DateTime.Now;
                _contract.EndTime = DateTime.Now;
                _contract.IdentifyDate = DateTime.Now;
                _contract.ContractTime = DateTime.Now;
                return;
            }
            
            Contract = Mapper.Map<Contract>(info);
            GetFiles(Contract); 
            LoadItemDefine(Contract.ItemDefineId);//加载立项项目
        }
        
        private async void LoadItemDefine(int Id)
        {
            if (Id > 0)
            {
                var item = await _itemDefineService.GetById(Id);
                ItemDefine = item;
            }
        }
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }
        private async void GetFiles(Contract contract)
        {
            FilesManages.Clear();
            var items = await _filesService.GetAllFiles(contract.Id, "Contract");
            FilesManages = new ObservableCollection<FilesManage>(items.Items);
        }
        private async Task GetItemDefineFiles(int itemdefineId)
        {
            var items = await _filesService.GetAllFiles(itemdefineId, "ItemDefine");
            foreach (var item in items.Items)
            {
                if (FilesManages.Contains(item))
                    continue;
                FilesManages.Add(item);
            }

            await Task.CompletedTask;
        }
        private async void GetReViewDefineFiles(int viewdefineId)
        {
            var items = await _filesService.GetAllFiles(viewdefineId, "ReViewDefine");

            foreach (var item in items.Items)
            {
                if (FilesManages.Contains(item))
                    continue;
                FilesManages.Add(item);
            }

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
            if (Contract.Id < 1)
            {
                MessageBox.Show("请先保存合同");
                return;
            }
            var view = _unityContainer.Resolve<SelectedDocumentType>();
            var notification = new Notification()
            {
                Title = "合同分类",
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
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityId", Contract.Id.ToString()));
                        keyValuePairs.Add(new KeyValuePair<string, string>("FileName", fileName));
                        keyValuePairs.Add(new KeyValuePair<string, string>("UploadType", viewModel.SelectedItem.Name));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityKey", "Contract"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityName", "合同登记"));
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
            
            //ItemDefine.DefineDate = DateTime.Now;
            //ItemDefine.ItemAddress = "成都";
            //ItemDefine.ItemCategoryId = 1;
            //ItemDefine.ItemDescription = "挥洒的阿萨德";
            //ItemDefine.ItemDocNo = "文号110";
            //ItemDefine.ItemName = "立项研究项目";
            //ItemDefine.Remark = "beizhu";
            //ItemDefine.UnitId = 1;
            if (ContractCategory==null||ItemDefine==null||VendorItem==null)
            {
                return;
            }
            Contract.VendorId = VendorItem.Id;
            Contract.UnitId = ItemDefine.UnitId;
            Contract.ContractCategoryId = ContractCategory.Id;
            Contract.ItemDefineId = ItemDefine.Id;
            var item=await _contractService.CreateOrUpdate(Contract);
            if (item.Id>0)
            {
                Contract.Id = item.Id;
                Contract.ContractNo = item.ContractNo;
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("保存失败！");
            }

        }
        private  void OnSubmit()
        {
            var auditItem = BuinessAudits.Where(o =>o.Status==0).OrderBy(o => o.DisplayOrder).FirstOrDefault();
            if (auditItem == null)
                return;
            var flag=IsCanAudit(auditItem);
            if (!flag)
            {
                MessageBox.Show("对不起，您没有审核权限");
                return;
            }
            var auditmapping = new AuditMapping()
            {
                BusinessTypeId = 4,
                BusinessTypeName = "合同登记",
                ItemId = ItemDefine.Id,
                BusinessAuditId = auditItem.Id,
                DisplayOrder = auditItem.DisplayOrder
            };

            var view = _unityContainer.Resolve<SubmitAuditView>(new ParameterOverride("data", auditmapping));
            var notification = new Notification()
            {
                Title = "审核",
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) => {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                   var item=  await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    if (item.Id > 0)
                    {
                        UpdateAuditStatus();
                        //LoadAuditMappings();
                        InitBusinessAudits();
                        UpdateStatus(1);
                    }
                    else
                        MessageBox.Show("审核失败!");
                }

            });
            view.BindAction(notification.Finish);
        }
        /// <summary>
        /// 根据当前审核项 查询当前用户是否具有审核资格
        /// </summary>
        /// <param name="auditItem"></param>
        private bool IsCanAudit(BusinessAudit auditItem)
        { 
            var findItem = _userModel.Roles.FirstOrDefault(o => o.Id == auditItem.RoleId);
            if (findItem==null)
            { 
                return false;
            }
            return true;
        }

         

        /// <summary>
        /// 更新审核状态
        /// </summary>
        private void UpdateAuditStatus()
        {
            foreach (var item in BuinessAudits)
            {
                var findItem = AuditMappings.FirstOrDefault(o => o.BusinessAuditId == item.Id);
                if (findItem != null)
                {
                    item.Status = findItem.Status;
                    item.StatusName = findItem.StatusText;
                }
            }
        }
        private async void OnCancel()
        {
            var deleteItem = AuditMappings.LastOrDefault(o => o.Status == 1);
            if (deleteItem != null)
            {
                await _auditMappingService.Delete(deleteItem.Id);
            }
        }

        private void OnBack()
        {
            var auditItem = BuinessAudits.Where(o => o.Status==0).OrderBy(o => o.DisplayOrder).FirstOrDefault();
            if (auditItem == null)
                return;
            var flag = IsCanAudit(auditItem);
            if (!flag)
            {
                MessageBox.Show("对不起，您没有审核权限");
                return;
            }
            var auditmapping = new AuditMapping()
            {
                BusinessTypeName = "合同登记",
                ItemId = Contract.Id,
                BusinessAuditId = auditItem.Id,
                DisplayOrder = auditItem.DisplayOrder
            };

            var view = _unityContainer.Resolve<SubmitAuditView>(new ParameterOverride("data", auditmapping));
            var notification = new Notification()
            {
                Title = "驳回审核",
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) => {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                    viewModel.AuditMapping.Status = 2; //驳回审核
                   var item=  await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    if (item.Id > 0)
                    {
                        UpdateAuditStatus();
                        //LoadAuditMappings();
                        InitBusinessAudits();
                        UpdateStatus(2);
                    }
                   
                }

            });
            view.BindAction(notification.Finish);
        }

        private void OnShowLog()
        {
            
        }

        private void OnScanFile()
        {
            if (Contract.Id < 1)
            {
                MessageBox.Show("请先保存合同");
                return;
            }
            var view = _unityContainer.Resolve<SelectedDocumentType>();
            var notification = new Notification()
            {
                Title = "文档分类",
                Content = view,
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

                    var scanParam = new FilesManage() { EntityId = Contract.Id, EntityKey = "Contract", EntityName = "合同登记", UploadType = viewModel.SelectedItem.Name };
                    var scanView = _unityContainer.Resolve<ScanFileView>(new ParameterOverride("data", scanParam));
                    var notify = new Notification()
                    {
                        Title = "文档扫描",
                        Content = scanView,
                    };
                    PopupWindows.NotificationRequest.Raise(notify, (scanBack) =>
                    {
                        var fileView = scanBack.Content as ScanFileView;
                        var scanFileViewmodel = fileView.DataContext as ScanFileViewModel;
                        scanFileViewmodel.Dispose();
                        GetFiles(Contract);
                    }); 
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);
        }


        [InjectionMethod]
        public  void Init()
        { 
            //InitBusinessAudits();
            //LoadAuditMappings();
        }

        private async void InitBusinessAudits()
        { 
            var items = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName: "合同登记");
            BuinessAudits.Clear();
            BuinessAudits.AddRange(items.Items);
            LoadAuditMappings();
        }
        private async void LoadAuditMappings()
        {
            if (Contract.Id == 0)
                return;
            var items = await _auditMappingService.GetAllAuditMappings(Contract.Id, BusinessTypeName:"合同登记");
         
            AuditMappings.Clear();
            _auditMappings.AddRange(items.Items);
            if (AuditMappings.Count > 0)
            {
                foreach (var item in BuinessAudits)
                {
                    var findItem = AuditMappings.FirstOrDefault(o => o.RoleId == item.RoleId & o.BusinessAuditId == item.Id);
                    if (findItem != null)
                    {
                        item.Status = findItem.Status;
                        item.StatusName =findItem.StatusText;
                    }
                }
                var isComplete = BuinessAudits.FirstOrDefault(o => o.Status == 0);
                if (isComplete == null)
                {
                    Contract.Status = 3;
                    await _contractService.CreateOrUpdate(Contract);
                }
                else
                {
                    //Contract.Status = 1;
                    //await _contractService.CreateOrUpdate(Contract);
                }
                CanEdit = false;
            }
            CheckRole();
        }
        private async void UpdateStatus(int status)
        {
            Contract.Status = status;
            await _contractService.CreateOrUpdate(Contract);
        }
        /// <summary>
        /// 获取用户是否是审核角色
        /// </summary>
        private void CheckRole()
        {
            //角色是否可审核
            var findItem = BuinessAudits.Where(o =>o.Status==0).OrderBy(o => o.DisplayOrder).FirstOrDefault();
            if (findItem != null)
            {
                var canAudit = _userModel.Roles.FirstOrDefault(o => o.Id == findItem.RoleId);
                if (canAudit != null)
                {
                    // MessageBox.Show("你不是审核角色");
                    CanChecked = true;
                    return;
                }
                else
                {
                    CanChecked = false;
                }
            }
        }
        private void OnSelectedItemCategory()
        {            
            var view = _unityContainer.Resolve<SelectedItemDefineView>();
            var notification = new Notification()
            {
                Title = "项目立项列表",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, async(callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedItemDefineView;
                    var viewModel = selectView.DataContext as SelectedItemDefineViewModel;
                    if (viewModel.SelectedItem!=null)
                    {
                        ItemDefine = Mapper.Map<ItemDefine>(viewModel.SelectedItem);
                       await GetItemDefineFiles(ItemDefine.Id);
                    }                    

                }               
            });
            view.BindAction(notification.Finish);

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
        private ContractItem  _contractCategory;
        public ContractItem ContractCategory
        {
            get { return _contractCategory; }
            set { SetProperty(ref _contractCategory, value); }
        }
        private VendorItem  _vendorItem;
        public VendorItem VendorItem
        {
            get { return _vendorItem; }
            set { SetProperty(ref _vendorItem, value); }
        }
        private bool _canEdit = true;
        public bool CanEdit
        {
            get { return _canEdit; }
            set { SetProperty(ref _canEdit, value); }
        }
        private bool _canChecked = false;
        public bool CanChecked
        {
            get { return _canChecked; }
            set { SetProperty(ref _canChecked, value); }
        }
    }


}
