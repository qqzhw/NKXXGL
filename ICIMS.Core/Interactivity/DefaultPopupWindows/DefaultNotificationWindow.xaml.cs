

using ICIMS.Core.Interactivity.InteractionRequest;
using System.Windows;

namespace ICIMS.Core.Interactivity.DefaultPopupWindows
{
    /// <summary>
    /// Interaction logic for NotificationChildWindow.xaml
    /// </summary>
    public partial class DefaultNotificationWindow
    {
        /// <summary>
        /// Creates a new instance of <see cref="DefaultNotificationWindow"/>
        /// </summary>
        public DefaultNotificationWindow()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Sets or gets the <see cref="INotification"/> shown by this window./>
        /// </summary>
        public INotification Notification 
        {
            get
            {
                return this.DataContext as INotification;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
