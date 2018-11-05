using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ICIMS.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.PoliceInfoSystem.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.PoliceInfoSystem.Controls;assembly=Pvirtech.PoliceInfoSystem.Controls"
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
    ///     <MyNamespace:MenuNavControl/>
    ///
    /// </summary>
    public class MenuNavControl : ToggleButton
    {
        private const string SwitchPart = "NumberBorder";
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MenuNavControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty SecTextProperty = DependencyProperty.Register("SecText", typeof(string), typeof(MenuNavControl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MenuNavControl), new PropertyMetadata(default(ImageSource)));
        //CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MenuNavControl), new PropertyMetadata(default(CornerRadius)));
        public static readonly DependencyProperty ImgWidthProperty = DependencyProperty.Register("ImgWidth", typeof(int), typeof(LeftNavControl), new PropertyMetadata(16));

        public static readonly DependencyProperty BorderMargionProperty = DependencyProperty.Register(
            "BorderMargion",
            typeof(Thickness),
            typeof(MenuNavControl),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0))
            );
        public Thickness BorderMargion
        {
            get { return (Thickness)GetValue(BorderMargionProperty); }
            set { SetValue(BorderMargionProperty, value); }
        }
        public event EventHandler<RoutedEventArgs> Click;
        static MenuNavControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuNavControl), new FrameworkPropertyMetadata(typeof(MenuNavControl)));
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var _toggleButton = GetTemplateChild(SwitchPart) as Border;
            if (_toggleButton != null)
            {
                _toggleButton.MouseLeftButtonDown += ClickHandler;
                _toggleButton.MouseLeftButtonUp += _toggleButton_MouseLeftButtonUp;
                _toggleButton.MouseLeave += _toggleButton_MouseLeave;
                _toggleButton.MouseEnter += _toggleButton_MouseEnter;
            }
        }

        void _toggleButton_MouseEnter(object sender, MouseEventArgs e)
        {
            //this.Background = new SolidColorBrush(Color.FromRgb(148, 218, 255)); 
        }

        void _toggleButton_MouseLeave(object sender, MouseEventArgs e)
        {
            // this.Background = new SolidColorBrush(Colors.Transparent); 
            this.Opacity = 1.0;
        }

        void _toggleButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 1.0;
        }

        public int ImgWidth
        {
            get { return (int)GetValue(ImgWidthProperty); }
            set { SetValue(ImgWidthProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string SecText
        {
            get { return (string)GetValue(SecTextProperty); }
            set { SetValue(SecTextProperty, value); }
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
            //LinearGradientBrush brush = new LinearGradientBrush();
            //brush.EndPoint = new Point(0.5, 1);
            //brush.MappingMode = BrushMappingMode.RelativeToBoundingBox;
            //brush.StartPoint = new Point(0.5, 0);
            //GradientStop fgs = new GradientStop(Color.FromRgb(207, 235, 249), 0);
            //GradientStop sgs = new GradientStop(Color.FromRgb(148, 218, 255), 1);
            //GradientStop tgs = new GradientStop(Color.FromRgb(161, 223, 255), 0.378);
            //GradientStop egs = new GradientStop(Color.FromRgb(155, 218, 249), 0.419);
            //GradientStopCollection collection = new GradientStopCollection();
            //collection.Add(fgs);
            //collection.Add(sgs);
            //collection.Add(tgs);
            //collection.Add(egs);
            //brush.GradientStops = collection;
            //this.Background = brush; 
            //this.Foreground =new SolidColorBrush(Colors.White);
            this.Opacity = 0.8;
            Raise(Click, this, e);
        }

        public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
        {
            if (eventToRaise != null)
            {
                eventToRaise(sender, args);
            }
        }
    }
}
