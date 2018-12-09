using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ICIMS.Controls
{
    public partial class RowNumberColumn:GridViewDataColumn
    {
        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
             
            TextBlock textBlock = cell.Content as TextBlock;

            if (textBlock == null)
            {
                textBlock = new TextBlock();
            } 
            textBlock.Text = (this.DataControl.Items.IndexOf(dataItem) + 1).ToString();
            //textBlock.Text = ((this.DataControl.ItemsSource as IList).IndexOf(dataItem) + 1).ToString();
            // this.DisplayIndex = (this.DataControl.Items.IndexOf(dataItem) + 1);
           // cell.Value = (this.DataControl.Items.IndexOf(dataItem) + 1);
            cell.Content = textBlock.Text;
            return textBlock;
        }
    }
}
