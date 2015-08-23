using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Media;

namespace AscianXbox
{
    /**
     * A simple inpenatrable shield
     * **/

    class Basic_Shield : DrawableGameComponent, Shield
    {
        Timer active;
        bool activate;

        Rectangle position;
        Texture2D shieldSprite;
        SpriteBatch shieldBatch;

        Character castee;
        Shield thisShield;  //Copy of this implementation of Shield interace, so that it may be passed as ref

        public Basic_Shield(int timeAvaliable, Rectangle positionPlaced, ref Character castee, ref global_vars variables, ref Game1 game) : base(game)
        {
            this.position = positionPlaced;
            this.castee = castee;
            shieldBatch = new SpriteBatch(game.GraphicsDevice);
            thisShield = this;

            active = new Timer(timeAvaliable);
            active.Elapsed += new ElapsedEventHandler(timeElapsed);
            activate = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            Collision_Detector.addShield(ref thisShield);
            active.Start();
        }

        protected override void LoadContent()
        {
            shieldSprite = Game.Content.Load<Texture2D>("Ability\\shield");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (activate == false)
            {
                castee.shieldExpired();
                kill();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (activate == true)
            {
                shieldBatch.Begin(); 
                shieldBatch.Draw(shieldSprite, position, Color.White);
                shieldBatch.End();
            }
            base.Draw(gameTime);
        }

        public void timeElapsed(object source, ElapsedEventArgs e)
        {
            activate = false;
            active.Stop();
        }

        public Rectangle getPosition()
        {
            return position;
        }

        public void togglePause()
        {
            if (active.Enabled)
                active.Stop();
            else
                active.Start();
        }

        public void kill()
        {
            Collision_Detector.removeShield(ref thisShield);
            this.Dispose(true);
        }

    }
}
