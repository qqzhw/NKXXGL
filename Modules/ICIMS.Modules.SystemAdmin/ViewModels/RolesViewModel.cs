using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.User;
using ICIMS.Modules.SystemAdmin.Views;
using ICIMS.Service;
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

    public class RolesViewModel : BindableBase, INavigationAware
    {
        private readonly IRoleService _service;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private IUnityContainer _unityContainer;
        public RolesViewModel(IRoleService service,
            IEventAggregator eventAggregator,
            IRegionManager regionManager,
            IUnityContainer unityContainer)
        {
            this._service = service;
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            this._regionManager = regionManager;
            eventAggregator.GetEvent<TabCloseEvent>().Subscribe(OnTabActive);
            AddCommand = new DelegateCommand<object>(OnAddCommand);
            EditCommand = new DelegateCommand<object>(OnEditCommand);
            DeleteCommand = new DelegateCommand<object>(OnDeleteCommand);
            CheckAllCommand = new DelegateCommand<object>(OnCheckAllCommand);
            UnCheckAllCommand = new DelegateCommand<object>(OnUnCheckAllCommand);
            SaveCommand = new DelegateCommand<object>(OnSaveCommand);
        }

        private async void OnSaveCommand(object obj)
        {
            try
            {
                var permissions = this.AllPermissions.Where(a => a.IsChecked).Select(b => b.Name);
                var selectedItem = CommonHelper.CopyItem(this.SelectedItem);
                selectedItem.Permissions = (ObservableCollection<string>)permissions;
                var rsData = await _service.Update(selectedItem);
                CommonHelper.SetValue(this.SelectedItem, rsData);
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void OnUnCheckAllCommand(object obj)
        {
            if (this.AllPermissions != null)
            {
                foreach (var item in AllPermissions)
                {
                    item.IsChecked = false;
                }
            }
        }

        private void OnCheckAllCommand(object obj)
        {
            if (this.AllPermissions != null)
            {
                foreach (var item in AllPermissions)
                {
                    item.IsChecked = true;
                }
            }
        }

        private void OnEditCommand(object obj)
        {
            var newItem = new RolesEditViewModel();
            var selectedItem = CommonHelper.CopyItem(this.SelectedItem);
            newItem.Item = selectedItem;
            RolesEditView view = new RolesEditView(newItem);
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
                        var data = await _service.Update(newItem.Item);
                        if (data != null)
                        {
                            CommonHelper.SetValue(this.SelectedItem, data);
                            MessageBox.Show("保存成功！");
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

        private async void OnDeleteCommand(object obj)
        {
            try
            {
                if (this.SelectedItem != null)
                {
                    await _service.Delete(this.SelectedItem.Id);
                    this.Items.Remove(SelectedItem);
                    this.SelectedItem = this.Items.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
          
        }

        private void OnAddCommand(object obj)
        {
            var newItem = new RolesEditViewModel();
            newItem.Item = new RoleModel();
            RolesEditView view = new RolesEditView(newItem);
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
                        newItem.Item.Name = newItem.Item.DisplayName;
                        var data = await _service.Create(newItem.Item);
                        if (data != null)
                        {
                            data.No = this.Items.Max(a => a.No) + 1;
                            this.Items.Add(data);
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

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CheckAllCommand { get; private set; }
        public ICommand UnCheckAllCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

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

        private ObservableCollection<RoleModel> _items;

        public ObservableCollection<RoleModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private ObservableCollection<PermissionModel> _allPermissions;

        public ObservableCollection<PermissionModel> AllPermissions
        {
            get => _allPermissions;
            set => SetProperty(ref _allPermissions, value);
        }

        private RoleModel _selectedItem;
        public RoleModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                UpdatePermissionState(value);
                SetProperty(ref _selectedItem, value);
            }
        }

        private void UpdatePermissionState(RoleModel value)
        {
            if (value != null && this.AllPermissions != null)
            {
                foreach (var item in AllPermissions)
                {
                    item.IsChecked = value.Permissions.Contains(item.Name);
                }
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public UserControl View { get; internal set; }

        [InjectionMethod]
        public async void Init()
        {
            _title = "角色权限";
            var rs = await _service.GetAllRole();
            var allPermissions = await _service.GetAllPermissions();

            this.Items = new ObservableCollection<RoleModel>(rs);
            this.AllPermissions = new ObservableCollection<PermissionModel>(allPermissions);

            this.SelectedItem = this.Items.FirstOrDefault();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
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
