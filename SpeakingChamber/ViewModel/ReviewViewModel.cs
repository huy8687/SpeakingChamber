using PropertyChanged;
using SpeakingChamber.Extension;
using SpeakingChamber.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SpeakingChamber.ViewModel
{
    public partial class ReviewViewModel : BaseViewModel, IReview
    {
        public Visibility ShowQuestion { get; set; }
        public Visibility ShowVideo { get; set; }
        public string VideoURL { get; set; }
        public Question CurQuestion { get; set; }
        public Part CurPart { get; set; }

        private BaseReviewState _state;
        [DoNotNotify]
        public BaseReviewState State
        {
            get => _state;
            set
            {
                if (_state != null)
                    _state.Exit();
                _state = value;
                if (_state != null)
                    _state.Enter();
            }
        }

        public ICommand CmdStop => new Command(() => Navigation.PopToRoot());
        public ICommand CmdNext => new Command(() => NextQuestion());

        private readonly MediaElement _videoView;

        private readonly Test _curTest;
        private Queue<Part> _parts;
        private Queue<Question> _questions;

        public ReviewViewModel(MediaElement videoView)
        {
            _videoView = videoView;
            _curTest = DataMaster.CurrentTest;
        }

        public override async Task Appearing()
        {
            await base.Appearing();

            _parts = new Queue<Part>(_curTest.Parts);
            NextQuestion();
        }

        public override async Task Disappearing()
        {
            await base.Disappearing();

            _parts = null;
            _questions = null;
        }

        public void NextQuestion()
        {
            if (_questions != null && _questions.Count > 0)
                CurQuestion = _questions.Dequeue();
            else
            {
                if (_parts.Count > 0)
                    CurPart = _parts.Dequeue();
                else
                    Navigation.PopToRoot();
            }
        }

        public void OnCurPartChanged()
        {
            if (CurPart != null)
            {
                _questions = new Queue<Question>(CurPart.Questions);
                NextQuestion();
            }
        }

        public void OnCurQuestionChanged()
        {
            if (CurQuestion != null)
                if (!string.IsNullOrWhiteSpace(CurQuestion.Video) && File.Exists(CurQuestion.Video))
                    State = new ReviewVideoState(this, _videoView);
                else
                    State = new ReviewQuestionState(this);
            else
                throw new InvalidOperationException("CurQuestion cannot be null");
        }
    }
}
