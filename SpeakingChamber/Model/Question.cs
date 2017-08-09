using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber.Model
{
    public class Question : BaseModel
    {
        public string Video { get; set; }
        public string Content { get; set; }
        public string Guide { get; set; }
        public int Duration { get; set; }
    }
}
