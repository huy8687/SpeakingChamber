namespace SpeakingChamber.ViewModel
{
    public abstract class BaseReviewState
    {
        private bool _entered;
        private bool _exited;

        protected readonly IReview ViewModel;

        public BaseReviewState(IReview viewModel)
        {
            ViewModel = viewModel;
        }

        public void Enter()
        {
            if (!_entered)
                HandleEnter();
            _entered = true;
        }
        public void Exit()
        {
            if (!_exited)
                HandleExit();
            _exited = true;
        }
        protected abstract void HandleEnter();
        protected abstract void HandleExit();
    }
}
