﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AscianXbox
{
    public interface Shield : Ability
    {
        Rectangle getPosition();
        void kill();
    }
}
