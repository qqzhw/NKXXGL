using ICIMS.Model.BusinessManages;
using ICIMS.Modules.BusinessManages.ViewModels;
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
using Telerik.Windows.Controls.GridView;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// PayAuditEditView.xaml 的交互逻辑
    /// </summary>
    public partial class PayAuditEditView : UserControl
    {
        public PayAuditEditView(PayAuditEditViewModel viewModel, PayAuditList data)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        } 
         
        private void CellDoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GridViewCellBase cell = e.OriginalSource as GridViewCellBase;
            if (cell != null)
            {
               // this.ClickedCell = cell;
            }
        }
    }
}
