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
using Telerik.Windows;
using Telerik.Windows.Controls.GridView;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// DocManagerView.xaml 的交互逻辑
    /// </summary>
    public partial class DocManagerView : UserControl
    {
        public DocManagerView(DocManagerViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.View = this;
            this.radInfo.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(viewModel.OnDoubleClick), true);
        }
    }
}
