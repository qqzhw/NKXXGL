using ICIMS.Model.BaseData;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ICIMS.Modules.BaseData.ViewModels
{
    public class FundEditViewModel: BindableBase
    {
        public FundItem Item { get; set; }

        public ICommand OkCmd { get; private set; }
        public ICommand CancelCmd { get; private set; }

        public FundEditViewModel()
        {
            this.OkCmd = new DelegateCommand<object>(OnOkCmd);
            this.CancelCmd = new DelegateCommand<object>(OnCancelCmd);
        }

        public bool IsOkClicked { get; private set; }

        private void OnCancelCmd(object obj)
        {
            this.IsOkClicked = false;
        }

        private void OnOkCmd(object obj)
        {
            this.IsOkClicked = true;
        }
    }
}
