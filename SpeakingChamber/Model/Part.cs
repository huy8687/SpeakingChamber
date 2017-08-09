using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber.Model
{
    public class Part : BaseModel
    {
        public string Name { get; set; }
        public IList<Question> Questions { get; set; }
    }
}
