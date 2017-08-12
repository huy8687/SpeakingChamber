using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeakingChamber.Extension
{
    public static class NavigationExtensions
    {
        public static void PopToRoot(this Frame nav)
        {
            if (nav.BackStack.Count() < 2)
                return;
            while(nav.BackStack.Count() >= 2)
            {
                nav.RemoveBackEntry();
            }
            nav.GoBack();
        }
    }
}
