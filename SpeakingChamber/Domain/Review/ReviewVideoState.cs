using System.Windows;
using System.Windows.Controls;

namespace SpeakingChamber.ViewModel
{
    public class ReviewVideoState : BaseReviewState
    {
        private readonly MediaElement _videoView;

        public ReviewVideoState(IReview viewModel) : base(viewModel)
        {
        }

        public ReviewVideoState(IReview viewModel, MediaElement videoView) : this(viewModel)
        {
            _videoView = videoView;
        }

        protected override void HandleEnter()
        {
            ViewModel.ShowVideo = Visibility.Visible;
            ViewModel.ShowQuestion = Visibility.Hidden;

            _videoView.MediaFailed += _videoView_MediaFailed;
            _videoView.MediaEnded += _videoView_MediaEnded;
            ViewModel.VideoURL = ViewModel.CurQuestion.Video;
        }

        protected override void HandleExit()
        {
            _videoView.Stop();
            _videoView.MediaFailed -= _videoView_MediaFailed;
            _videoView.MediaEnded -= _videoView_MediaEnded;
        }

        private void _videoView_MediaEnded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => ViewModel.State = new ReviewQuestionState(ViewModel));
        }

        private void _videoView_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => ViewModel.State = new ReviewQuestionState(ViewModel));
        }
    }
}
