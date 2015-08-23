using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace AscianXbox
{
    class Shadow_Basic_Attack : Basic_Attack
    {
        public Shadow_Basic_Attack(int timeAvaliable, Rectangle startpos, char direction, global_vars.sender sender, ref global_vars variables, ref Game1 game):base(timeAvaliable, startpos, direction, sender, ref variables, ref game)
        {
            
        }

        protected override void loadAttackSpriteType()
        {
            attackSprite = game.Content.Load<Texture2D>("Ability\\ShadAtk");
        }

        protected override void doAttackSound()
        {
            attackSound = game.Content.Load<SoundEffect>("Sounds\\shadowAttackSound");
            attackSound.Play();
        }

    }
}
