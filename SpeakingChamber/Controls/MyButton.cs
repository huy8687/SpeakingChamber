using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpeakingChamber.Controls
{
    public class MyButton : Button
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MyButton), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private readonly Image _imageBackground;
        private readonly TextBlock _textBlock;
        private readonly Grid _template;

        public MyButton()
        {
            _imageBackground = new Image()
            {
                Stretch = System.Windows.Media.Stretch.Fill
            };
            _textBlock = new MyTextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
            };
            var uriSource = new Uri("/SpeakingChamber;component/Resources/9.png", UriKind.Relative);
            _imageBackground.Source = new BitmapImage(uriSource);
            _template = new Grid();
            _template.Children.Add(_imageBackground);
            _template.Children.Add(_textBlock);
            Content = _template;
            Foreground = Brushes.White;
            FontSize = 23;
            Height = 40;
            BorderThickness = new Thickness(0);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == TextProperty.Name)
            {
                _textBlock.Text = Text;
            }
            else if (e.Property.Name == IsEnabledProperty.Name)
            {
                Opacity = IsEnabled ? 1 : 0.5;
            }
            else if (e.Property.Name == FontSizeProperty.Name)
            {
                _textBlock.FontSize = FontSize;
            }
            else if (e.Property.Name == FontFamilyProperty.Name)
            {
                _textBlock.FontFamily = FontFamily;
            }
            else if (e.Property.Name == ForegroundProperty.Name)
            {
                _textBlock.Foreground = Foreground;
            }
            else if (e.Property.Name == IsPressedProperty.Name)
            {
                var uriSource = new Uri(IsPressed ?
                     "/SpeakingChamber;component/Resources/10.png"
                    : "/SpeakingChamber;component/Resources/9.png", UriKind.Relative);
                _imageBackground.Source = new BitmapImage(uriSource);
            }
        }
    }
}
