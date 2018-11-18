using ICIMS.Modules.BusinessManages.ViewModels;
using System;
using System.Collections.Generic; 
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
using Unity.Attributes;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// SelectItemCategoryView.xaml 的交互逻辑
    /// </summary>
    public partial class SelectItemCategoryView : UserControl
    {
        public SelectItemCategoryView()
        {
            InitializeComponent(); 
        }  
        [Dependency]
        public SelectItemCategoryViewModel ViewModel { get; set; }
        [InjectionMethod]
        public void Init()
        {
            this.DataContext = ViewModel;
            this.RadTreeListView1.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(ViewModel.OnDoubleClick), true);
        }
        public void BindAction(Action<bool?> action)
        {
            ViewModel.Close = action;
        }
    }
}
