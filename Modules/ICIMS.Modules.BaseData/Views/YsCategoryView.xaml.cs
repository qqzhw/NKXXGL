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
    /// YsCategoryView.xaml 的交互逻辑
    /// </summary>
    public partial class YsCategoryView : UserControl
    {
        public YsCategoryView()
        {
            InitializeComponent();
        }


        [Unity.Attributes.Dependency]
        public YsCategoryViewModel ViewModel { get; set; }
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
        private void RadTreeListView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.EditCommand.Execute(null);
        }
    }
}
