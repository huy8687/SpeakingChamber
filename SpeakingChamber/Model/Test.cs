﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber.Model
{
    public class Test : BaseModel
    {
        public string Code { get; set; }
        public IList<Part> Parts { get; set; }
    }
}