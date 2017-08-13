using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpeakingChamber.Controls
{
    public class MyTextBlock : TextBlock
    {
        public MyTextBlock()
        {
            Foreground = Brushes.White;
            FontSize = 23;
        }
    }
}
