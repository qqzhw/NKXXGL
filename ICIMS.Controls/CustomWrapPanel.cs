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
    ///     xmlns:MyNamespace="clr-namespace:Pvirtech.Commander.Controls"
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
    ///     <MyNamespace:CustomWrapPanel/>
    ///
    /// </summary>
    public class CustomWrapPanel : Panel
    {
        public static readonly DependencyProperty MinItemWidthProperty = DependencyProperty.Register("MinItemWidth", typeof(double), typeof(CustomWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty MaxItemWidthProperty = DependencyProperty.Register("MaxItemWidth", typeof(double), typeof(CustomWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(double), typeof(CustomWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty RowMarginProperty = DependencyProperty.Register("RowMargin", typeof(double), typeof(CustomWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty FloorItemWidthProperty = DependencyProperty.Register("FloorItemWidth", typeof(bool), typeof(CustomWrapPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));

       
      public bool FloorItemWidth
		{
			get
			{
				return (bool)base.GetValue(FloorItemWidthProperty);
			}
			set
			{
				base.SetValue(FloorItemWidthProperty, value);
			}
		}

		public double MinItemWidth
		{
			get
			{
				return (double)base.GetValue(MinItemWidthProperty);
			}
			set
			{
				base.SetValue(MinItemWidthProperty, value);
			}
		}

		public double MaxItemWidth
		{
			get
			{
				return (double)base.GetValue(MaxItemWidthProperty);
			}
			set
			{
				base.SetValue(MaxItemWidthProperty, value);
			}
		}

		public double ItemMargin
		{
			get
			{
				return (double)base.GetValue(ItemMarginProperty);
			}
			set
			{
				base.SetValue(ItemMarginProperty, value);
			}
		}

		public double RowMargin
		{
			get
			{
				return (double)base.GetValue(RowMarginProperty);
			}
			set
			{
				base.SetValue(RowMarginProperty, value);
			}
		}
         static CustomWrapPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWrapPanel), new FrameworkPropertyMetadata(typeof(CustomWrapPanel)));
        }
		protected override Size MeasureOverride(Size availableSize)
		{
			foreach (UIElement uIElement in base.Children)
			{
				uIElement.Measure(availableSize);
			}
			return new Size(0.0, 0.0);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			double num = 0.0;
			double num2 = 0.0;
			int num3 = this.CalculateItemsCountInOneRow(finalSize);
			double num4 = this.CalculateItemWidth(finalSize.Width, num3);
			for (int i = 0; i < base.Children.Count; i += num3)
			{
				double num5 = 0.0;
				int num6 = 0;
				while (num6 < num3 && i + num6 < base.Children.Count)
				{
					UIElement uIElement = base.Children[i + num6];
					uIElement.Arrange(new Rect(num2, num, num4, uIElement.DesiredSize.Height));
					if (uIElement.DesiredSize.Height > num5)
					{
						num5 = uIElement.DesiredSize.Height;
					}
					num2 += num4 + this.ItemMargin;
					num6++;
				}
				num += num5 + this.RowMargin;
				num2 = 0.0;
			}
			return base.ArrangeOverride(finalSize);
		}

		private int CalculateItemsCountInOneRow(Size finalSize)
		{
			return (int)Math.Floor((finalSize.Width + this.ItemMargin) / (this.MinItemWidth + this.ItemMargin));
		}

		private double CalculateItemWidth(double totalWidth, int itemCountInRow)
		{
			if (itemCountInRow > base.Children.Count)
			{
				itemCountInRow = base.Children.Count;
			}
			double num = (totalWidth - (double)(itemCountInRow - 1) * this.ItemMargin) / (double)itemCountInRow;
			if (num > this.MaxItemWidth)
			{
				num = this.MaxItemWidth;
			}
			return this.FloorItemWidth ? Math.Floor(num) : num;
		}
	}
  
}
