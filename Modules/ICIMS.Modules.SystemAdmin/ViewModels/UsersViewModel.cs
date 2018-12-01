using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BaseData;
using ICIMS.Model.User;
using ICIMS.Modules.SystemAdmin.Views;
using ICIMS.Service;
using ICIMS.Service.BaseData;
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
    public partial class UsersViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private IUnityContainer _unityContainer;
        private IUserService _service;
        private IOrganizationUnitService _departmentService;
        private IRoleService _roleService;
        private string _title;
        private ObservableCollection<UserModel> _items;
        private UserModel _selectedItem;
        private ObservableCollection<OrganizationUnitItem> _departments;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CheckAllCommand { get; private set; }
        public ICommand UnCheckAllCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public UsersViewModel(IUnityContainer unityContainer,
            IEventAggregator eventAggregator,
            IUserService service,
            IRoleService roleService,
            IOrganizationUnitService departmentService)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _service = service;
            _departmentService = departmentService;
            _roleService = roleService;
            AddCommand = new DelegateCommand<object>(OnAddCommand);
            EditCommand = new DelegateCommand<object>(OnEditCommand);
            DeleteCommand = new DelegateCommand<object>(OnDeleteCommand);
            CheckAllCommand = new DelegateCommand<object>(OnCheckAllCommand);
            UnCheckAllCommand = new DelegateCommand<object>(OnUnCheckAllCommand);
            SaveCommand = new DelegateCommand<object>(OnSaveCommand);
            RefreshCommand = new DelegateCommand<object>(OnRefreshCommand);
            _title = "操作员管理";
        }

        private void OnRefreshCommand(object obj)
        {
            this.Init();
        }

        private async void OnSaveCommand(object obj)
        {
            try
            {
                var departments = this.Departments.Where(a => a.IsChecked).ToList();
                var selectedItem = CommonHelper.CopyItem(this.SelectedItem);
                selectedItem.Units = departments;
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
            foreach (var item in Departments)
            {
                item.IsChecked = false;
            }
        }

        private void OnCheckAllCommand(object obj)
        {
            foreach (var item in Departments)
            {
                item.IsChecked = true;
            }
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

        private void OnEditCommand(object obj)
        {
            var newItem = new UsersEditViewModel(this.Departments, this.Roles);
            var selectedItem = CommonHelper.CopyItem(this.SelectedItem);
            newItem.Item = selectedItem;
            UsersEditView view = new UsersEditView(newItem);
            var notification = new Notification()
            {
                Title = "操作员",
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

        private void OnAddCommand(object obj)
        {
            var newItem = new UsersEditViewModel(this.Departments,this.Roles);
            newItem.Item = new UserModel();
            UsersEditView view = new UsersEditView(newItem);
            var notification = new Notification()
            {
                Title = "操作员",
                Content = view,// (new ParameterOverride("name", "")),
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (newItem.IsOkClicked)
                {
                    try
                    {
                        newItem.Item.Name = newItem.Item.UserName;
                        newItem.Item.Surname = newItem.Item.UserName;
                        newItem.Item.Password = "111111";
                        newItem.Item.UnitIds = new List<int> { newItem.SelectedDepartment.Id};
                        newItem.Item.RoleNames = new List<string> { newItem.SelectedRole.Name};
                       var data = await _service.Create(newItem.Item);
                        if (data != null)
                        {
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

        [InjectionMethod]
        public async void Init()
        {
            ShowCommand = new DelegateCommand<object>(OnMenuCommand);
            var users = await _service.GetAllUsersAsync();
            this.Items = new ObservableCollection<UserModel>(users);
            var departments = (await _departmentService.GetPageItems()).datas;
            foreach (var data in departments)
            {
                if (data.GroupNo != data.Code)
                {
                    data.Parent = departments.FirstOrDefault(a => a.Code == data.GroupNo);
                    data.Parent?.Children.Add(data);
                }
            }
            this.Departments = new ObservableCollection<OrganizationUnitItem>(departments.Where(a => a.Children.Count == 0));
            this.SelectedItem = this.Items.FirstOrDefault();
            var roles = await _roleService.GetAllRole();
            this.Roles = new ObservableCollection<RoleModel>(roles);
        }
        public ICommand ShowCommand { get; private set; }
        private void OnMenuCommand(object obj)
        {

            var notification = new Notification()
            {
                Title = "操作员",
                Content = _unityContainer.Resolve<UsersView>(),// (new ParameterOverride("name", "")),
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) =>
            {

            });
        }


        #region 通用属性

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

        //whether the tab is selected;
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        //To do:define the UI for tabcontrol's content;
        public virtual UserControl View { get; set; }

        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
            {
                if (ConfirmToClose())
                {
                    _eventAggregator.GetEvent<TabCloseEvent>().Publish(View);
                }
            }));

        public ObservableCollection<UserModel> Items { get => _items; set => SetProperty(ref _items, value); }
        public ObservableCollection<OrganizationUnitItem> Departments { get => _departments; set => SetProperty(ref _departments, value); }
        public UserModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetDepartmentStatusByUser(value);
                SetProperty(ref _selectedItem, value);
            }
        }

        public ObservableCollection<RoleModel> Roles { get; private set; }

        private void SetDepartmentStatusByUser(UserModel user)
        {
            foreach (var item in Departments)
            {
                item.IsChecked = user.Units?.Any(a => a.Code == item.Code) ?? false;
            }
        }

        protected virtual bool ConfirmToClose()
        {
            return true;
        }
        #endregion
    }

}
