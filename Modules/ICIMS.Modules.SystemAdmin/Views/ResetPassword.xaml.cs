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
    /// ResetPassword.xaml 的交互逻辑
    /// </summary>
    public partial class ResetPassword : UserControl
    {
        public ResetPasswordViewModel ViewModel { get; set; }
        public ResetPassword(ResetPasswordViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.ViewModel = viewModel;
        }

        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Pwd1 = pd1.Password;
            ViewModel.Pwd2 = pd2.Password;
            ViewModel.ResetPasswordCommand.Execute(null);
        }
    }
}
