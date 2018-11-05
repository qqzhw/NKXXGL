using ICIMS.Client.ViewModels;
using Prism.Events;
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
using System.Windows.Shapes;


namespace ICIMS.Client.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    { 
        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Close += OnCloseWindow;
            this.DataContext = viewModel;
        }

        private void OnCloseWindow()
        {
            CloseWindow();
        }

        public void CloseWindow() {
            this.DialogResult = true;
        }

     
        private void LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();//实现整个窗口的拖动
        }

        public void ShowWindow()
        {
            this.ShowDialog();
        }

        private void tClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;

        }
    }
}
