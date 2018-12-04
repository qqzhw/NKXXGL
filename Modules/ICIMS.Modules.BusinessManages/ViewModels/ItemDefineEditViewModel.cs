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
using Unity.Resolution;
using ICIMS.Model.User;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
    public class ItemDefineEditViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _unityContainer;
        private readonly IItemDefineService _itemDefineService;
        private readonly IFilesService _filesService;
        private readonly IWebApiClient _webApiClient;
        private readonly IBusinessAuditService _businessAuditService;
        private readonly IAuditMappingService _auditMappingService;
        private readonly UserModel _userModel;
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
        public ItemDefineEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, ItemDefineList data, IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, UserModel userModel)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _webApiClient = webApiClient;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService;
            _title = "项目立项";
            _userModel = userModel;
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            //LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItemCategory);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            _itemDefine = new ItemDefine();
            _filesManages = new ObservableCollection<FilesManage>();
            _buinessAudits = new ObservableCollection<BusinessAudit>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            BindData(data);
        }

        internal void BindData(ItemDefineList info)
        {
            InitBusinessAudits();
            if (info.Id == 0)
            {
                ItemDefine.DefineDate = DateTime.Now;
                ItemDefine.UnitId = _userModel.UnitId;
                ItemDefine.UnitName = _userModel.UnitName;
                return;
            }
            
            ItemDefine = Mapper.Map<ItemDefine>(info);
            GetFiles(ItemDefine);
           
        }
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }
        private async void GetFiles(ItemDefine itemDefine)
        { 
            var items = await _filesService.GetAllFiles(itemDefine.Id, "ItemDefine"); 
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
            if (ItemDefine.Id < 1)
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
                        keyValuePairs.Add(new KeyValuePair<string, string>("EntityId", ItemDefine.Id.ToString()));
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
            //ItemDefine.DefineAmount = 5000;
            //ItemDefine.DefineDate = DateTime.Now;
            //ItemDefine.ItemAddress = "成都";
            //ItemDefine.ItemCategoryId = 1;
            //ItemDefine.ItemDescription = "挥洒的阿萨德";
            //ItemDefine.ItemDocNo = "文号110";
            //ItemDefine.ItemName = "立项研究项目";
            //ItemDefine.Remark = "beizhu";
            
            
           // if (ItemDefine.Id==0)
            {
               var item= await _itemDefineService.CreateOrUpdate(ItemDefine);
                if (item.Id>0)
                {
                    ItemDefine.Id = item.Id;
                }
            }
        

        }
        private async void OnSubmit()
        {
            var busaudits = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName:"立项登记");
            var auditItem = busaudits.Items.OrderBy(o => o.DisplayOrder).FirstOrDefault();
            if (auditItem == null)
                return;
            
            var auditmapping = new AuditMapping()
            {
                BusinessTypeId=2,
                BusinessTypeName="立项登记",
                ItemId=ItemDefine.Id,
                BusinessAuditId=auditItem.Id,
                DisplayOrder=auditItem.DisplayOrder
            };
            
            var view = _unityContainer.Resolve<SubmitAuditView>(new ParameterOverride("data", auditmapping));
            var notification = new Notification()
            {
                Title = "审核", 
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, async(callback) => {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SubmitAuditView;
                    var viewModel = selectView.DataContext as SubmitAuditViewModel;
                    await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    UpdateAuditStatus();
                    //LoadAuditMappings();
                    InitBusinessAudits();
                }
                
            });
            view.BindAction(notification.Finish);
        }

        private void UpdateAuditStatus()
        {
            foreach (var item in BuinessAudits)
            {
                var findItem = AuditMappings.FirstOrDefault(o => o.BusinessAuditId == item.Id);
                if (findItem!=null)
                {
                    item.Status = 1;
                    item.StatusName = "已审核";
                }
            }
        }

        private async void OnCancel()
        {
            var deleteItem = AuditMappings.LastOrDefault(o => o.Status == 1);
            if (deleteItem!=null)
            {
                await _auditMappingService.Delete(deleteItem.Id);
            } 
        }

        private void OnBack()
        {
            
        }

        

        [InjectionMethod]
        public async void Init()
        {

                        
        }

        private async void InitBusinessAudits()
        { 
            var items = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName:"立项登记");
            BuinessAudits.Clear();
            BuinessAudits.AddRange(items.Items);
            LoadAuditMappings();
        }
        private async void LoadAuditMappings()
        {
            if (ItemDefine.Id == 0)
                return;
            var items = await _auditMappingService.GetAllAuditMappings(ItemDefine.Id, 2);
            AuditMappings.Clear();
            _auditMappings.AddRange(items.Items);
            if (AuditMappings.Count>0)
            {
                foreach (var item in BuinessAudits)
                {
                    var findItem = AuditMappings.FirstOrDefault(o => o.RoleId == item.RoleId & o.BusinessAuditId == item.Id);
                    if (findItem!=null)
                    {
                        item.IsChecked = true;
                        item.StatusName = "已审核";
                    }
                }
                CanEdit = false;
            }
            CheckRole();
        }


        /// <summary>
        /// 获取用户是否是审核角色
        /// </summary>
        private void CheckRole()
        {
            //角色是否可审核
            var findItem = BuinessAudits.Where(o => !o.IsChecked).OrderBy(o => o.DisplayOrder).FirstOrDefault();
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
            var view = _unityContainer.Resolve<SelectItemCategoryView>();
            var notification = new Notification()
            {
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    var selectView = callback.Content as SelectItemCategoryView;
                    var viewModel = selectView.DataContext as SelectItemCategoryViewModel;
                    this.ItemDefine.ItemCategoryId = viewModel.SelectedItem.Id;
                    this.ItemDefine.ItemCategoryName = viewModel.SelectedItem.Name;
                } 
            });
            view.BindAction(notification.Finish);

        }
        private bool _iscansave;
        public bool IsCanSave
        {
            get { return _iscansave; }
            set { SetProperty(ref _iscansave, value); }
        }

        private ItemDefine _itemDefine;
        public ItemDefine ItemDefine
        {
            get { return _itemDefine; }
            set { SetProperty(ref _itemDefine, value); }
        }
        private  bool _canEdit=true;
        public  bool CanEdit
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
