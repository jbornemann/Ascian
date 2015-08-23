using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Ascian
{
    public abstract class Enemy : DrawableGameComponent
    {
        protected Game1 game;
        protected Campaign currentCampaign;
        protected Enemy thisEnemy;

        protected Texture2D enemy;
        protected SpriteBatch enemybatch;
        protected Rectangle enemypos;


        protected Character player;
        protected Rectangle playerpos;
        protected SoundEffect dieSound;

        protected int vector_polarity_x;  //+1, -1, 0 (neutral)
        protected int vector_polarity_y;  //Used to determine enemy move direction vector
        protected bool stop = false;
       
        protected global_vars variables;

        protected float rotation = 0;

        public Enemy(ref global_vars variables, Texture2D enemysprite, ref Character player_char, ref Campaign currentCampaign,  Game1 game): base(game)
        {
            this.enemy = enemysprite;
            this.player = player_char;
            this.variables = variables;
            this.game = game;
            this.currentCampaign = currentCampaign;
            thisEnemy = this;
        }

        public override void Initialize()
        {
            setPosition();
            enemybatch = new SpriteBatch(variables.manager.GraphicsDevice);
            Collision_Detector.addEnemy(ref thisEnemy);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            dieSound = game.Content.Load<SoundEffect>("Sounds\\Hit");
            base.LoadContent();
        }

        protected virtual void setPosition()
        {
            int x = new Random().Next(12);
            //Randomly assign a spawn point for the enemy
            enemypos = new Rectangle(variables.ENEMY_SPAWN_POSITIONS[x].X, variables.ENEMY_SPAWN_POSITIONS[x].Y, variables.ENEMY_WIDTH, variables.ENEMY_HEIGHT);
        }

        public void kill()
        {
            dieSound.Play();
            currentCampaign.killEnemy(this.ToString());
            this.Dispose(true);
        }

        public Rectangle getPosition()
        {
            return enemypos;
        }

        protected void findPlayer()
        {
            Rectangle playerT = player.getPosition();
            //We are interested in the player's position center
            playerpos = new Rectangle(playerT.X + (variables.CHARACTER_WIDTH / 2), playerT.Y + (variables.CHARACTER_HEIGHT / 2), playerT.Width, playerT.Height);
            if (playerpos.X > enemypos.X)
                vector_polarity_x = 1;
            else if (playerpos.X < enemypos.X)
                vector_polarity_x = -1;
            else if (playerpos.X == enemypos.X)
                vector_polarity_x = 0;

            if (playerpos.Y > enemypos.Y)
                vector_polarity_y = 1;
            else if (playerpos.Y < enemypos.Y)
                vector_polarity_y = -1;
            else if (playerpos.Y == enemypos.Y)
                vector_polarity_y = 0;
        }

        protected void followPlayer()
        {
            if (stop == false)
            {
                if (vector_polarity_x == -1)
                    enemypos.X -= variables.ENEMY_MOVE_SPEED;
                else if (vector_polarity_x == 1)
                    enemypos.X += variables.ENEMY_MOVE_SPEED;
                if (vector_polarity_y == -1)
                    enemypos.Y -= variables.ENEMY_MOVE_SPEED;
                else if (vector_polarity_y == 1)
                    enemypos.Y += variables.ENEMY_MOVE_SPEED;
            }
        }

        public override void Update(GameTime gameTime)
        {
            findPlayer();
            followPlayer();
            special();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            enemybatch.Begin(SpriteSortMode.Texture, null);
            enemybatch.Draw(enemy, enemypos, null, Color.White, rotation, new Vector2(15,15), SpriteEffects.None, 0);
            enemybatch.End();
            base.Draw(gameTime);
        }

        public abstract void special();

    }
}
