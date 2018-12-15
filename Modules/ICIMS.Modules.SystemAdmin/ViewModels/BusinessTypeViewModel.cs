using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
using ICIMS.Modules.SystemAdmin.Views;
using ICIMS.Service.BusinessManages;
using ICIMS.Service.User;
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
using System.Windows.Controls;
using System.Windows.Input;
using Unity;
using Unity.Attributes;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
    public class BusinessTypeViewModel : BindableBase, INavigationAware
    {
        private readonly IBusinessTypeService _businessTypeService;
        private readonly IBusinessAuditService _businessAuditService;
        private readonly IRoleService _roleService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private IUnityContainer _unityContainer;

        private BusinessType _selectedItem;
        private ObservableCollection<BusinessType> _items;
        private List<BusinessAudit> _businessAudits;
        private ObservableCollection<RoleModel> _roles;
        private BusinessAudit _selectedAudit;
        private string _name;

        public BusinessTypeViewModel(IEventAggregator eventAggregator,
             IRegionManager regionManager,
            IUnityContainer unityContainer,
            IBusinessTypeService businessTypeService,
            IBusinessAuditService businessAuditService,
            IRoleService roleService
            )
        {
            this._regionManager = regionManager;
            this._eventAggregator = eventAggregator;
            this._unityContainer = unityContainer;
            this._businessTypeService = businessTypeService;
            this._businessAuditService = businessAuditService;
            this._roleService = roleService;
            eventAggregator.GetEvent<TabCloseEvent>().Subscribe(OnTabActive);

            AddCommand = new DelegateCommand<object>(OnAddCommand);
            DeleteCommand = new DelegateCommand<object>(OnDeleteCommand);
            MoveCommand = new DelegateCommand<object>(OnMoveCommand);
        }

        private void OnTabActive(UserControl view)
        {
            var region = _regionManager.Regions["MainRegion"];
            if (region.Views.Count() > 1)
            {
                if (view != null)
                {
                    if (region.Views.Contains(view))
                        region.Remove(view);
                }
            }
        }

        private async void OnMoveCommand(object obj)
        {
            try
            {
                var isUp = ((string)obj) == "1";
                var idx = this.SelectedItem.Audits.IndexOf(this.SelectedAudit);
               
                if (isUp)
                {
                    if (idx > 0)
                    {
                        var a = SelectedItem.Audits[idx - 1];
                        var b = SelectedItem.Audits[idx];
                        var temp = a.DisplayOrder;
                        a.DisplayOrder = b.DisplayOrder;
                        b.DisplayOrder = temp;
                        var newA = await _businessAuditService.CreateOrUpdate(a);
                        var newB = await _businessAuditService.CreateOrUpdate(b);
                        a.DisplayOrder = newA.DisplayOrder;
                        b.DisplayOrder = newB.DisplayOrder;
                        this.SelectedItem.Audits = new ObservableCollection<BusinessAudit>(this.SelectedItem.Audits.OrderBy(item => item.DisplayOrder));
                        int i = 1;
                        foreach (var item in this.SelectedItem.Audits)
                        {
                            item.No = i++;
                        }
                    }
                }
                else
                {
                    if (idx < this.SelectedItem.Audits.Count - 1)
                    {
                        var a = SelectedItem.Audits[idx + 1];
                        var b = SelectedItem.Audits[idx];
                        var temp = a.DisplayOrder;
                        a.DisplayOrder = b.DisplayOrder;
                        b.DisplayOrder = temp;
                        var newA = await _businessAuditService.CreateOrUpdate(a);
                        var newB =  await _businessAuditService.CreateOrUpdate(b);
                        a.DisplayOrder = newA.DisplayOrder;
                        b.DisplayOrder = newB.DisplayOrder;
                        this.SelectedItem.Audits = new ObservableCollection<BusinessAudit>(this.SelectedItem.Audits.OrderBy(item => item.DisplayOrder));
                        int i = 1;
                        foreach (var item in this.SelectedItem.Audits)
                        {
                            item.No = i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }

        private async void OnDeleteCommand(object obj)
        {
            if(MessageBox.Show("请确认是否删除","提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _businessAuditService.Delete(this.SelectedAudit.Id);
                    this.SelectedItem.Audits.Remove(this.SelectedAudit);
                    this.BusinessAudits.Remove(this.SelectedAudit);
                    this.SelectedAudit = this.SelectedItem.Audits.FirstOrDefault();
                    MessageBox.Show("删除成功！");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
             
            }
        }

       

        private void OnAddCommand(object obj)
        {
            var newItem = new BusinessTypeEditViewModel(this.Roles);
            BusinessTypeEditView view = new BusinessTypeEditView(newItem);
            var notification = new Notification()
            {
                Title = "角色",
                Content = view,// (new ParameterOverride("name", "")),
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (newItem.IsOkClicked)
                {
                    try
                    {
                        var selectedRole = newItem.SelectedItem;
                        if(!this.SelectedItem.Audits.Any(a=>a.RoleId == selectedRole.Id))
                        {
                            var order = this.SelectedItem.Audits.Count == 0 ? 1 : this.SelectedItem.Audits.Max(a => a.DisplayOrder) + 1;
                            BusinessAudit one = new BusinessAudit
                            {
                                RoleId = selectedRole.Id,
                                RoleName =selectedRole.DisplayName,
                                Name = selectedRole.DisplayName,
                                BusinessTypeId =SelectedItem.Id,
                                BusinessTypeName=SelectedItem.Name,
                                DisplayOrder = order
                            };
                            var returnOne= await _businessAuditService.CreateOrUpdate(one);
                            returnOne.No = order;
                            this.BusinessAudits.Add(returnOne);
                            this.SelectedItem.Audits.Add(returnOne);
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

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public UserControl View { get; internal set; }
        public BusinessType SelectedItem
        {
            get => _selectedItem;
            set
            {
                if(value.Audits == null)
                {
                    var audits = this.BusinessAudits.Where(a => a.BusinessTypeId == value.Id).OrderBy(b=>b.DisplayOrder).ToList();
                    var idx = 1;
                    audits.ForEach(a=>a.No = idx++);
                    value.Audits = new ObservableCollection<BusinessAudit>(audits);
                }
                SetProperty(ref _selectedItem, value);
            }
        }
        public ObservableCollection<BusinessType> Items { get => _items; set => SetProperty(ref _items, value); }
        public List<BusinessAudit> BusinessAudits { get => _businessAudits; set => SetProperty(ref _businessAudits, value); }
        public ObservableCollection<RoleModel> Roles { get => _roles; set => SetProperty(ref _roles, value); }
        public BusinessAudit SelectedAudit { get => _selectedAudit; set => SetProperty(ref _selectedAudit,value); }

        [InjectionMethod]
        public async void Init()
        {
            _title = "业务类型";
            var roles = await _roleService.GetAllRole();
            this.Roles = new ObservableCollection<RoleModel>(roles);

            var businessTypes = (await _businessTypeService.GetAllBusinessTypes()).Items;
            int idx = 1;
            businessTypes.ForEach(a => a.No = idx++);
            this.Items = new ObservableCollection<BusinessType>(businessTypes?.OrderBy(a=>a.DisplayOrder));

            try
            {
               
                var auditTypes = await _businessAuditService.GetAllBusinessAudits();
                this.BusinessAudits = auditTypes.Items;

            }
            catch (Exception ex)
            {
                this.BusinessAudits = new List<BusinessAudit>();
            }

            this.SelectedItem = this.Items.FirstOrDefault();


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

        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
            {
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
