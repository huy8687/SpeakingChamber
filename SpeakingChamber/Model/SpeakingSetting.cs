using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpeakingChamber.Model
{
    [XmlRoot("setting")]
    public class SpeakingSetting : BaseModel
    {
        [XmlElement("onlineurl")]
        public string OnlineUrl { get; set; }
        [XmlElement("localpath")]
        public string LocalPath { get; set; }
        [XmlElement("networkpath")]
        public string NetworkPath { get; set; }
        [XmlElement("devnumber")]
        public int? DevNumber { get; set; }
        [XmlElement("micchanel")]
        public int? MicChanel { get; set; }

        [XmlIgnore]
        public string UserLocalPath => LocalPath + DataMaster.UserName.Trim();
    }
}
