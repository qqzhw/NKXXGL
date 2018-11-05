using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    ///     <MyNamespace:LeftNavControl/>
    ///
    /// </summary>
    public class LeftNavControl : Control
    {
        private const string SwitchPart = "NavBorder";
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LeftNavControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(LeftNavControl), new PropertyMetadata(default(ImageSource)));
        //CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(LeftNavControl), new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(LeftNavControl), new PropertyMetadata(new PropertyChangedCallback(OnIsPressedChanged)));

        public event EventHandler<RoutedEventArgs> Click;
        static LeftNavControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LeftNavControl), new FrameworkPropertyMetadata(typeof(LeftNavControl)));
        }
         public override void OnApplyTemplate()
        {
                base.OnApplyTemplate();
                var _toggleButton = GetTemplateChild(SwitchPart) as Border;
                if (_toggleButton!=null)
                {
                    _toggleButton.MouseLeftButtonDown+= ClickHandler;
                    _toggleButton.MouseLeftButtonUp += _toggleButton_MouseLeftButtonUp;
                    _toggleButton.MouseLeave += _toggleButton_MouseLeave;
                    _toggleButton.MouseEnter += _toggleButton_MouseEnter;
                    _toggleButton.MouseMove += _toggleButton_MouseMove;
                }
        }



         void _toggleButton_MouseMove(object sender, MouseEventArgs e)
         { 
            
         }

         void _toggleButton_MouseEnter(object sender, MouseEventArgs e)
         {
           // this.Background = new SolidColorBrush(Color.FromArgb(255, 77, 139, 187));
         }

        void _toggleButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 1.0;
           //  this.Background = new SolidColorBrush(Colors.Transparent);
        }

        void _toggleButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 1.0;
        }
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Color.FromArgb(255, 51, 133, 255));
            this.Opacity = 0.8;
             Raise(Click, this, e);
             IsPressed = true;
        }

        public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
        {
            if (eventToRaise != null)
            {
                eventToRaise(sender, args);
            }
        }
        private static   void OnIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = (d as LeftNavControl);
            if (item.IsPressed)
            {
                item.Background = new SolidColorBrush(Color.FromArgb(255, 51, 133, 255));
            }
            else
            {
                item.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }    
}
