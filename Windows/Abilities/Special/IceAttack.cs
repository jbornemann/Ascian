using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace AscianXbox
{
    class IceAttack : TargetAttack, Special
    {
        bool setUp;
        Timer duration;
        List<Enemy> enemiesAffected;

        public IceAttack(ref global_vars vars, ref Game1 game)
            : base(ref vars, ref game)
        {
            setUp = false;
            duration = new Timer(8000);
            duration.Elapsed += new ElapsedEventHandler(unfreeze);
            duration.AutoReset = false;
        }

        protected override Texture2D loadAttackTexture()
        {
            return game.Content.Load<Texture2D>("Ability//iceSquare");
        }

        protected override void performAttackUpdate()
        {
            if (!setUp)
            {
                enemiesAffected = Collision_Detector.getEnemiesCollided(attackPos);
                freeze();
                setUp = true;
            }
        }

        protected void freeze()
        {
            duration.Start();
            foreach (Enemy e in enemiesAffected)
            {
                e.Enabled = false;
            }
        }

        public void unfreeze(object sender, ElapsedEventArgs e)
        {
            duration.Dispose();
            foreach (Enemy es in enemiesAffected)
            {
                es.Enabled = true;
            }
            base.kill();
        }
    }
}
