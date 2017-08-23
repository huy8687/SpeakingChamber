using SpeakingChamber.Model;
using System.Windows;

namespace SpeakingChamber.ViewModel
{
    public interface IReview
    {
        Visibility ShowQuestion { get; set; }
        Visibility ShowVideo { get; set; }
        string VideoURL { get; set; }
        Question CurQuestion { get; set; }
        Part CurPart { get; set; }
        BaseReviewState State { get; set; }

        void NextQuestion();
    }
}
