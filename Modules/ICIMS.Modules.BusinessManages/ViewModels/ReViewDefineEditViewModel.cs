using AutoMapper;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
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
using System.Net;
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
        private readonly IBusinessAuditStatusService _businessAuditStatusService;
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand BrowseCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand ScanCommand { get; private set; }
        public DelegateCommand SearchItemCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand UploadCommand { get; private set; }
        public DelegateCommand DownloadCommand { get; private set; }
        private readonly SettingModel _settingModel;
        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private FilesManage _selectedFile;
        public FilesManage SelectedFile { get => _selectedFile; set => SetProperty(ref _selectedFile, value); }
        public ReViewDefineEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, ReViewDefineList data, IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, IReViewDefineService reViewDefineService, UserModel userModel,SettingModel settingModel, IBusinessAuditStatusService businessAuditStatusService)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _webApiClient = webApiClient;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService; 
            _reViewDefineService = reViewDefineService;
            _businessAuditStatusService = businessAuditStatusService;
            _title = "评审审核";
            _userModel = userModel;
            _settingModel = settingModel;
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            BrowseCommand = new DelegateCommand(OnBrowseCommand);
            ScanCommand = new DelegateCommand(OnScanFile);
            SearchItemCommand = new DelegateCommand(OnSelectedItemCategory);
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            DownloadCommand = new DelegateCommand(OnDownloadCommand);
            _filesManages = new ObservableCollection<FilesManage>();
            _businessAudits = new ObservableCollection<BusinessAuditList>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            BindData(data);
        }

        private void OnDownloadCommand()
        {
            OnDownloadFile(SelectedFile);
        }

        private async void OnDeleteCommand()
        {
            try
            {
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
        private void OnBrowseCommand()
        {
            var permission = _userModel.Permissions.FirstOrDefault(o => o == "Pages.FilesManage");
            if (permission == null)
            {
                MessageBox.Show("你没有权限操作此项");
            }
            if (SelectedFile == null || string.IsNullOrEmpty(SelectedFile.DownloadUrl))
                return;
            using (WebClient client = new WebClient())
            {
                var url = _settingModel.ServerApi + SelectedFile.DownloadUrl.Replace("\\", "/").Replace("//", "/");
                var saveFilePath = AppDomain.CurrentDomain.BaseDirectory + SelectedFile.FileName;// Properties.Settings.Default.LocalPath + name;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadFileAsync(new Uri(url), saveFilePath);
            }

        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + SelectedFile.FileName);
            }
            catch
            {


            }

        }


        internal async void BindData(ReViewDefineList info)
        {
           await InitBusinessAudits();
            if (info.ReViewDefine==null)
            {
                _itemDefine = new ItemDefine();
                _reviewDefine = new ReViewDefine();
                ReViewDefine.UnitName = _userModel.UnitName; 
                RaisePropertyChanged("ReViewDefine");
                CanEdit = true;
                return;
            }
            
            ReViewDefine = info.ReViewDefine;
            ReViewDefine.UnitName = info.UnitName; 
            GetFiles(ReViewDefine); 
            await LoadItemDefine(ReViewDefine.ItemDefineId);//加载立项项目 
            await GetNewStatus();
            await LoadAuditMappings();
        }
        private void OnScanFile()
        {
            if (ReViewDefine.Id < 1)
            {
                MessageBox.Show("请先保存评审登记");
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

                    var scanParam = new FilesManage() { EntityId = ReViewDefine.Id, EntityKey = "ReViewDefine", EntityName = "评审登记", UploadType = viewModel.SelectedItem.Name };
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
                        GetFiles(ReViewDefine);
                    }); 

                }
                int s = 0;
            });
            view.BindAction(notification.Finish);
        }


        private async Task LoadItemDefine(int Id)
        {
            if (Id>0)
            {
                var item =await _itemDefineService.GetById(Id);
                ItemDefine = item;
                await GetItemDefineFiles(Id);
            }
        }

        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
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
           
            await  Task.CompletedTask;
        }
        private async void GetFiles(ReViewDefine reViewDefine)
        { 
            var items = await _filesService.GetAllFiles(reViewDefine.Id, "ReViewDefine");

            foreach (var item in items.Items)
            {
                if (FilesManages.Contains(item))
                    continue;
                FilesManages.Add(item);
            }

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
            if (ReViewDefine==null||ReViewDefine.Id<1)
            {
                MessageBox.Show("请先保存评审");
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
            });
            view.BindAction(notification.Finish);
        }

        private async void OnSave()
        {
            
            if (ItemDefine.Id<1)
            {
                RadWindow.Confirm(new DialogParameters
                {
                    Content = "请选择评审项目", 
                    Owner = Application.Current.MainWindow
                });
                return;
            }
            try
            {

                ReViewDefine.ItemDefineId = ItemDefine.Id;
                var item = await _reViewDefineService.CreateOrUpdate(ReViewDefine);
                if (item.Id > 0)
                {
                    CanChecked = true;
                    ReViewDefine.Id = item.Id;
                    ReViewDefine.ReViewNo = item.ReViewNo;
                    MessageBox.Show("保存成功！");
                }
                else
                {
                    MessageBox.Show("保存失败！");
                }  
                await GetNewStatus();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          


        }
        private  void OnSubmit()
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
                BusinessTypeId = 3,
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
                    viewModel.AuditMapping.Status = 1;
                    try
                    {
                        var item = await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                        if (item.Id > 0)
                        {
                            auditItem.Status = 1;

                            await UpdateBusinessAudit(auditItem, ReViewDefine.Id, 1);

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
                            CheckComplete();
                        }
                        else
                        {
                            MessageBox.Show("审核失败");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                 
                }

            });
            view.BindAction(notification.Finish);
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
         
        private async void OnCancel()
        { 
            if (ReViewDefine.Status == 3)
                return;
            var findItem = GetCheckedItem();
            if (findItem == null)
                return;
            try
            {  
                var findmap = AuditMappings.LastOrDefault(o => _userModel.Roles.FirstOrDefault(r => r.Id == o.RoleId) != null);
                await _auditMappingService.Delete(findmap.Id);
                await UpdateBusinessAudit(findItem, ReViewDefine.Id, 0);

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
            var auditItem = BusinessAudits.OrderBy(o=>o.DisplayOrder).LastOrDefault(o => o.Status == 1);
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
                BusinessTypeName = "评审登记",
                ItemId = ReViewDefine.Id,
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
                   var item= await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                    if (item.Id > 0)
                    {
                        
                        UpdateStatus(2);//标记立项处于已驳回状态
                       
                       
                        await UpdateBusinessAudit(auditItem, ReViewDefine.Id, 2);
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
                               // UpdateStatus(0);
                            }
                        }
                        await LoadAuditMappings();
                    }
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
       



        [InjectionMethod]
        public async void Init()
        {

           // InitBusinessAudits();

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
        /// <summary>
        /// 初始化加载
        /// </summary>
        private async Task InitBusinessAudits()
        {
            var result = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName:"评审登记");             
            BusinessAudits.Clear();
            foreach (var item in result.Items)
            {
                var model = Mapper.Map<BusinessAuditList>(item);
                BusinessAudits.Add(model);
            }
        }
        
        private async Task LoadAuditMappings()
        { 
            if (ReViewDefine.Id == 0)
              await  Task.CompletedTask;
            var items = await _auditMappingService.GetAllAuditMappings(ReViewDefine.Id, BusinessTypeName: "评审登记");
             
            AuditMappings.Clear();
            AuditMappings.AddRange(items.Items);
            foreach (var item in BusinessAudits)
            {
                var findItem = AuditMappings.FirstOrDefault(o => o.RoleId == item.RoleId && o.BusinessAuditId == item.Id);
                if (findItem != null)
                {
                    item.AuditUserName = findItem.AuditUserName;
                    item.AuditTime = findItem.AuditTime;
                }
            }
            await Task.CompletedTask;
        }

        private async Task GetNewStatus()
        {
            if (ReViewDefine.Id > 0)
            {
                var result = await _businessAuditService.GetAll(BusinessTypeName: "评审登记", entityId: ReViewDefine.Id);

                foreach (var item in result.Items)
                {
                    var model = BusinessAudits.FirstOrDefault(o => o.Id == item.Id);
                    if (model != null)
                    {
                        model.BusinessAuditStatusId = item.BusinessAuditStatusId;
                        model.EntityId = ReViewDefine.Id;
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
        /// 是否可取消审批
        /// </summary>
        private void CheckAudit()
        {
            var findItem = GetCheckedItem();
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
            if (ReViewDefine.Status == 3)
            {
                CanEdit = false;
                CanChecked = false;
                CanBack = false;
                CanCancel = false;
            }
        }
        private async void UpdateStatus(int status)
        {
            ReViewDefine.Status = status;
            var item=await _reViewDefineService.CreateOrUpdate(ReViewDefine); 
            
        }
        /// <summary>
        /// 获取用户是否是审核角色
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
            }
        }
        private void OnSelectedItemCategory()
        {
            var view = _unityContainer.Resolve<SelectedItemDefineView>();
            var notification = new Notification()
            {
                Title="项目立项列表",
                Content = view,
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
        private string _unitName;
        public string UnitName
        {
            get { return _unitName; }
            set { SetProperty(ref _unitName, value); }
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
    }


}
