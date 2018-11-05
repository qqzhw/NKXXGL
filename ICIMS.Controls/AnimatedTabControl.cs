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
using System.Windows.Threading;

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
    ///     <MyNamespace:AnimatedTabControl/>
    ///
    /// </summary>
 
    public class AnimatedTabControl : TabControl
    {
        public static readonly RoutedEvent SelectionChangingEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanging", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AnimatedTabControl));

        private DispatcherTimer timer;
        static AnimatedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedTabControl), new FrameworkPropertyMetadata(typeof(AnimatedTabControl)));
        }
        public AnimatedTabControl()
        {
            DefaultStyleKey = typeof(AnimatedTabControl);
        }

        public event RoutedEventHandler SelectionChanging
        {
            add { AddHandler(SelectionChangingEvent, value); }
            remove { RemoveHandler(SelectionChangingEvent, value); }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                (Action)delegate
                {
                    this.RaiseSelectionChangingEvent();

                    this.StopTimer();

                    this.timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = new TimeSpan(0, 0, 0, 0, 500) };

                    EventHandler handler = null;
                    handler = (sender, args) =>
                    {
                        this.StopTimer();
                        base.OnSelectionChanged(e);
                    };
                    this.timer.Tick += handler;
                    this.timer.Start();
                });
        }

        // This method raises the Tap event
        private void RaiseSelectionChangingEvent()
        {
            var args = new RoutedEventArgs(SelectionChangingEvent);
            RaiseEvent(args);
        }

        private void StopTimer()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer = null;
            }
        }
    }
}
