using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SpeakingChamber.Model;
using System.Windows;
using NAudio.Wave;
using SpeakingChamber.Pages;
using System.Linq;

namespace SpeakingChamber.ViewModel
{
    public class MicCheckingViewModel : BaseViewModel
    {
        public IList<WaveInCapabilities> InputSources { get; private set; }
        public WaveInCapabilities? SelectedInput { get; set; }
        public bool EnableComplete { get; set; } = true;

        public Visibility VisibleConfirm => EnableComplete ? Visibility.Hidden : Visibility.Visible;
        public Visibility VisibleInputSource => (InputSources != null && InputSources.Count > 1 && !SelectedInput.HasValue) ? Visibility.Visible : Visibility.Hidden;

        private WaveFileReader _waveReader;
        private WaveChannel32 _waveChanel;
        private WaveIn _inputStream;
        private DirectSoundOut _waveOut;
        private WaveFileWriter _waveWriter;

        public ICommand CmdComplete => new Command(() =>
        {
            if (SelectedInput.HasValue)
            {
                EnableComplete = false;

                ReleaseResource();

                _waveReader = new WaveFileReader("sample.wav");
                _waveChanel = new WaveChannel32(_waveReader) { PadWithZeroes = false };
                _waveOut = new DirectSoundOut();
                _waveOut.Init(_waveChanel);
                _waveOut.PlaybackStopped += WaveOutOnPlaybackStopped;
                _waveOut.Play();
            }
            else
            {
                MessageBox.Show("Please select an Microphone card!");
            }
        });

        public ICommand CmdYes => new Command(() =>
        {
            DataMaster.SaveSetting();
            Navigation.Navigate(new StartExamPage());
        });

        public ICommand CmdNo => new Command(() =>
        {
            OpenMicFailedPage();
        });

        public override async Task Appearing()
        {
            await base.Appearing();
            var temp = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                temp.Add(WaveIn.GetCapabilities(i));
            }
            InputSources = temp;
            if (temp.Count == 0)
            {
                OpenMicFailedPage();
            }
            else if (temp.Count == 1)
            {
                SelectedInput = InputSources[0];
            }
            else if (DataMaster.Setting.DevNumber.HasValue && InputSources.Count > DataMaster.Setting.DevNumber.Value)
            {
                SelectedInput = InputSources[DataMaster.Setting.DevNumber.Value];
            }
        }

        public override async Task Disappearing()
        {
            await base.Disappearing();
            ReleaseResource();
        }

        public void OnSelectedInputChanged()
        {
            var devNumber = InputSources.IndexOf(SelectedInput.Value);

            DataMaster.Setting.DevNumber = devNumber;
            DataMaster.Setting.MicChanel = SelectedInput.Value.Channels;

            ReleaseResource();

            try
            {
                _inputStream = new WaveIn
                {
                    DeviceNumber = devNumber,
                    WaveFormat = new WaveFormat(44100, SelectedInput.Value.Channels)
                };
                _inputStream.DataAvailable += InputStreamOnDataAvailable;
                _waveWriter = new WaveFileWriter("sample.wav", _inputStream.WaveFormat);
                _inputStream.StartRecording();
            }
            catch (Exception)
            {
                OpenMicFailedPage();
            }
        }

        private void InputStreamOnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_waveWriter == null) return;
            try
            {
                _waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
                _waveWriter.Flush();
            }
            catch (Exception)
            {
                OpenMicFailedPage();
            }
        }

        private void OpenMicFailedPage()
        {
            ReleaseResource();
            Navigation.Navigate(new MicCheckingFailedPage());
        }

        private void WaveOutOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
            ReleaseResource();
        }

        private void ReleaseResource()
        {
            if (_waveWriter != null)
            {
                _waveWriter.Close();
                _waveWriter.Dispose();
                _waveWriter = null;
            }
            if (_inputStream != null)
            {
                _inputStream.StopRecording();
                _inputStream.DataAvailable -= InputStreamOnDataAvailable;
                _inputStream.Dispose();
                _inputStream = null;
            }
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
