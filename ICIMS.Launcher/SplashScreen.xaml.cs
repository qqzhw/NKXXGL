using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICIMS.Launcher
{
    /// <summary>
    /// SplashScreen.xaml 的交互逻辑
    /// </summary>
    public partial class SplashScreen : Window,IDisposable
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            WindowInteropHelper wih = new WindowInteropHelper(this);  
            SendMessage(wih.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            return;
        }

        private void ClosedMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();            
        }


        public void Dispose()
        {
           
        }
    }
}
