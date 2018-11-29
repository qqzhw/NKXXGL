using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ICIMS.Model.BaseData;
using ICIMS.Model.User;
using Prism.Commands;
using Prism.Mvvm;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
    public class UsersEditViewModel : BindableBase
    {
        public UserModel Item { get; set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public bool IsOkClicked { get; private set; }
        private ObservableCollection<RoleModel> _roles;
        private ObservableCollection<OrganizationUnitItem> _departments;
        private RoleModel _selectedRole;
        private OrganizationUnitItem _selectedDepartment;

        public UsersEditViewModel(ObservableCollection<OrganizationUnitItem> departments, ObservableCollection<RoleModel> roles)
        {
            this.SaveCommand = new DelegateCommand<object>(OnSaveCommand);
            this.ResetCommand = new DelegateCommand<object>(OnResetCommand);
            this.Departments = departments;
            this.Roles = roles;
        }

        private void OnResetCommand(object obj)
        {
            if (this.Item != null)
            {
                this.Item.Name = "";
                this.Item.UserName = "";
                this.Item.UnitId = null;
                this.Item.UnitName = "";
                this.Item.RoleNames = new List<string>();
            }
        }

        private void OnSaveCommand(object obj)
        {
            this.IsOkClicked = true;
            this.Close?.Invoke(null);
        }

        public virtual UserControl View { get; set; }



        public Action<bool?> Close { get; internal set; }
        public RoleModel SelectedRole
        {
            get => _selectedRole;
            set
            {
                this.Item.RoleNames = new List<string> { value.DisplayName };
                this.Item.RoleIds = new List<int> { value.Id};
                SetProperty(ref _selectedRole, value);
            }
        }
        public OrganizationUnitItem SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                this.Item.UnitId = value.Id;
                this.Item.UnitName = value.DisplayName;
                SetProperty(ref _selectedDepartment, value);
            }
        }

        public ObservableCollection<RoleModel> Roles { get => _roles; set => SetProperty(ref _roles,value); }
        public ObservableCollection<OrganizationUnitItem> Departments { get => _departments; set =>SetProperty(ref _departments,value); }
    }
}
