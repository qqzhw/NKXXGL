using ICIMS.Modules.SystemAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICIMS.Modules.SystemAdmin.Views
{
    /// <summary>
    /// RolesEditView.xaml 的交互逻辑
    /// </summary>
    public partial class RolesEditView : UserControl
    {
        public RolesEditView(RolesEditViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.View = this;
            this.ViewModel = viewModel;
        }

        public RolesEditViewModel ViewModel { get; }

        public void BindAction(Action<bool?> action)
        {
            ViewModel.Close = action;
        }
    }
}
