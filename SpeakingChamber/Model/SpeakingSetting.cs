using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber.Model
{
    public class SpeakingSetting : BaseModel
    {
        public string OnlineUrl { get; set; }
        public string LocalPath { get; set; }
        public string NetworkPath { get; set; }
    }
}
