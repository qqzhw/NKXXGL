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
using Telerik.Windows.Imaging.Tools;
using Telerik.Windows.Media.Imaging.Tools;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// CustomSettingsContainer.xaml 的交互逻辑
    /// </summary>
    public partial class CustomSettingsContainer : UserControl, IToolSettingsContainer
    {
        public CustomSettingsContainer()
        {
            InitializeComponent();
        }
        private Action applyCallback;
        private Action cancelCallback;

         
        public void Show(ITool tool, Action applyCallback, Action cancelCallback)
        {
            this.applyCallback = applyCallback;
            this.cancelCallback = cancelCallback;
            this.Visibility = Visibility.Visible;
        }
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (this.applyCallback != null)
            {
                this.CallApplyCallback();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.cancelCallback != null)
            {
                this.CallCancelCallback();
            }
        }


        private void CallApplyCallback()
        {
            this.applyCallback();
        }

        private void CallCancelCallback()
        {
            this.cancelCallback();
        }
    }
}
