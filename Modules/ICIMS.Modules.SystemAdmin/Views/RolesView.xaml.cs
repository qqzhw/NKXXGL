using ICIMS.Modules.SystemAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ICIMS.Modules.SystemAdmin.Views
{
    /// <summary>
    /// RolesView.xaml 的交互逻辑
    /// </summary>
    public partial class RolesView : UserControl
    {
        [Unity.Attributes.Dependency]
        public RolesViewModel ViewModel { get; set; }
        public RolesView()
        {
            InitializeComponent();
        }

        [InjectionMethod]
        public void Init()
        {
            this.DataContext = ViewModel;
            ViewModel.View = this;
        }

       
    }
}
