﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Ascian
{
    class SmokeAttack : TargetAttack, Special
    {

        public SmokeAttack(ref global_vars vars, ref Game1 game)
            : base(ref vars, ref game)
        {

        }

        protected override Texture2D loadAttackTexture()
        {
            return null;
        }

        protected override void performAttackUpdate()
        {

        }
    }
}
