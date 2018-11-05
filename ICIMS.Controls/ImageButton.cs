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
    ///
    ///     <MyNamespace:ImageButton/>
    ///
    /// </summary>
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageNormalProperty = DependencyProperty.Register("ImageNormal", typeof(string), typeof(ImageButton), new UIPropertyMetadata(null, new PropertyChangedCallback(ImgNormal_OnPropertyChanged)));
         
        public static readonly DependencyProperty ImagePressedProperty = DependencyProperty.Register("ImagePressed", typeof(string), typeof(ImageButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ImageDisabledProperty = DependencyProperty.Register("ImageDisabled", typeof(string), typeof(ImageButton), new UIPropertyMetadata(null));
      
        public static readonly DependencyProperty IsPoupupOpenProperty = DependencyProperty.Register("IsPoupupOpen", typeof(bool), typeof(ImageButton), new UIPropertyMetadata(false));

        public static readonly DependencyProperty PoupupProperty = DependencyProperty.Register("Poupup", typeof(string), typeof(ImageButton), new UIPropertyMetadata(null));

        public string ImageNormal
        {
            get { return (string)GetValue(ImageNormalProperty); }
            set { SetValue(ImageNormalProperty, value); }
        }
        public string ImagePressed
        {
            get { return (string)GetValue(ImagePressedProperty); }
            set { SetValue(ImagePressedProperty, value); }
        }
        public string ImageDisabled
        {
            get { return (string)GetValue(ImageDisabledProperty); }
            set { SetValue(ImageDisabledProperty, value); }
        }
        public bool IsPoupupOpen
        {
            get { return (bool)GetValue(IsPoupupOpenProperty); }
            set { SetValue(IsPoupupOpenProperty, value); }
        }
        public string Poupup
        {
            get { return (string)GetValue(PoupupProperty); }
            set { SetValue(PoupupProperty, value); }
        }

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        private static void ImgNormal_OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageButton)
            {
                ImageButton imageButton = d as ImageButton;
                if (string.IsNullOrEmpty(imageButton.ImageDisabled) && e.NewValue is string)
                {
                    imageButton.ImageDisabled = (string)e.NewValue;
                }
            }
        }
    }
}
