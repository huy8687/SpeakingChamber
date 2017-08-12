using System.Collections.Generic;
using System.Xml.Serialization;

namespace SpeakingChamber.Model
{
    [XmlType("part")]
    public class Part : BaseModel
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlArray("questions")]
        public List<Question> Questions { get; set; }
    }
}
