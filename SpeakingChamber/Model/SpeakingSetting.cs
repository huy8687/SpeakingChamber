using System.IO;
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
        public int DevNumber { get; set; }
        [XmlElement("micchanel")]
        public int MicChanel { get; set; }
        [XmlElement("username")]
        public string UserName { get; set; }
        [XmlElement("dob")]
        public string DateOfBirth { get; set; }

        [XmlIgnore]
        public string UserLocalPath => Path.Combine(LocalPath, DataMaster.UserName.Trim() + "_" + DataMaster.CurrentTest?.Code);
    }
}
