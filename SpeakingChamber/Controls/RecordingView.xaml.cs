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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeakingChamber.Controls
{
    /// <summary>
    /// Interaction logic for RecordingView.xaml
    /// </summary>
    public partial class RecordingView : UserControl
    {
        public static readonly DependencyProperty IsRecordingProperty =
            DependencyProperty.Register(nameof(IsRecording), typeof(bool), typeof(RecordingView), new UIPropertyMetadata(true));

        public bool IsRecording
        {
            get { return (bool)GetValue(IsRecordingProperty); }
            set { SetValue(IsRecordingProperty, value); }
        }

        private readonly DoubleAnimation da = new DoubleAnimation()
        {
            From = 1,
            To = 0,
            Duration = new Duration(TimeSpan.FromSeconds(1.5)),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };

        public RecordingView()
        {
            InitializeComponent();
            RecordingText.BeginAnimation(OpacityProperty, da);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == IsRecordingProperty)
            {
                if (IsRecording)
                {
                    RecordingText.BeginAnimation(OpacityProperty, da);
                    InRecordingState.Visibility = Visibility.Hidden;
                    RecordingState.Visibility = Visibility.Visible;

                }
                else
                {
                    InRecordingState.Visibility = Visibility.Visible;
                    RecordingState.Visibility = Visibility.Hidden;
                    RecordingText.BeginAnimation(OpacityProperty, null);
                }
            }
        }
    }
}
