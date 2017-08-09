﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
