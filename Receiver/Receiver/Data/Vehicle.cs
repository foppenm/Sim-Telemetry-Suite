﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Data
{
    public class Vehicle : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
