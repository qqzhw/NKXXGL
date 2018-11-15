using ICIMS.Model.BaseData;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ICIMS.Modules.BaseData.ViewModels
{
    public class FundEditViewModel : BindableBase
    {
        private bool _showReAddBtn;

        public FundItem Item { get; set; }

        public ICommand OkCmd { get; private set; }
        public ICommand CancelCmd { get; private set; }
        public ICommand ReAddCmd { get; private set; }

        public FundEditViewModel()
        {
            this.OkCmd = new DelegateCommand<object>(OnOkCmd);
            this.CancelCmd = new DelegateCommand<object>(OnCancelCmd);
            this.ReAddCmd = new DelegateCommand<object>(OnReAddCmd);
        }

        private void OnReAddCmd(object obj)
        {
            this.IsOkClicked = null;
        }

        public bool? IsOkClicked { get; private set; }

        private void OnCancelCmd(object obj)
        {
            this.IsOkClicked = false;
            this.Close?.Invoke(null);
        }

        private void OnOkCmd(object obj)
        {
            this.IsOkClicked = true;
            this.Close?.Invoke(null);
        }

        public bool ShowReAddBtn
        {
            get => _showReAddBtn; set { this.SetProperty(ref _showReAddBtn, value); this.RaisePropertyChanged(nameof(ReAddVisibility)); }
        }

        public Visibility ReAddVisibility
        {
            get
            {
                return ShowReAddBtn ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public virtual UserControl View { get; set; }



        public Action<bool?> Close { get; internal set; }
    }
}
