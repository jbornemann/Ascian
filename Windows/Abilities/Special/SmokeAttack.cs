using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace AscianXbox
{
    class SmokeAttack : TargetAttack, Special
    {
        List<Enemy> enemiesAffected;
        Timer duration;
        bool started;

        public SmokeAttack(ref global_vars vars, ref Game1 game)
            : base(ref vars, ref game)
        {
            started = false;
            duration = new Timer(8000);
            duration.Elapsed += new ElapsedEventHandler(disable);
            duration.AutoReset = false;
        }

        protected override Texture2D loadAttackTexture()
        {
            return game.Content.Load<Texture2D>("Ability//smokeSquare");
        }

        protected override void performAttackUpdate()
        {
            if (!started)
            {
                enemiesAffected = Collision_Detector.getEnemiesCollided(attackPos);
                disorient();
                duration.Start();
                started = true;
            }
        }

        protected void disorient()
        {
            foreach (Enemy e in enemiesAffected)
            {
                e.setAI(new Enemy.AI(disorientAI));
            }
        }

        public void disorientAI(object data)
        {
            Enemy en = (Enemy)data;
            int x = new Random().Next(3);
            int y = new Random().Next(3);
            int change = new Random().Next(4);
            if (change == 1)
            {
                switch (x)
                {
                    case 0: en.vector_polarity_x = -1; break;
                    case 1: en.vector_polarity_x = 1; break;
                    case 2: en.vector_polarity_x = 0; break;
                }
                switch (y)
                {
                    case 0: en.vector_polarity_y = 1; break;
                    case 1: en.vector_polarity_y = -1; break;
                    case 2: en.vector_polarity_y = 0; break;
                }
            }
            en.followPlayer();
        }

        public void disable(object sender, ElapsedEventArgs e)
        {
            foreach (Enemy es in enemiesAffected)
                es.defaultAI();
            this.Dispose(true);
        }
    }
}
