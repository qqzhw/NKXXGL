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
using Telerik.Windows.Controls;

namespace ICIMS.Modules.BaseData.Views
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : UserControl, IInteractionRequestAware
    {
       
        public UserView(UserViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.View = this;
            this.FinishInteraction += viewModel.Close;    
        }

        public INotification Notification { get ; set; }
        public Action FinishInteraction { get; set; }

        private void movieAutoCompleteBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var autoCompleteBox = sender as RadAutoCompleteBox;
            if (autoCompleteBox.SelectedItem != null)
            {
               // this.movieInfoStackPanel.Visibility = System.Windows.Visibility.Visible;
               // this.viewModel.CurrentDate = DateTime.Now;
            }
            else
            {
               // this.movieInfoStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
