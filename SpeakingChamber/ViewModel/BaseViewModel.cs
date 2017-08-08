using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyChanged;
using System.Threading.Tasks;

namespace SpeakingChamber.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel
    {
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
