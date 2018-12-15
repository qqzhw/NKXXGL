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
        private readonly IBusinessAuditStatusService _businessAuditStatusService;
        private readonly UserModel _userModel;
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand ScanCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand SearchItemCommand { get; private set; }
        public DelegateCommand UploadCommand { get; private set; }
        public DelegateCommand BrowseCommand { get; private set; }
        public DelegateCommand DownloadCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; set; }


        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        public ItemDefineEditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, ItemDefineList data, IItemDefineService itemDefineService, IFilesService filesService, IWebApiClient webApiClient, IBusinessAuditService businessAuditService, IAuditMappingService auditMappingService, UserModel userModel, IBusinessAuditStatusService businessAuditStatusService)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _itemDefineService = itemDefineService;
            _filesService = filesService;
            _webApiClient = webApiClient;
            _businessAuditService = businessAuditService;
            _auditMappingService = auditMappingService;
            _businessAuditStatusService = businessAuditStatusService;
            _title = "项目立项";
            _userModel = userModel;
            SaveCommand = new DelegateCommand(OnSave);
            SubmitCommand = new DelegateCommand(OnSubmit);
            CancelCommand = new DelegateCommand(OnCancel);
            BackCommand = new DelegateCommand(OnBack);
            ScanCommand = new DelegateCommand(OnScanFile);
            BrowseCommand = new DelegateCommand(OnBrowseCommand);
            DownloadCommand = new DelegateCommand(OnDownloadCommand);
            //LogCommand = new DelegateCommand(OnShowLog);
            SearchItemCommand = new DelegateCommand(OnSelectedItemCategory);
            UploadCommand = new DelegateCommand(OnUploadedFiles);
            _itemDefine = new ItemDefine();
            _filesManages = new ObservableCollection<FilesManage>();
            _businessAudits = new ObservableCollection<BusinessAuditList>();
            _auditMappings = new ObservableCollection<AuditMapping>();
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
            BindData(data);
        }

        private async void OnDeleteCommand()
        {
            try
            {
                await this._filesService.Delete((long)this.SelectedFile.EntityId);
                this.FilesManages.Remove(this.SelectedFile);
                this.SelectedFile = this.FilesManages.FirstOrDefault();
                MessageBox.Show("删除成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void OnDownloadCommand()
        {
            var file = this.SelectedFile;
            OnDownloadFile(file);
        }

        private void OnBrowseCommand()
        {
        }

        private void OnScanFile()
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
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification,  (callback) =>
            {
                if (callback.DialogResult == true)
                {
                    //选择文档类型
                    var selectView = callback.Content as SelectedDocumentType;
                    var viewModel = selectView.DataContext as SelectedDocumentTypeModel;
                    if (viewModel.SelectedItem == null)
                        return;

                    var scanParam = new FilesManage() { EntityId = ItemDefine.Id, EntityKey = "ItemDefine", EntityName = "立项登记", UploadType = viewModel.SelectedItem.Name };
                        var scanView = _unityContainer.Resolve<ScanFileView>(new ParameterOverride("data", scanParam));
                        var notify= new Notification()
                        {
                            Title = "文档扫描",
                            Content = scanView,
                        };
                        PopupWindows.NotificationRequest.Raise(notify, (scanBack) =>
                        {
                            var fileView = scanBack.Content as ScanFileView;
                            var scanFileViewmodel = fileView.DataContext as ScanFileViewModel;
                            scanFileViewmodel.Dispose();
                            GetFiles(ItemDefine);
                        }); 
                  
                }
                int s = 0;
            });
            view.BindAction(notification.Finish);
        }
        
        private void OnDownloadFile(FilesManage model)
        {
            var permission = _userModel.Permissions.FirstOrDefault(o => o == "Pages.FilesManage");
            if (permission==null)
            {
                MessageBox.Show("你没有权限操作此项");
            }
            if (model == null||string.IsNullOrEmpty(model.DownloadUrl))
                return;
            //var notification = new Notification()
            //{
            //    Title = "文件下载",
            //    Content = _unityContainer.Resolve<FileDownloadView>(new ParameterOverride("fileName", model.FullName)),
            //};
      
            var view = _unityContainer.Resolve<FileDownloadView>(new ParameterOverride("model", model));
            var notification = new Notification()
            {
                Title = "附件下载",
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification,  (callback) => { });
        }
        internal async void BindData(ItemDefineList info)
        {
            await InitBusinessAudits();
            ItemDefine = Mapper.Map<ItemDefine>(info);
            if (info.Id == 0)
            {
                ItemDefine.Status = 0;
                ItemDefine.DefineDate = DateTime.Now;
                ItemDefine.UnitId = _userModel.UnitId;
                ItemDefine.UnitName = _userModel.UnitName;
                CanEdit = true; 
                return;
            } 
            
            GetFiles(ItemDefine);
            LoadAuditMappings();
            await  GetNewStatus();
            

        }
        private ObservableCollection<FilesManage> _filesManages;
        public ObservableCollection<FilesManage> FilesManages
        {
            get { return _filesManages; }
            set { SetProperty(ref _filesManages, value); }
        }

        private FilesManage _selectedFile;
    
        private async void GetFiles(ItemDefine itemDefine)
        { 
            var items = await _filesService.GetAllFiles(itemDefine.Id, "ItemDefine"); 
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
            try
            {
                if (ItemDefine.ItemCategoryId==0)
                {
                    MessageBox.Show("请选择项目分类");
                    return;
                }
               var item= await _itemDefineService.CreateOrUpdate(ItemDefine);
                if (item.Id>0)
                {
                    //foreach (var businessaudit in BuinessAudits)
                    //{
                    //    var auditStatus = new BusinessAuditStatus()
                    //    {
                    //        BusinessAuditId=businessaudit.Id,
                    //        BusinessTypeName=businessaudit.BusinessTypeName,
                    //        DisplayOrder=businessaudit.DisplayOrder,
                    //        EntityId=item.Id,
                    //        RoleId=businessaudit.RoleId,
                    //        RoleName=businessaudit.RoleName,                           
                    //    };
                    //   await _businessAuditStatusService.CreateOrUpdate(auditStatus);
                    //}
                    CanChecked = true;
                    ItemDefine.ItemNo = item.ItemNo;
                    ItemDefine.Id = item.Id; 
                    await GetNewStatus();
                    MessageBox.Show("保存成功！");
                }
                else
                MessageBox.Show("保存失败！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("保存失败："+ex.Message);
            }
            await Task.CompletedTask; 
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
                    viewModel.AuditMapping.Status = 1;
                    viewModel.AuditMapping.BusinessAuditStatusId = auditItem.BusinessAuditStatusId;
                    try
                    {
                        var item = await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);
                        if (item.Id > 0)
                        {
                            
                          
                            
                            UpdateBusinessAudit(auditItem, ItemDefine.Id, 1);

                            auditItem.Status = 1;
                            await GetNewStatus();
                            var completed = BusinessAudits.Count(o => o.Status == 1);
                            if (completed == BusinessAudits.Count)
                            {
                                UpdateStatus(3);
                            }
                            else
                            {
                                UpdateStatus(1);//标记立项处于审核中状态
                            }
                            LoadAuditMappings();
                        }
                    }
                    catch (Exception)
                    { 

                    }
                   
                   
                }
                
            });
            view.BindAction(notification.Finish);
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
         

        private async void OnCancel()
        {
            if (ItemDefine.Status == 3)
                return;
            var findItem = BusinessAudits.LastOrDefault(o => o.Status == 1&&_userModel.Roles.FirstOrDefault(r=>r.Id==o.RoleId)!=null);
            if (findItem == null)
                return; 
            try
            { 
                //var findItem = BuinessAudits.LastOrDefault(o => o.Status == 1);
                var findmap = AuditMappings.LastOrDefault(o =>_userModel.Roles.FirstOrDefault(r=>r.Id==o.RoleId)!=null);
                await _auditMappingService.Delete(findmap.Id);
                 UpdateBusinessAudit(findItem, ItemDefine.Id, 0);
                 
                if (BusinessAudits.FirstOrDefault(o=>o.Status>0)==null)
                {
                    UpdateStatus(0);//标记立项处于制单中
                }
               
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="item"></param>
        /// <param name="entityId"></param>
        /// <param name="status"></param>
        private async Task UpdateBusinessAudit(BusinessAuditList item,int entityId,int status)
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
        }
        /// <summary>
        /// 驳回审核
        /// </summary>
        private void OnBack()
        {
            var auditItem = BusinessAudits.Where(o =>o.Status==1).OrderBy(o => o.DisplayOrder).LastOrDefault();
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
                BusinessTypeName = "立项登记",
                ItemId = ItemDefine.Id,
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
                    viewModel.AuditMapping.BusinessAuditStatusId = auditItem.BusinessAuditStatusId;
                   var item= await _auditMappingService.CreateOrUpdate(viewModel.AuditMapping);

                    if (item.Id>0)
                    {
                        
                        //LoadAuditMappings();
                        
                        UpdateStatus(2);//标记立项处于已驳回状态
                        //var status =await _businessAuditStatusService.GetById(auditItem.BusinessAuditStatusId);
                        //if (status!=null)
                        //{
                        //    status.Status = 0;
                        //    await _businessAuditStatusService.CreateOrUpdate(status);
                        //}
                        UpdateBusinessAudit(auditItem, ItemDefine.Id, 0);
                        await GetNewStatus();//获取最新状态
                        auditItem.Status = 1;
                        await GetNewStatus();
                        var completed = BusinessAudits.Count(o => o.Status == 1);
                        if (completed == BusinessAudits.Count)
                        {
                            UpdateStatus(3);
                        }
                        else
                        {
                            //UpdateStatus(1);//标记立项处于审核中状态
                            if (completed==0)
                            {
                                UpdateStatus(0);
                            }
                        }
                        LoadAuditMappings();
                    }
                  
                }

            });
            view.BindAction(notification.Finish);
        }

        

        [InjectionMethod]
        public async void Init()
        {

                        
        }
        //初始加载 只加载一次
        private async Task InitBusinessAudits()
        { 
            var result = await _businessAuditService.GetAllBusinessAudits(BusinessTypeName:"立项登记");
            BusinessAudits.Clear();
            foreach (var item in result.Items)
            {
                var model = Mapper.Map<BusinessAuditList>(item);
                BusinessAudits.Add(model);
            }
           
        }

        private async Task GetNewStatus()
        {
            if (ItemDefine.Id>0)
            {
                var result = await _businessAuditService.GetAll(BusinessTypeName: "立项登记", entityId: ItemDefine.Id);
                 
                foreach (var item in result.Items)
                {
                    var model =BusinessAudits.FirstOrDefault(o=>o.Id==item.Id);
                    if (model!=null)
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
        }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        private void CheckEdit()
        {
            //查询当前是否有审核项 true 不能编辑
            var findItem = BusinessAudits.FirstOrDefault(o => o.Status > 0);
            if (findItem!=null)
            {
                CanEdit = false;
            }
            else
            {
                CanEdit = true;
                //var check = BusinessAudits.OrderBy(o => o.DisplayOrder).FirstOrDefault(o=>o.Status==0);
                //var item = _userModel.Roles.FirstOrDefault(o => o.Id == check.RoleId);
                //if (item==null)
                //{
                //    CanEdit = false;
                //}
                //else
                //{
                //    CanEdit = true;
                //}
                
            }
        }
        /// <summary>
        /// 是否可审批
        /// </summary>
        private void CheckAudit()
        {
            var findItem =BusinessAudits.LastOrDefault(o=>o.Status==1&&_userModel.Roles.FirstOrDefault(r=>r.Id==o.RoleId)!=null);
            if (findItem!=null)
            { 
                if (findItem != null)
                {
                    CanCancel = true;
                }
            }
        }
        //获取当前待审批项 无审核项返回null
        public BusinessAuditList GetCurrent()
        {
            var findItem = BusinessAudits.OrderBy(o => o.DisplayOrder).FirstOrDefault(o => o.Status == 0 && _userModel.Roles.FirstOrDefault(r => r.Id == o.RoleId)!=null);
            if (findItem!=null)
            { 
                return findItem;
            }
            return null;
        }

        public BusinessAuditList GetCheckedItem()
        {
            var findItem = BusinessAudits.OrderBy(o => o.DisplayOrder).LastOrDefault(o => o.Status == 1 && _userModel.Roles.FirstOrDefault(r => r.Id == o.RoleId) != null);
            if (findItem != null)
            {
                return findItem;
            }
            return null;
        }
        //是否可以驳回
        private void CheckOnBack()
        {
            var count = BusinessAudits.Count(o => o.Status > 0);
            if (count>0&&count<BusinessAudits.Count)
            {
                var findItem = GetCurrent();
                if (findItem != null)
                {
                    CanBack = true;
                }

            }
            
        }

        private async void LoadAuditMappings()
        {
            if (ItemDefine.Id == 0)
                return;
            var items = await _auditMappingService.GetAllAuditMappings(ItemDefine.Id, BusinessTypeName:"立项登记");
            AuditMappings.Clear();
            AuditMappings.AddRange(items.Items);
         
            
        }

        private async void UpdateStatus(int status)
        {
            ItemDefine.Status = status;
            await _itemDefineService.CreateOrUpdate(ItemDefine);
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

        private void OnSelectedItemCategory()
        {
            var view = _unityContainer.Resolve<SelectItemCategoryView>();
            var notification = new Notification()
            {
                Title="项目分类",
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
        public FilesManage SelectedFile { get => _selectedFile; set => SetProperty(ref _selectedFile,value); }

       
    }
    
}
