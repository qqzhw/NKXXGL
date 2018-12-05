using ICIMS.Modules.BusinessManages.ViewModels;
using Saraff.Twain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// ScanFileView.xaml 的交互逻辑
    /// </summary>
    public partial class ScanFileView : UserControl
    {

        private string mRunPath = "";

        private string mImagePath = "";

        private Twain32 mTwain = new Twain32();

        private string PDFFileName = $"{Guid.NewGuid().ToString()}.PDF";

        private bool NewScanPage = false;

        private DataTable dtFile = new DataTable();

        private int PageCount = 0;
        public ScanFileView(ScanFileViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel; 
            this.Loaded += ScanFileView_Loaded;
            //this.FadeIn = (Storyboard)this.LayoutRoot.Resources["FadeIn"];
            //this.FadeOut = (Storyboard)this.LayoutRoot.Resources["FadeOut"];

            //this.imageEditor.ToolSettingsContainer = this.settingsContainer;

            //ImageExampleHelper.LoadSampleImage(this.imageEditor, "CustomUIImage.jpg");
            UpdateImageSources();
           
          
        }

        private void ScanFileView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void changeTools_Click(object sender, RoutedEventArgs e)
        {

        }
        private Storyboard FadeIn;
        private Storyboard FadeOut;
        private ImageSource openImageSource;
        private ImageSource saveImageSource;

       
       
 
        private void Example_ThemeChanged(object sender, System.EventArgs e)
        {
            UpdateImageSources();
        }

        private void UpdateImageSources()
        {
            string currTheme ="Office2016";
            switch (currTheme)
            {
                
                case "Fluent_Light":
                case "Fluent_Dark":
                    openImageSource = RadGlyph.GetImageSource(this.GetGlyph("&#xe901;"), 16, new SolidColorBrush(FluentPalette.Palette.IconColor), "TelerikWebUI");
                    saveImageSource = RadGlyph.GetImageSource(this.GetGlyph("&#xe109;"), 16, new SolidColorBrush(FluentPalette.Palette.IconColor), "TelerikWebUI");
                    break;
                case "Material":
                    openImageSource = RadGlyph.GetImageSource(this.GetGlyph("&#xe901;"), 16, new SolidColorBrush(MaterialPalette.Palette.IconColor), "TelerikWebUI");
                    saveImageSource = RadGlyph.GetImageSource(this.GetGlyph("&#xe109;"), 16, new SolidColorBrush(MaterialPalette.Palette.IconColor), "TelerikWebUI");
                    break;
                case "Office2016":
                case "Office2016Touch":
                    openImageSource = RadGlyph.GetImageSource(this.GetGlyph("&#xe901;"), 16, new SolidColorBrush(Office2016Palette.Palette.IconColor), "TelerikWebUI");
                    saveImageSource = RadGlyph.GetImageSource(this.GetGlyph("&#xe109;"), 16, new SolidColorBrush(Office2016Palette.Palette.IconColor), "TelerikWebUI");
                    break;
                default:
                    openImageSource = new BitmapImage(new Uri(@"/Telerik.Windows.Controls.ImageEditor;Component/Images/open.png", UriKind.RelativeOrAbsolute));
                    saveImageSource = new BitmapImage(new Uri(@"/Telerik.Windows.Controls.ImageEditor;Component/Images/save.png", UriKind.RelativeOrAbsolute));
                    break;
            }
            this.OpenImageEditorButton.Image = openImageSource;
            this.SaveImageEditorButton.Image = saveImageSource;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.FadeOut.Stop();
            this.FadeIn.Begin();
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.FadeIn.Stop();
            this.FadeOut.Begin();
        }

        private string GetGlyph(string hexCode)
        {
            string glyphInt = hexCode.Substring(3, 4);
            var character = (char)int.Parse(glyphInt, NumberStyles.HexNumber);
            return character.ToString();
        }
    }
}
