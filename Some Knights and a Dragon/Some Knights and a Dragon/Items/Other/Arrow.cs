﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Some_Knights_and_a_Dragon.Items.Other
{
    class Arrow : Item
    {
        public Arrow()
        {
            Name = "Arrow";
            Description = "An arrow that should be used in bows, duh!";
            LoadSprite("Items/Other/arrow");
        }
    }
}
