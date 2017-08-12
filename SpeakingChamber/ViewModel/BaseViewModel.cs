using PropertyChanged;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeakingChamber.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel
    {
        public static Frame Navigation;

        public virtual Task Appearing()
        {
            return Task.FromResult(true);
        }

        public virtual Task Disappearing()
        {
            return Task.FromResult(true);
        }
    }
}
