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
    public class SubjectEditViewModel : BindableBase
    {
        private bool _showReAddBtn;

        private SubjectItem _item;
        public SubjectItem Item { get => _item; set => SetProperty(ref _item, value); }

        public ICommand OkCmd { get; private set; }
        public ICommand CancelCmd { get; private set; }
        public ICommand ReAddCmd { get; private set; }

        public SubjectEditViewModel()
        {
            this.OkCmd = new DelegateCommand<object>(OnOkCmd);
            this.CancelCmd = new DelegateCommand<object>(OnCancelCmd);
            this.ReAddCmd = new DelegateCommand<object>(OnReAddCmd);
        }

        private void OnReAddCmd(object obj)
        {
            this.IsOkClicked = 2;
        }

        public int IsOkClicked { get; private set; }

        private void OnCancelCmd(object obj)
        {
            this.IsOkClicked =0;
            this.TriggerClose?.Invoke();
        }

        private void OnOkCmd(object obj)
        {
            this.IsOkClicked = 1;
            this.TriggerClose?.Invoke();
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
        public Action TriggerClose { get; internal set; }
    }
}
