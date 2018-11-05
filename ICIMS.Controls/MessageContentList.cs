using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ICIMS.Controls
{ 
    [ContainerItem(typeof(ListBoxItem))]
    public class MessageContentList : ListBox
    {
        private readonly Type ItemType;

        public static readonly DependencyProperty OrientationProperty;

        public static readonly DependencyProperty ItemContainerCommandProperty;

        public Orientation Orientation
        {
            get
            {
                return (Orientation)base.GetValue(MessageContentList.OrientationProperty);
            }
            set
            {
                base.SetValue(MessageContentList.OrientationProperty, value);
            }
        }

        public ICommand ItemContainerCommand
        {
            get
            {
                return (ICommand)base.GetValue(ItemContainerCommandProperty);
            }
            set
            {
                base.SetValue(ItemContainerCommandProperty, value);
            }
        }

        static MessageContentList()
        { 
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(MessageContentList), new PropertyMetadata(Orientation.Horizontal));
            ItemContainerCommandProperty = DependencyProperty.Register("ItemContainerCommand", typeof(ICommand), typeof(MessageContentList), new PropertyMetadata(null));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageContentList), new FrameworkPropertyMetadata(typeof(MessageContentList)));
        }

        public MessageContentList()
		{ 
            //ContainerItemAttribute custermAttribute = GetCustermAttribute<ContainerItemAttribute>();
            //if (custermAttribute != null)
            //{
            //    this.ItemType = custermAttribute.ItemType;
            //}
		}

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item.GetType().FullName == this.ItemType.FullName;
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return base.GetContainerForItemOverride();
        }
        //protected override DependencyObject GetContainerForItemOverride()
        //{
        //    return (DependencyObject)Active.SolveInstance(this.ItemType, new object[0]);
        //}
    }
}
