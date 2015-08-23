using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Media;

namespace AscianXbox
{
    public class Creeper : Enemy
    {
        public Creeper(ref global_vars variables, Texture2D enemysprite, ref Character player_char, ref Campaign currentCampaign, Game1 game) : base(ref variables, enemysprite, ref player_char, ref currentCampaign, game) 
        {
        
        }
        public override void special()
        {
            //None.  Creeper doesn't have special abilities   
        }
    }
}
