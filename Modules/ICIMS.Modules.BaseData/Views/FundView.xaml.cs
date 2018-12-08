using ICIMS.Core.Interactivity.InteractionRequest;
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
using Unity.Attributes;

namespace ICIMS.Modules.BaseData.Views
{
    /// <summary>
    /// FundView.xaml 的交互逻辑
    /// </summary>
    public partial class FundView : UserControl
    {
        public FundView()
        {
            InitializeComponent();  
        }
        [Dependency]
        public FundViewModel ViewModel { get; set; }
        [InjectionMethod]
        public void Init()
        {
            this.DataContext = ViewModel;
            ViewModel.View = this;
        }
        public void BindAction(Action<bool?> action)
        {
            ViewModel.Close = action;
        }
       
       

        private void RadDataPager_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            var newIndex = e.NewPageIndex;
        }

        private void RadTreeListView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.EditCommand.Execute(null);
        }
    }
}
