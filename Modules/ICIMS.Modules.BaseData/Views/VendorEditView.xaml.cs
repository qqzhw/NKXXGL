using ICIMS.Modules.BaseData.ViewModels;
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

namespace ICIMS.Modules.BaseData.Views
{
    /// <summary>
    /// VendorEditView.xaml 的交互逻辑
    /// </summary>
    public partial class VendorEditView : UserControl
    {
        private readonly VendorEditViewModel ViewModel;

        public VendorEditView(VendorEditViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.View = this;
            this.ViewModel = viewModel;

        }

        public void BindAction(Action<bool?> action)
        {
            ViewModel.Close = action;
        }
    }
}
