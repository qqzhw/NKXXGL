using ICIMS.Core.Common;
using ICIMS.Metro.Controls;
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
using Telerik.Windows.Controls;

namespace ICIMS.Client.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow 
    {
        private RadDesktopAlertManager selectedDesktopAlertManager;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            selectedDesktopAlertManager = new RadDesktopAlertManager();
            //MainTabControl.Items.Add(new MetroTabItem() { Name = "ddddd" });

            // this.selectedDesktopAlertManager.CloseAlert(tt);
            //  tt.Content = new TextBlock() { Text = "asdasdasd" };
            this.selectedDesktopAlertManager.CloseAlert(tt);
            this.selectedDesktopAlertManager.ShowAlert(new DesktopAlertParameters
                {
                    Header = "测试",
                    Content = new TextBlock(){Text="asdasdasd" },
                    ShowDuration =1,
                    CanMove =true,
                    CanAutoClose =true,
                    //Icon = new Image { Source = Application.Current.FindResource("DesktopAlertIcon") as ImageSource, Width = 48, Height = 48 },
                    IconColumnWidth = 48,
                    IconMargin = new Thickness(10, 0, 20, 0),
                    ShowMenuButton = true,
                    ShowCloseButton = this.ShowCloseButton,
                    //MenuItemsSource = this.GetMenuItems(),
                    Closed = (s, a) =>
                    {
                       // (this.CloseAllAlertsCommand as DelegateCommand).InvalidateCanExecute();
                    }
                });
           // this.selectedDesktopAlertManager.ShowAlert(tt);
            //  (this.CloseAllAlertsCommand as DelegateCommand).InvalidateCanExecute();

        }
         
    }
}
