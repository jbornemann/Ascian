using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Ascian
{
    class ShadowEater : Enemy
    {
        Timer ShootTimer = new Timer(3000);
        SoundEffect attackSound;
        bool timerSetUp = false;
        bool shootNow = false;

        public ShadowEater(ref global_vars variables, Texture2D enemysprite, ref Character player_char, ref Campaign currentCampaign, Game1 game) : base(ref variables, enemysprite, ref player_char, ref currentCampaign, game) 
        {
            attackSound = game.Content.Load<SoundEffect>("Sounds\\shadowAttackSound");
        }
        public override void special()
        {
            if (!timerSetUp)
                setupTimer();
            if (shootNow)
            {
                shoot();
            }
            rotate();
        }

        protected override void  setPosition()
        {
            int x = new Random().Next(12);       //Randomly assign a spawn point for the enemy
            enemypos = new Rectangle(variables.ENEMY_SPAWN_POSITIONS[x].X, variables.ENEMY_SPAWN_POSITIONS[x].Y, variables.SHADOW_WIDTH, variables.SHADOW_HEIGHT);
        }

        void setupTimer()
        {
            ShootTimer.AutoReset = false;
            ShootTimer.Elapsed += new ElapsedEventHandler(setShoot);
            timerSetUp = true;
            ShootTimer.Start();
        }

        void setShoot(object source, ElapsedEventArgs e)
        {
            shootNow = true;
            ShootTimer.Stop();
        }

        void shoot()
        {
            int random = new Random().Next(4);
            char dir = 'u';
            switch (random)
            {
                case 0: dir = 'u'; break;
                case 1: dir = 'd'; break;
                case 2: dir = 'l'; break;
                case 3: dir = 'r'; break;
            }
            int posx = getPosition().X;
            int posy = getPosition().Y;
            Game.Components.Add(new Shadow_Basic_Attack(4000, new Rectangle(posx, posy, 50, 50), dir, global_vars.sender.Enemy, ref variables, ref game));
            shootNow = false;
            ShootTimer.Start();
        }

        void rotate()
        {
            rotation += .01f;

        }

    }
}
