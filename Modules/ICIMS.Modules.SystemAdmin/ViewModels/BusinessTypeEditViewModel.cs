using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ICIMS.Model.User;
using Prism.Mvvm;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
    public class BusinessTypeEditViewModel: BindableBase
    {
        private ObservableCollection<RoleModel> _roles;

        public ICommand SaveCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public bool IsOkClicked { get; set; }
        private RoleModel _selectedItem;

        public BusinessTypeEditViewModel(ObservableCollection<RoleModel> roles)
        {
            this.Roles = roles;
        }


      

        private void OnSaveCommand(object obj)
        {
            this.IsOkClicked = true;
            this.Close?.Invoke(null);
        }

        public virtual UserControl View { get; set; }



        public Action<bool?> Close { get; internal set; }

        public ObservableCollection<RoleModel> Roles { get => _roles; set => SetProperty(ref _roles,value); }
        public RoleModel SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem,value); }
    }
}
