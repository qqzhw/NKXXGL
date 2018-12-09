using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
    public class ResetPasswordViewModel: BindableBase
    {
        private string _pwd1;
        private string _pwd2;

        public string Pwd1 { get => _pwd1; set => SetProperty(ref _pwd1,value); }
        public string Pwd2 { get => _pwd2; set => SetProperty(ref _pwd2,value); }
        public ICommand ResetPasswordCommand { get; private set; }

        public bool IsOkClick { get; set; }

        public ResetPasswordViewModel()
        {
            this.ResetPasswordCommand = new DelegateCommand<object>(OnResetPasswordViewModel);
        }

        private void OnResetPasswordViewModel(object obj)
        {
            if(string.IsNullOrEmpty(Pwd1) || string.IsNullOrEmpty(Pwd2))
            {
                MessageBox.Show("输入密码不能为空！");
                return;
            }
            if(this.Pwd1 != this.Pwd2)
            {
                MessageBox.Show("两次输入密码不一致！");
                return;
            }
            this.IsOkClick = true;
            TriggerClose?.Invoke();
        }

        public Action TriggerClose { get; set; }

    }
}
