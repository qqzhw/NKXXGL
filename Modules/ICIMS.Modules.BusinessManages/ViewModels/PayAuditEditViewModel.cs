using Aspose.Words;
using Aspose.Words.Replacing;
using Aspose.Words.Tables;
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
using Unity.Resolution;

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
        private readonly IBusinessAuditStatusService _businessAuditStatusService;
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
        public DelegateCommand ScanCommand { get; private set; }
        public DelegateCommand<object> DelFund { get; private set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand DownloadCommand { get; }

        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public PayAuditEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, PayAuditList data, IItemDefineService itemDefineService, IFilesService filesService, IPayAuditService payAuditService, IVendorService vendorService, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, UserModel userModel, IFundFromService fundFromService, IBusinessAuditStatusService businessAuditStatusService)
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
            _businessAuditStatusService = businessAuditStatusService;
            _title = "支付审核";
            _userModel = userModel;
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItem);
            SearchContractCommand = new DelegateCommand(OnSelectedContract);
            SearchPaymentCommand = new DelegateCommand(OnSelectedPaymentType);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            AddMoneyCommand = new DelegateCommand(OnAddFundFrom);
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
            DownloadCommand = new DelegateCommand(OnDownloadCommand);
            ScanCommand = new DelegateCommand(OnScanFile);
            DelFund = new DelegateCommand<object>(OnDelSelectedFund);
            _filesManages = new ObservableCollection<FilesManage>();
            _businessAudits = new ObservableCollection<BusinessAuditList>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            _fundItems = new ObservableCollection<FundItem>();
            BindData(data);
            _payAuditList = data;
        }

        private void OnDownloadCommand()
        {
            OnDownloadFile(SelectedFile);
        }

        private async void OnDeleteCommand()
        {
            try
            {
                if (SelectedFile==null)
                {
                    return;
                }
                await this._filesService.Delete((long)this.SelectedFile.Id);
                this.FilesManages.Remove(this.SelectedFile);
                this.SelectedFile = this.FilesManages.FirstOrDefault();
                MessageBox.Show("删除成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void OnDelSelectedFund(object obj)
        {
            PayAuditDetails.Remove(PayAuditDetail);
            PayAudit.PayAmount = PayAuditDetails.Select(o => o.Amount).Sum();
        }

        private void OnAddFundFrom()
        {
            if (SelectFundItem == null)
                return;
            if (TempAmount == 0)
                return;
            var detail = new PayAuditDetail()
            {
                Amount = TempAmount,
                FundName = SelectFundItem.Name,
                PayAuditId = PayAudit.Id
            };
            var findItem = PayAuditDetails.FirstOrDefault(o => o.FundName == SelectFundItem.Name);
            if (findItem != null)
            {
                findItem.Amount = TempAmount;
                return;
            }
            PayAuditDetails.Add(detail);

            PayAudit.PayAmount = PayAuditDetails.Select(o => o.Amount).Sum();

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
                    PayAudit.PaymentTypeId = viewModel.SelectedItem.Id;
                    PayAudit.PaymentName = viewModel.SelectedItem.Name;
                    SelectedIndex = 1;
                    PaymentRemark = viewModel.SelectedItem.Description;
                }

            });
            view.BindAction(notification.Finish);
        }


        private void OnDownloadFile(FilesManage model)
        {
            var permission = _userModel.Permissions.FirstOrDefault(o => o == "Pages.FilesManage");
            if (permission == null)
            {
                MessageBox.Show("你没有权限操作此项");
            }
            if (model == null || string.IsNullOrEmpty(model.DownloadUrl))
                return;
            var view = _unityContainer.Resolve<FileDownloadView>(new ParameterOverride("model", model));
            var notification = new Notification()
            {
                Title = "附件下载",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => { });
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

        internal async void BindData(PayAuditList info)
        {
            await InitBusinessAudits();
            UnitName = _userModel.UnitName;
            _payAudit = null ?? new PayAudit();
            _itemDefine = null ?? new ItemDefine();
            PaymentTypeItem = null ?? new PaymentTypeItem();
            _contract = null ?? new ContractList();
            _vendorItem = null ?? new VendorItem();
            _payAuditDetails = null ?? new ObservableCollection<PayAuditDetail>();
            RaisePropertyChanged("PayAudit");
            if (info.PayAudit == null)
            { 
               
                return;
            }
            Contract.ContractName = info.ContractName;
            VendorItem.OpenBank = info.OpenBank;
            VendorItem.Name = info.VendorName;
            VendorItem.AccountName = info.AccountName;
            PaymentTypeItem.Name = info.PaymentTypeName;
            PayAuditList = info;
            ItemDefine.ItemName = info.ItemDefineName;
            ItemDefine.DefineAmount = info.DefineAmount;
            UnitName = info.UnitName;
            PayAudit = info.PayAudit;
            PayAudit.ItemDefineId = info.PayAudit.ItemDefineId;
            PayAudit.PaymentTypeId = info.PayAudit.PaymentTypeId;
            _payAuditDetails = null ?? new ObservableCollection<PayAuditDetail>();
            RaisePropertyChanged("PaymentTypeItem");
            RaisePropertyChanged("VendorItem");
            RaisePropertyChanged("ItemDefine");
            RaisePropertyChanged("Contract");
            GetInfo();
            GetFiles(PayAudit);
            await LoadAuditMappings();
            await GetNewStatus();
        }
        private async void GetInfo()
        {
            await UpdatePayDetail();
        }
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }

        private FilesManage _selectedFile;
        private async void GetFiles(PayAudit payAudit)
        {
            FilesManages.Clear();
            var items = await _filesService.GetAllFiles(payAudit.Id, "PayAudit");
            FilesManages = new ObservableCollection<FilesManage>(items.Items);
        }
        private ObservableCollection<BusinessAuditList> _businessAudits;
        public ObservableCollection<BusinessAuditList> BusinessAudits
        {
            get { return _businessAudits; }
            set { SetProperty(ref _businessAudits, value); }
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
                MessageBox.Show("请先提交保存");
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
            _payAudit.ContractTotalAmount = Contract.ContractAmount;
            _payAudit.ContrctId = Contract.Id;
            _payAudit.ItemDefineId = ItemDefine.Id;
            _payAudit.ItemTotalAmount = ItemDefine.DefineAmount;
            _payAudit.PaymentTypeId = PaymentTypeItem.Id;
            //_payAudit.p = "文号110";
            //_payAudit.ItemName = "立项研究项目";
            //_payAudit.Remark = "beizhu";

            _payAudit.PayAuditDetails = PayAuditDetails;
            _payAudit.UnitId = _userModel.UnitId;
            var item = await _payAuditService.CreateOrUpdate(_payAudit);
            if (item.Id > 0)
            {
                PayAudit.Id = item.Id;
                PayAudit.PaymentNo = item.PaymentNo;
                MessageBox.Show("保存成功！");
            }
            else
                MessageBox.Show("保存失败！");
        }
        private void OnSubmit()
        { 
            var auditItem = GetCurrent();
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
                BusinessTypeId = 5,
                BusinessTypeName = "支付审核",
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
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                    viewModel.AuditMapping.Status = 1; 
                    viewModel.AuditMapping.BusinessAuditStatusId = auditItem.BusinessAuditStatusId;
                    try
                    {
                        var item = await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                        if (item.Id > 0)
                        {

                            auditItem.Status = 1;

                            await UpdateBusinessAudit(auditItem, ItemDefine.Id, 1);

                            //await GetNewStatus();
                            var completed = BusinessAudits.Count(o => o.Status == 1);
                            if (completed == BusinessAudits.Count)
                            {
                                UpdateStatus(3);
                            }
                            else
                            {
                                UpdateStatus(1);//标记立项处于审核中状态
                            }
                            await LoadAuditMappings();
                        }
                    }
                    catch (Exception)
                    {

                    }

                }

            });
            view.BindAction(notification.Finish);
        }

        private async void OnCancel()
        { 
            if (PayAudit.Status == 3)
                return;
            var findItem = BusinessAudits.LastOrDefault(o => o.Status == 1 && _userModel.Roles.FirstOrDefault(r => r.Id == o.RoleId) != null);
            if (findItem == null)
                return;
            try
            {
                //var findItem = BuinessAudits.LastOrDefault(o => o.Status == 1);
                var findmap = AuditMappings.LastOrDefault(o => _userModel.Roles.FirstOrDefault(r => r.Id == o.RoleId) != null);
                await _auditMappingService.Delete(findmap.Id);
                await UpdateBusinessAudit(findItem, ItemDefine.Id, 0);

                if (BusinessAudits.FirstOrDefault(o => o.Status > 0) == null)
                {
                    UpdateStatus(0);//标记立项处于制单中
                }

            }
            catch (Exception)
            {

            }
        }

        private void OnBack()
        {
            var auditItem = BusinessAudits.Where(o => o.Status == 1).OrderBy(o => o.DisplayOrder).LastOrDefault();
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
                BusinessTypeName = "支付审核",
                ItemId = PayAudit.Id,
                BusinessAuditId = auditItem.Id,
                DisplayOrder = auditItem.DisplayOrder
            };

            var view = _unityContainer.Resolve<SubmitAuditView>(new ParameterOverride("data", auditmapping));
            var notification = new Notification()
            {
                Title = "驳回审核",
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                    viewModel.AuditMapping.Status = 2; //驳回审核
                    try
                    {
                        var item = await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);

                        if (item.Id > 0)
                        {

                            //LoadAuditMappings();

                            UpdateStatus(2);//标记立项处于已驳回状态
                                            //var status =await _businessAuditStatusService.GetById(auditItem.BusinessAuditStatusId);
                                            //if (status!=null)
                                            //{
                                            //    status.Status = 0;
                                            //    await _businessAuditStatusService.CreateOrUpdate(status);
                                            //}

                            await UpdateBusinessAudit(auditItem, ItemDefine.Id, 0);
                            //await GetNewStatus();//获取最新状态

                            await GetNewStatus();
                            var completed = BusinessAudits.Count(o => o.Status == 1);
                            if (completed == BusinessAudits.Count)
                            {
                                UpdateStatus(3);
                            }
                            else
                            {
                                //UpdateStatus(1);//标记立项处于审核中状态
                                if (completed == 0)
                                {
                                    UpdateStatus(0);
                                }
                            }
                            await LoadAuditMappings();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("驳回失败!");
                    }

                   

                }

            });
            view.BindAction(notification.Finish);
        }

        private void OnShowLog()
        {
            OnExportFlowDocumentCmd(null);
        }
        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="item"></param>
        /// <param name="entityId"></param>
        /// <param name="status"></param>
        private async Task UpdateBusinessAudit(BusinessAuditList item, int entityId, int status)
        {
            var entity = new BusinessAuditStatus()
            {
                BusinessAuditId = item.Id,
                BusinessTypeName = item.BusinessTypeName,
                DisplayOrder = item.DisplayOrder,
                EntityId = entityId,
                Id = item.BusinessAuditStatusId,
                Status = status,
            };
            await _businessAuditStatusService.CreateOrUpdate(entity);
            await GetNewStatus();
            await LoadAuditMappings();
        }
        private async Task GetNewStatus()
        {
            if (ItemDefine.Id > 0)
            {
                var result = await _businessAuditService.GetAll(BusinessTypeName: "支付审核", entityId: PayAudit.Id);

                foreach (var item in result.Items)
                {
                    var model = BusinessAudits.FirstOrDefault(o => o.Id == item.Id);
                    if (model != null)
                    {
                        model.BusinessAuditStatusId = item.BusinessAuditStatusId;
                        model.EntityId = ItemDefine.Id;
                        model.Status = item.Status;
                    }
                }
                RaisePropertyChanged("BusinessAudits");
            }

            CheckRole();
            CheckEdit();
            CheckAudit();//取消审核
            CheckOnBack();
            CheckComplete();
        }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        private void CheckEdit()
        {
            //查询当前是否有审核项 true 不能编辑
            var findItem = BusinessAudits.FirstOrDefault(o => o.Status > 0);
            if (findItem != null)
            {
                CanEdit = false;
            }
            else
            {
                CanEdit = true; 

            }
        }
        /// <summary>
        /// 是否可审批
        /// </summary>
        private void CheckAudit()
        {
            var findItem = BusinessAudits.LastOrDefault(o => o.Status == 1 && _userModel.Roles.FirstOrDefault(r => r.Id == o.RoleId) != null);
            if (findItem != null)
            {
                if (findItem != null)
                {
                    CanCancel = true;
                }
                else
                {
                    CanCancel = false;
                }
            }
        }
        //获取当前待审批项 无审核项返回null
        public BusinessAuditList GetCurrent()
        {
            var findItem = BusinessAudits.FirstOrDefault(o => o.Status == 0);
            if (findItem != null)
            {
                var role = _userModel.Roles.FirstOrDefault(r => r.Id == findItem.RoleId);
                if (role != null)
                {
                    return findItem;
                }

            }
            return null;
        }

        public BusinessAuditList GetCheckedItem()
        {
            var findItem = BusinessAudits.LastOrDefault(o => o.Status == 1);
            if (findItem != null)
            {
                var role = _userModel.Roles.FirstOrDefault(r => r.Id == findItem.RoleId);
                if (role != null)
                {
                    return findItem;
                }

            }
            return null;
        }
        private void CheckComplete()
        {
            if (PayAudit.Status == 3)
            {
                CanEdit = false;
                CanChecked = false;
                CanBack = false;
                CanCancel = false;
            }
        }
        //是否可以驳回
        private void CheckOnBack()
        {
            var count = BusinessAudits.Count(o => o.Status > 0);
            if (count > 0 && count < BusinessAudits.Count)
            {
                var findItem = GetCurrent();
                if (findItem != null)
                {
                    CanBack = true;
                }
                else
                {
                    CanBack = false;
                }
            }

        }

        private async Task LoadAuditMappings()
        {
            if (ItemDefine.Id == 0)
                return;
            var items = await _auditMappingService.GetAllAuditMappings(PayAudit.Id, BusinessTypeName: "支付审核");
            AuditMappings.Clear();
            AuditMappings.AddRange(items.Items);

            await Task.CompletedTask; 
          
        } 
        /// <summary>
        /// 根据当前审核项 查询当前用户是否具有审核资格
        /// </summary>
        /// <param name="auditItem"></param>
        private bool IsCanAudit(BusinessAuditList auditItem)
        {
            var findItem = GetCurrent();
            if (findItem == null)
            {
                return false;
            }
            return true;
        }

        private void OnScanFile()
        {
            if (PayAudit.Id < 1)
            {
                MessageBox.Show("请先提交保存");
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

                    var scanParam = new FilesManage() { EntityId = PayAudit.Id, EntityKey = "PayAudit", EntityName = "支付审核", UploadType = viewModel.SelectedItem.Name };
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
                        GetFiles(PayAudit);
                    });

                    //var filePath = fileDialog.FileName;
                    //var fileName = fileDialog.SafeFileName;
                    //List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                    //keyValuePairs.Add(new KeyValuePair<string, string>("EntityId", ItemDefine.Id.ToString()));
                    //keyValuePairs.Add(new KeyValuePair<string, string>("FileName", fileName));
                    //keyValuePairs.Add(new KeyValuePair<string, string>("UploadType", viewModel.SelectedItem.Name));
                    //keyValuePairs.Add(new KeyValuePair<string, string>("EntityKey", "ItemDefine"));
                    //keyValuePairs.Add(new KeyValuePair<string, string>("EntityName", "立项登记"));
                    //var filemanage = await _filesService.UploadFileAsync(keyValuePairs, filePath, fileName);
                    //FilesManages.Add(filemanage);

                }
                int s = 0;
            });
            view.BindAction(notification.Finish);
        }


        [InjectionMethod]
        public void Init()
        {

            //InitBusinessAudits();
            //LoadAuditMappings();
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
        private async Task InitBusinessAudits()
        {
            var result = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName: "支付审核");
            BusinessAudits.Clear(); 
            foreach (var item in result.Items)
            {
                var model = Mapper.Map<BusinessAuditList>(item);
                BusinessAudits.Add(model);
            }

        }
      
        private async void UpdateStatus(int status)
        {
            PayAudit.Status = status;
            await _payAuditService.CreateOrUpdate(PayAudit);
        }
         
        /// <summary>
        /// 获取用户是否可审核
        /// </summary>
        private void CheckRole()
        {
            //角色是否可审核
            var findItem = GetCurrent();
            if (findItem != null)
            {
                CanChecked = true;
            }
            else
            {
                CanChecked = false;
                //审核完毕
            }
        }

        private async void LoadFundFrom()
        {
            var result = await _fundFromService.GetPageItems();
            if (result.datas.Count > 0)
            {
                FundItems.AddRange(result.datas);
            }
        }

        /// <summary>
        /// 立项支付
        /// </summary>
        private void OnSelectedItem()
        {
            var view = _unityContainer.Resolve<SelectedItemDefineView>();
            var notification = new Notification()
            {
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedItemDefineView;
                    var viewModel = selectView.DataContext as SelectedItemDefineViewModel;

                    this.ItemDefine.Id = viewModel.SelectedItem.Id;
                    this.ItemDefine.ItemName = viewModel.SelectedItem.ItemName;
                    this.ItemDefine.ItemNo = viewModel.SelectedItem.ItemNo;
                    PayAudit.ItemTotalAmount = ItemDefine.DefineAmount;
                    PayAudit.ItemDefineId = viewModel.SelectedItem.Id;
                    await GetItemDefineFiles(ItemDefine.Id);
                    var items = await UpdatePayDetail();
                    PayAudit.PaymentCount = items.Count + 1;
                    var id = PayAudit.PaymentCount.ToString().PadLeft(3, '0');
                    var no = $"{ItemDefine.ItemNo}_ZF{id}";

                    PayAudit.PaymentNo = no;


                }
                int s = 0;
            });
            view.BindAction(notification.Finish);

        }

        private async Task<List<PayAudit>> UpdatePayDetail()
        {
            var item = await _payAuditService.SearchPayCount(PayAudit.ItemDefineId);//支付次数
            //PayAudit.PaymentCount = item.Count + 1;
            //PayAudit.PaymentNo = ItemDefine.ItemNo;
            StringBuilder sb = new StringBuilder();
            sb.Append($"初始支付金额：{PayAudit.InitPayAmount}元 ");
            var index = 1;
            foreach (var model in item)
            {
                sb.Append($"第{index}次支付：{model.PayAmount}元 ");
                index++;
            }
            sb.Append($"累计支付：{item.Sum(o => o.PayAmount)}元 ");
            PayAudit.PayDetail = sb.ToString();
            return item;
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
        private void OnExportFlowDocumentCmd(object o)
        {
            try
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "666.docx");

                var file1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PayAudit.PaymentNo+".pdf");
                var content = "evento";
                // Open the document.
                Document doc = new Document(file);


                //// Replace the text in the document.
                doc.Range.Replace("$UnitName$", UnitName, new FindReplaceOptions(FindReplaceDirection.Forward));
                 
                doc.Range.Replace("$CreationTime$", PayAuditList.CreationTime.ToString("yyyy-MM-dd"), new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$ItemDefineName$", PayAuditList.ItemDefineName, new FindReplaceOptions(FindReplaceDirection.Forward));

                doc.Range.Replace("$DefineAmount$", PayAuditList.DefineAmount.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$ItemDescription$", ItemDefine.ItemDescription == null ? "" : ItemDefine?.ItemDescription, new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$FundItems$", PayAuditList.AccountName.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));

                doc.Range.Replace("$ContractName$", PayAuditList.ContractName, new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$ContractAmount$", PayAuditList.ContractAmount.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));


                doc.Range.Replace("$VendorName$", PayAuditList.VendorName, new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$PayDescription$", PayAudit.PayDetail, new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$PayAmount$", PayAudit.PayAmount.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));
                //doc.Range.Replace("CreatorUserName","ffff", new FindReplaceOptions(FindReplaceDirection.Forward));
                doc.Range.Replace("$PayProgress$", "第"+PayAudit.PaymentCount.ToString()+"次支付", new FindReplaceOptions(FindReplaceDirection.Forward));
                //doc.Range.Replace("CreationTime", PayAudit.PayAmount.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));
                var filesName = string.Join(",", FilesManages.Select(f => f.FileName).ToArray());
                 doc.Range.Replace("$FilesName$", filesName, new FindReplaceOptions(FindReplaceDirection.Forward));

                DocumentBuilder builder = new DocumentBuilder(doc);
                NodeCollection allTables = doc.GetChildNodes(NodeType.Table, true);
                builder.MoveToDocumentEnd();
                //builder.StartTable();
                Aspose.Words.Tables.Table table = allTables[0] as Aspose.Words.Tables.Table;//拿到第一个表格
                var rowCount = table.Count;
                //builder.InsertCell();
                //builder.Write("ROW1ASD");
                //builder.InsertCell();
                //builder.Write("asdsad");
                //builder.EndRow();
                //  DataTable products = this.GetData(); //数据源
                //builder.InsertCell();
                //builder.InsertCell();
                
                for (int i = 0; i <AuditMappings.Count; i++)
                {
                    var roww = table.Rows[10];
                    //var row = table.LastRow.Clone(true);
                    var row = roww.Clone(true);//复制第三行(绿色行)
                    table.Rows.Insert(11 + i, row);//将复制的行插入当前行的上方
                    var currentrow = table.Rows[11+i];
                    //Cell lshCell = table.Rows[11].Cells[0];
                    //lshCell.Range.Replace("$a1$", BusinessAudits[i].Name + "意见", new FindReplaceOptions(FindReplaceDirection.Forward));
                    currentrow.Range.Replace("$RoleName$", AuditMappings[i].RoleName + "意见", new FindReplaceOptions(FindReplaceDirection.Forward));
                    currentrow.Range.Replace("$AuditOpinion$", AuditMappings[i].AuditOpinion, new FindReplaceOptions(FindReplaceDirection.Forward));
                    currentrow.Range.Replace("$CreatorUserName$", AuditMappings[i].AuditUserName, new FindReplaceOptions(FindReplaceDirection.Forward));
                    currentrow.Range.Replace("$CreationTime$", AuditMappings[i].AuditTime==null?"": AuditMappings[i].AuditTime.Value.ToString("yyyy-MM-dd"), new FindReplaceOptions(FindReplaceDirection.Forward));
                    ////将单元格中的第一个段落移除
                    //lshCell.FirstParagraph.Remove();
                    ////新建一个段落
                    //Paragraph p = new Paragraph(doc);
                    ////设置一个string的值
                    //string value = "新建一个string的值";
                    ////把设置的值赋给之前新建的段落
                    //p.AppendChild(new Run(doc, value));
                    //将此段落加到单元格内
                    //lshCell.AppendChild(p); 
                    //builder.MoveToCell(0, 11+ i, 0, 0);
                     
                    //builder.MoveToCell(0, 11 + i, 1, 0);
                   // builder.Write("$Title$" + i.ToString()); 
                }
                builder.DeleteRow(0, 10);
                //doc.Range.Replace("$Title$", "真的啊", new FindReplaceOptions(FindReplaceDirection.Forward));
                //builder.MoveToCell(0, 3, 0, 0); //移动到第一个表格的第四行第一个格子
                //builder.Write("test"); //单元格填充文字
                int count = 0;

                //记录要显示多少列
                //for (var i = 0; i < products.Columns.Count; i)
                //{
                //    if (doc.Range.Bookmarks[products.Columns[i].ColumnName.Trim()] != null)
                //    {
                //        Bookmark mark = doc.Range.Bookmarks[products.Columns[i].ColumnName.Trim()];
                //        mark.Text = "";
                //        count;
                //    }

                //}
                //System.Collections.Generic.List listcolumn = new System.Collections.Generic.List(count);
                //for (var i = 0; i < count; i)
                //{
                //    builder.MoveToCell(0, 0, i, 0); //移动单元格
                //    if (builder.CurrentNode.NodeType == NodeType.BookmarkStart)
                //    {
                //        listcolumn.Add((builder.CurrentNode as BookmarkStart).Name);
                //    }
                //}
                double width = builder.CellFormat.Width;//获取单元格宽度
                //builder.MoveToBookmark("table"); //开始添加值
                //for (var m = 0; m < products.Rows.Count; m)
                //{
                //    for (var i = 0; i < listcolumn.Count; i)
                //    {
                //        builder.InsertCell(); // 添加一个单元格 
                //        builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                //        builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                //        builder.CellFormat.Width = width;
                //        builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                //        builder.Write(products.Rows[m][listcolumn[i]].ToString());
                //    }
                //    builder.EndRow();
                //}
                //doc.Range.Bookmarks["table"].Text = ""; // 清掉标示 
                //builder.DeleteRow(0, 5);
                // Save the modified document.
                doc.Save(file1,SaveFormat.Pdf);
                 

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
        private decimal _tempAmount;
        public decimal TempAmount
        {
            get { return _tempAmount; }
            set { SetProperty(ref _tempAmount, value); }
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
        public FundItem SelectFundItem
        {
            get { return _selectFundItem; }
            set { SetProperty(ref _selectFundItem, value); }
        }
        private PayAuditDetail _payAuditDetail;
        public PayAuditDetail PayAuditDetail
        {
            get { return _payAuditDetail; }
            set { SetProperty(ref _payAuditDetail, value); }
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
        private bool _canCancel = false;
        public bool CanCancel
        {
            get { return _canCancel; }
            set { SetProperty(ref _canCancel, value); }
        }
        private bool _canBack = false;
        public bool CanBack
        {
            get { return _canBack; }
            set { SetProperty(ref _canBack, value); }
        }
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }
        private string _paymentRemark;
        public string PaymentRemark  //附件备注
        {
            get { return _paymentRemark; }
            set { SetProperty(ref _paymentRemark, value); }
        }
        
        public FilesManage SelectedFile { get => _selectedFile; set => SetProperty(ref _selectedFile, value); }
    }

}
