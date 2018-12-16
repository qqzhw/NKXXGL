using ICIMS.Modules.BusinessManages.ViewModels;
using Microsoft.Win32;
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
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// ItemDefineView.xaml 的交互逻辑
    /// </summary>
    public partial class ItemDefineView : UserControl
    {
        public ItemDefineView(ItemDefineViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.View = this;
            this.radInfo.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(viewModel.OnDoubleClick), true);
        }

        private void OnExportData()
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "xls";
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|全部文件 (*.*)|*.*", "xls", ExportFormat.Xlsx);
            dialog.FilterIndex = 1;
            radInfo.ElementExporting -= this.ElementExporting;
            radInfo.ElementExporting += this.ElementExporting;
            if (dialog.ShowDialog() == true)
            {
                using (var stream = dialog.OpenFile())
                {
                    var exportOptions = new GridViewExportOptions();
                    exportOptions.Format = ExportFormat.Xlsx;
                    exportOptions.ShowColumnFooters = true;
                    exportOptions.ShowColumnHeaders = true;
                    exportOptions.ShowGroupFooters = true;
                    exportOptions.Encoding = Encoding.Unicode;

                    radInfo.Export(stream, exportOptions);
                }
            }
        }

        private void ElementExporting(object sender, GridViewElementExportingEventArgs e)
        {
            var visParameters = e.VisualParameters as GridViewExcelMLVisualExportParameters;
            if (e.Element == ExportElement.Row)
            {
                visParameters.RowHeight = radInfo.RowHeight;
            }
            if (e.Element == ExportElement.Cell && (e.Context as GridViewBoundColumnBase).UniqueName == "")
            {
                visParameters.StyleId = "Name";
            }
            if (e.Element == ExportElement.Cell && (e.Context as GridViewBoundColumnBase).UniqueName == "UnitPrice")
            {
                visParameters.StyleId = "UnitPrice";
            }
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            OnExportData();
        }
    }
}
