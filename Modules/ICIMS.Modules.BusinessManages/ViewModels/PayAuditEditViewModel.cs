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
            ScanCommand = new DelegateCommand(OnScanFile);
            _filesManages = new ObservableCollection<FilesManage>();
            _buinessAudits = new ObservableCollection<BusinessAudit>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            _fundItems = new ObservableCollection<FundItem>();
            BindData(data);
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
            var findItem = PayAuditDetails.FirstOrDefault(o=>o.FundName==SelectFundItem.Name);
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
            InitBusinessAudits();
            UnitName = _userModel.UnitName;
            _payAudit = null ?? new PayAudit();
            _itemDefine = null ?? new ItemDefine();
            PaymentTypeItem = null ?? new PaymentTypeItem();
            _contract = null ?? new ContractList();
            _vendorItem = null ?? new VendorItem();
            _payAuditDetails = null ?? new ObservableCollection<PayAuditDetail>();
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
            _payAuditDetails = null ?? new ObservableCollection<PayAuditDetail>();
            //PayAuditDetails.Add(new PayAuditDetail()
            //{
            //    Amount = 1000,
            //    FundName = "市局",
            //    Remark = "市局款项",
            //});
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
            var items = await _filesService.GetAllFiles(payAudit.Id, "PayAudit");
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
                PayAudit.PaymentNo = item.PaymentNo;
                MessageBox.Show("保存成功！");
            }
            else 
            MessageBox.Show("保存失败！");
        }
        private void OnSubmit()
        {

            var auditItem = BuinessAudits.Where(o => !o.IsChecked).OrderBy(o => o.DisplayOrder).FirstOrDefault();
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
            PopupWindows.NotificationRequest.Raise(notification, async (callback) => {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                    var item= await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    if (item.Id > 0)
                    {
                        UpdateAuditStatus();
                        //LoadAuditMappings();
                        InitBusinessAudits();
                        UpdateStatus(1);
                    }
                    else
                    {
                        MessageBox.Show("审核失败");
                    }
                }

            });
            view.BindAction(notification.Finish);
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
            var auditItem = BuinessAudits.Where(o => o.Status==1).OrderBy(o => o.DisplayOrder).FirstOrDefault();
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
            PopupWindows.NotificationRequest.Raise(notification, async (callback) => {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                    viewModel.AuditMapping.Status = 2; //驳回审核
                    var item=await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    if (item.Id > 0)
                    {
                        UpdateAuditStatus();
                        //LoadAuditMappings();
                        InitBusinessAudits();
                        UpdateStatus(2);
                    }
                    else
                    {
                        MessageBox.Show("驳回失败");
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
        /// 根据当前审核项 查询当前用户是否具有审核资格
        /// </summary>
        /// <param name="auditItem"></param>
        private bool IsCanAudit(BusinessAudit auditItem)
        {

            var findItem = _userModel.Roles.FirstOrDefault(o => o.Id == auditItem.RoleId);
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
        public  void Init()
        {
            
            //InitBusinessAudits();
            //LoadAuditMappings();
            LoadFundFrom();//加载资金来源
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
                    item.Status =findItem.Status;
                    item.StatusName =findItem.StatusText;
                }
            }
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
            var items = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName: "支付审核");
            BuinessAudits.Clear();
            BuinessAudits.AddRange(items.Items);
            LoadAuditMappings();
             
        }
        private async void LoadAuditMappings()
        {
            if (PayAudit.Id == 0)
                return;
            var items = await _auditMappingService.GetAllAuditMappings(PayAudit.Id, BusinessTypeName: "支付审核");
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
                CanEdit = false;
                var isComplete = BuinessAudits.FirstOrDefault(o => o.Status == 0);
                if (isComplete == null)
                {
                    PayAudit.Status = 3;
                    await _payAuditService.CreateOrUpdate(PayAudit);
                }
                else
                {
                   
                }
            }
            CheckRole();
        }
        private async void UpdateStatus(int status)
        {
            PayAudit.Status = status;
            await _payAuditService.CreateOrUpdate(PayAudit);
        }
        /// <summary>
        /// 获取用户是否是审核角色
        /// </summary>
        private void CheckRole()
        {
            //角色是否可审核
            var findItem = BuinessAudits.Where(o => o.Status==0).OrderBy(o => o.DisplayOrder).FirstOrDefault();
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
        private async void LoadFundFrom()
        {
            var result = await _fundFromService.GetPageItems();
            if (result.datas.Count>0)
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
            PopupWindows.NotificationRequest.Raise(notification, async(callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedItemDefineView;
                    var viewModel = selectView.DataContext as SelectedItemDefineViewModel;
                    
                    this.ItemDefine.Id = viewModel.SelectedItem.Id;
                    this.ItemDefine.ItemName = viewModel.SelectedItem.ItemName;
                    PayAudit.ItemTotalAmount = ItemDefine.DefineAmount;
                   
                  await  GetItemDefineFiles(ItemDefine.Id);
                  var item = await _payAuditService.SearchPayCount(ItemDefine.Id);//支付次数
                    PayAudit.PaymentCount = item.Count + 1;
                    PayAudit.PaymentNo = ItemDefine.ItemNo;
                    StringBuilder sb = new StringBuilder();
                    foreach (var model in item)
                    {
                        sb.Append($"第一次支付：{model.PayAmount}元");
                    }
                    PayAudit.PayDetail = sb.ToString();
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);

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
        private  void OnExportFlowDocumentCmd(object o)
        {
            try
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "666.docx");

                var file1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "777.docx");
                var content = "evento";
                // Open the document.
                Document doc = new Document(file);

              
                //// Replace the text in the document.
                //doc.Range.Replace("$Title$", DateTime.Now.ToString(), new FindReplaceOptions(FindReplaceDirection.Forward));
                //string cmdNo = string.Format("成联指【{0}】号", 511);
                //doc.Range.Replace("$CmdNo$", cmdNo, new FindReplaceOptions(FindReplaceDirection.Forward));
                //doc.Range.Replace("$HostTypeName$","", new FindReplaceOptions(FindReplaceDirection.Forward));

                //doc.Range.Replace("$EventSourceFiredTime$", DateTime.Now.ToString("yyyy年MM月dd日 HH点mm分"), new FindReplaceOptions(FindReplaceDirection.Forward));
                //doc.Range.Replace("$InfoContent$", "currentEventInfo.EventInfoContent", new FindReplaceOptions(FindReplaceDirection.Forward));
                //doc.Range.Replace("$EventSourcePerson$", "currentEventInfo.Clrxm", new FindReplaceOptions(FindReplaceDirection.Forward));

                //doc.Range.Replace("$EventSourceDepartment$"," currentEventInfo.EventSourceDepartment", new FindReplaceOptions(FindReplaceDirection.Forward));
                //doc.Range.Replace("$Content$", content, new FindReplaceOptions(FindReplaceDirection.Forward));


                //doc.Range.Replace("$EndDescription$", "", new FindReplaceOptions(FindReplaceDirection.Forward));

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
                for (int i = 0; i < 3; i++)
                {
                    var roww = table.Rows[rowCount-2];
                    //var row = table.LastRow.Clone(true);
                    var row = roww.Clone(true);//复制第三行(绿色行)
                    table.Rows.Insert(rowCount + i, row);//将复制的行插入当前行的上方
                    builder.MoveToCell(0, rowCount+i, 0, 0);
                    builder.Writeln($"asdas还是审核人{i+1}");
                    builder.MoveToCell(0, rowCount + i, 1, 0);
                    builder.Write("$Title$" + i.ToString());
                }
                doc.Range.Replace("$Title$", "真的啊", new FindReplaceOptions(FindReplaceDirection.Forward));
                //builder.MoveToCell(0, 3, 0, 0); //移动到第一个表格的第四行第一个格子
                builder.Write("test"); //单元格填充文字
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
        public  FundItem SelectFundItem
        {
            get { return _selectFundItem; }
            set { SetProperty(ref _selectFundItem, value); }
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
