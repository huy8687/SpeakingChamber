using NAudio.Wave;
using System.Windows;

namespace SpeakingChamber.ViewModel
{
    public class ReviewQuestionState : BaseReviewState
    {
        private Mp3FileReader _waveReader;
        private WaveChannel32 _waveChanel;
        private DirectSoundOut _waveOut;

        public ReviewQuestionState(IReview viewModel) : base(viewModel)
        {
        }

        protected override void HandleEnter()
        {
            ViewModel.ShowVideo = Visibility.Hidden;
            ViewModel.ShowQuestion = Visibility.Visible;

            _waveReader = new Mp3FileReader(ViewModel.CurQuestion.LocalRecordedPath);
            _waveChanel = new WaveChannel32(_waveReader) { PadWithZeroes = false };
            _waveOut = new DirectSoundOut();
            _waveOut.Init(_waveChanel);
            _waveOut.PlaybackStopped += WaveOutOnPlaybackStopped;
            _waveOut.Play();
        }

        protected override void HandleExit()
        {
            ReleaseResource();
        }

        private void WaveOutOnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            ReleaseResource();
            ViewModel.NextQuestion();
        }

        private void ReleaseResource()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.PlaybackStopped -= WaveOutOnPlaybackStopped;
                _waveOut.Dispose();
                _waveOut = null;
            }
            if (_waveChanel != null)
            {
                _waveChanel.Dispose();
            }
            if (_waveReader != null)
            {
                _waveReader.Dispose();
            }
        }
    }
}
