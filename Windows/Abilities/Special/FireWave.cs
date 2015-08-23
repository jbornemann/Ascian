using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AscianXbox
{
    class FireWave : Basic_Attack, Special
    {
        Special thisFirewave;
        Texture2D firewave1;
        Texture2D firewave2;
        int switchTex = 0;
        
        public FireWave(int timeAvaliable, Rectangle startpos, char direction, global_vars.sender sender, ref global_vars variables, ref Game1 game)
            : base(timeAvaliable, startpos, direction, sender, ref variables, ref game)
        {
            thisFirewave = this;
        }

        protected override void loadAttackSpriteType()
        {
            firewave1 = game.Content.Load<Texture2D>("Ability//Fire_Wall1");
            firewave2 = game.Content.Load<Texture2D>("Ability//Fire_Wall2");
            attackSprite = firewave1;
        }

        protected override void doAttackSound()
        {
            attackSound = game.Content.Load<SoundEffect>("");
            attackSound.Play();
        }

        protected override void attackUpdate()
        {
            switch (direction)
            {
                case 'u': position.Y -= 10; break;
                case 'd': position.Y += 10; break;
                case 'l': position.X -= 10; break;
                case 'r': position.X += 10; break;
            }
            switchTex = (switchTex < 5) ? switchTex+1 : 0;
            if (switchTex == 5)
            {
                if (attackSprite == firewave1)
                    attackSprite = firewave2;
                else
                    attackSprite = firewave1;
            }
            if (activate == false)
                this.Dispose(true);
        }

        public override void kill()
        {
            Collision_Detector.removeSpecial(ref thisFirewave);
            base.kill();
        }

        protected override void addToCollisionDetector()
        {
            Collision_Detector.addSpecial(ref thisFirewave);
        }
      
    }
}
