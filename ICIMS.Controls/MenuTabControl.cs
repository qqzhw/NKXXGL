 
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

namespace ICIMS.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.CaseCommander.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.CaseCommander.Controls;assembly=Pvirtech.CaseCommander.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:HeaderTabControl/>
    ///
    /// </summary>
    public class MenuTabControl : Control
    {
        private static Path MainPath;
        private static TextBlock TxtHeader;
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(MenuTabControl), new UIPropertyMetadata(false, 
                 new PropertyChangedCallback(OnValueChanged))); 
          
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(MenuTabControl), new UIPropertyMetadata(null));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.Register("PressedColor", typeof(Brush), typeof(MenuTabControl), new UIPropertyMetadata(null));


        public Brush PressedColor
        {
            get { return (Brush)GetValue(PressedColorProperty); }
            set { SetValue(PressedColorProperty, value); }
        }

        public  event Action<object , EventArgs> MouseClosed;
      

        static MenuTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuTabControl), new FrameworkPropertyMetadata(typeof(MenuTabControl)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TxtHeader = GetTemplateChild("txtHeader") as TextBlock; 

            var  _closeBtn = GetTemplateChild("btnX") as TextBlock;
             MainPath = GetTemplateChild("path") as Path;
           
            if (_closeBtn != null)
            {
                _closeBtn.MouseLeftButtonDown += OnClickHandler;
              
            } 
            this.MouseLeftButtonDown += OnPathMouseLeftButtonDown;
        }
        
        void OnPathMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
             this.IsPressed = true;
             this.PressedColor = new SolidColorBrush(Color.FromRgb(100, 155, 200));
             this.Foreground = new SolidColorBrush(Colors.White);   
        }
        private void OnClickHandler(object sender, EventArgs e)
        { 
            if (this.MouseClosed != null)
            {
                this.MouseClosed(this, e);
            }  
        }

        public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
        {
            if (eventToRaise != null)
            {
                eventToRaise(sender, args);
            }
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuTabControl)
            {
                MenuTabControl menuTab = d as MenuTabControl;
                
                //if (menuTab.IsPressed)
                //{ 
                //     MainPath.Fill = new SolidColorBrush(Color.FromRgb(100,155,200));
                //    TxtHeader.Foreground = new SolidColorBrush(Colors.White);
                //}
                //else
                //{
                //    MainPath.Fill = new SolidColorBrush(Color.FromRgb(125, 176, 248));
                //    TxtHeader.Foreground = new SolidColorBrush(Colors.Black);
                //}
               
            }
        }
    }
}
