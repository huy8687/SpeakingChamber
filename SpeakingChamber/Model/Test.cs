﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpeakingChamber.Model
{
    [XmlType("test")]
    public class Test : BaseModel
    {
        [XmlElement("code")]
        public string Code { get; set; }
        [XmlArray("parts")]
        public List<Part> Parts { get; set; }
    }
}
