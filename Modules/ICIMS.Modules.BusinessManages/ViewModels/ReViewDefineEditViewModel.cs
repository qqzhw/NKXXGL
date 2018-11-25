using AutoMapper;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
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
using Telerik.Windows.Controls;
using Unity;
using Unity.Attributes;
using Unity.Resolution;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
   
    public class ReViewDefineEditViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _unityContainer;
        private readonly IItemDefineService _itemDefineService;
        private readonly IFilesService _filesService;
        private readonly IWebApiClient _webApiClient;
        private readonly IBusinessAuditService _businessAuditService;
        private readonly IAuditMappingService _auditMappingService;
        private readonly IReViewDefineService _reViewDefineService;
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
        public ReViewDefineEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, ReViewDefineList data, IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, IReViewDefineService reViewDefineService)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _webApiClient = webApiClient;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService; 
            _reViewDefineService = reViewDefineService;
            _title = "评审审核";
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItemCategory);
            UploadCommand = new DelegateCommand(OnUploadedFiles); 
            _filesManages = new ObservableCollection<FilesManage>();
            _buinessAudits = new ObservableCollection<BusinessAudit>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            BindData(data);
        }

        internal void BindData(ReViewDefineList info)
        {
            if (info.ReViewDefine==null)
            {
                _itemDefine = new ItemDefine();
                _reviewDefine = new ReViewDefine();
                return;
            }
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
            ReViewDefine = info.ReViewDefine;
            GetFiles(ReViewDefine);
            LoadAuditMappings();
            LoadItemDefine(ReViewDefine.ItemDefineId);//加载立项项目
        }

        private async void LoadItemDefine(int Id)
        {
            if (Id>0)
            {
                var item =await _itemDefineService.GetById(Id);
            }
        }

        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }
        private async void GetFiles(ReViewDefine reViewDefine)
        {
            FilesManages.Clear();
            var items = await _filesService.GetAllFiles(reViewDefine.Id, "ReViewDefine");
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
            if (ReViewDefine==null||ReViewDefine.Id<1)
            {
                MessageBox.Show("请先保存评审");
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
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityId", ReViewDefine.Id.ToString()));
                        keyValuePairs.Add(new KeyValuePair<string, string>("FileName", fileName));
                        keyValuePairs.Add(new KeyValuePair<string, string>("UploadType", viewModel.SelectedItem.Name));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityKey", "ReViewDefine"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityName", "评审登记"));
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
            //ItemDefine.DefineAmount = 5000;
            //ItemDefine.DefineDate = DateTime.Now;
            //ItemDefine.ItemAddress = "成都";
            //ItemDefine.ItemCategoryId = 1;
            //ItemDefine.ItemDescription = "挥洒的阿萨德";
            //ItemDefine.ItemDocNo = "文号110";
            //ItemDefine.ItemName = "立项研究项目";
            //ItemDefine.Remark = "beizhu";
            if (ItemDefine.Id<1)
            {
                RadWindow.Confirm(new DialogParameters
                {
                    Content = "请选择评审项目", 
                    Owner = Application.Current.MainWindow
                });
                return;
            }
            // if (ItemDefine.Id==0)
            {
                ReViewDefine.ItemDefineId = ItemDefine.Id;
                var item = await _reViewDefineService.CreateOrUpdate(ReViewDefine);
                if (item.Id > 0)
                {
                    ReViewDefine.Id = item.Id;
                    ReViewDefine.ReViewNo = item.ReViewNo;
                }
            }


        }
        private async void OnSubmit()
        {
            var busaudits = await _businessAuditService.GetAllBusinessAudits(2);
            var auditItem = busaudits.Items.OrderBy(o => o.DisplayOrder).FirstOrDefault();
            if (auditItem == null)
                return;
            var auditmapping = new AuditMapping()
            {
                BusinessTypeId = 2,
                BusinessTypeName = "评审登记",
                ItemId = ReViewDefine.Id,
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
                    await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    UpdateAuditStatus();
                    LoadAuditMappings();
                }

            });
            view.BindAction(notification.Finish);
        }

        private void UpdateAuditStatus()
        {
            foreach (var item in BuinessAudits)
            {
                var findItem = AuditMappings.FirstOrDefault(o => o.BusinessAuditId == item.Id);
                if (findItem != null)
                {
                    item.Status = 1;
                    item.StatusName = "已审核";
                }
            }
        }

        private void OnCancel()
        {

        }

        private void OnBack()
        {
            throw new NotImplementedException();
        }

        private void OnShowLog()
        {

        }



        [InjectionMethod]
        public async void Init()
        {

            InitBusinessAudits();

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
        private void OnSelectedItemCategory()
        {
            var view = _unityContainer.Resolve<SelectedItemDefineView>();
            var notification = new Notification()
            {
                Title="项目立项列表",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectedItemDefineView;
                    var viewModel = selectView.DataContext as SelectedItemDefineViewModel;
                    ItemDefine = Mapper.Map<ItemDefine>(viewModel.SelectedItem);
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);

        }
        private ReViewDefine _reviewDefine;
        public ReViewDefine ReViewDefine
        {
            get { return _reviewDefine; }
            set { SetProperty(ref _reviewDefine, value); }
        } 
        private ItemDefine _itemDefine;
        public ItemDefine ItemDefine
        {
            get { return _itemDefine; }
            set { SetProperty(ref _itemDefine, value); }
        }
    }


}
