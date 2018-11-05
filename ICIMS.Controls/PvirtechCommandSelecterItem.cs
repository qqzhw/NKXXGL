using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ICIMS.Controls
{
    public class PvirtechCommandSelecterItem:ListBoxItem
    {
        public static readonly DependencyProperty EnableSelectableProperty = DependencyProperty.Register("EnableSelectable", typeof(bool), typeof(PvirtechCommandSelecterItem), new PropertyMetadata(true));

        public bool EnableSelectable
        {
            get
            {
                return (bool)base.GetValue(PvirtechCommandSelecterItem.EnableSelectableProperty);
            }
            set
            {
                base.SetValue(PvirtechCommandSelecterItem.EnableSelectableProperty, value);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            bool flag = !this.EnableSelectable;
            if (flag)
            {
                e.Handled = true;
            }
            base.OnMouseDown(e);
        }
    }
}
