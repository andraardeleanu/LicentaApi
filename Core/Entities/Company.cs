﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Cui {  get; set; }
        public string CreatedBy { get; set; }
    }
}
