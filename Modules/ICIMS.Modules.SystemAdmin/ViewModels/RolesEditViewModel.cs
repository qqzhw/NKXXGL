using ICIMS.Model.User;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
    public class RolesEditViewModel: BindableBase
    {
        public RoleModel Item { get; set; }

        public ICommand SaveCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public bool IsOkClicked { get; private set; }


        public RolesEditViewModel()
        {
            this.SaveCommand = new DelegateCommand<object>(OnSaveCommand);
            this.ResetCommand = new DelegateCommand<object>(OnResetCommand);
        }

        private void OnResetCommand(object obj)
        {
            if (this.Item != null)
            {
                this.Item.DisplayName = "";
            }
        }

        private void OnSaveCommand(object obj)
        {
            this.IsOkClicked = true;
            this.Close?.Invoke(null);
        }

        public virtual UserControl View { get; set; }



        public Action<bool?> Close { get; internal set; }
    }
}
