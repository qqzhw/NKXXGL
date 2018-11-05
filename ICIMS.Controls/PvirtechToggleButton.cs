using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.Framework.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.Framework.Controls;assembly=Pvirtech.Framework.Controls"
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
    ///     <MyNamespace:PvirtechToggleButton/>
    ///
    /// </summary> 
    public class PvirtechToggleButton : ToggleButton
    {
        private DoubleAnimation Animate;

        private Storyboard AnimatrStoryBoard;

        public static readonly DependencyProperty IsAnimateProperty;

        public bool IsAnimate
        {
            get
            {
                return (bool)base.GetValue(PvirtechToggleButton.IsAnimateProperty);
            }
            set
            {
                base.SetValue(PvirtechToggleButton.IsAnimateProperty, value);
            }
        }

        static PvirtechToggleButton()
        {
            PvirtechToggleButton.IsAnimateProperty = DependencyProperty.Register("IsAnimate", typeof(bool), typeof(PvirtechToggleButton), new PropertyMetadata(false, new PropertyChangedCallback(OnIsAnimatePropertyChangedCallBack)));
           
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PvirtechToggleButton), new FrameworkPropertyMetadata(typeof(PvirtechToggleButton)));
        }

        private static void OnIsAnimatePropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PvirtechToggleButton)d).OnIsAnimatePropertyChangedCallBack();
        }

        private void OnIsAnimatePropertyChangedCallBack()
        {
            bool flag = this.AnimatrStoryBoard == null;
            if (flag)
            {
                this.AnimatrStoryBoard = new Storyboard();
            }
            bool flag2 = this.AnimatrStoryBoard != null;
            if (flag2)
            {
                bool isAnimate = this.IsAnimate;
                if (isAnimate)
                {
                    this.Animate = new DoubleAnimation(1.0, 0.1, new Duration(TimeSpan.FromSeconds(1.0)));
                    this.Animate.RepeatBehavior = RepeatBehavior.Forever;
                    this.Animate.AutoReverse = true;
                    Storyboard.SetTargetProperty(this.Animate, new PropertyPath(UIElement.OpacityProperty));
                    this.AnimatrStoryBoard.Children.Add(this.Animate);
                    base.BeginStoryboard(this.AnimatrStoryBoard, HandoffBehavior.SnapshotAndReplace, true);
                }
                else
                {
                    this.Animate.RepeatBehavior = new RepeatBehavior(1.0);
                    this.AnimatrStoryBoard.Stop();
                    this.AnimatrStoryBoard.Children.Remove(this.Animate);
                    this.AnimatrStoryBoard.Remove(this);
                }
            }
        }
    }
}
