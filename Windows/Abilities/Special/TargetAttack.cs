using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AscianXbox
{
    public abstract class TargetAttack : DrawableGameComponent
    {
        protected global_vars variables;
        protected Game1 game;
        protected TargetAttack thisTA;
        protected Texture2D texture;
        protected SpriteBatch batch;
        protected float alphavalue = 0;
        protected Rectangle attackPos;
        protected bool targetSet = false;

        public TargetAttack(ref global_vars vars, ref Game1 game)
            : base(game)
        {
            this.game = game;
            this.variables = vars;
            thisTA = this;
        }

        public override void Initialize()
        {
            batch = new SpriteBatch(variables.manager.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            texture = loadAttackTexture();
            getTarget();
            base.LoadContent();
        }

        protected abstract Texture2D loadAttackTexture();
        protected abstract void performAttackUpdate();

        public override void Update(GameTime gameTime)
        {
            if (targetSet)
            {
                performAttackUpdate();
                alphavalue = alphavalue + .05f;
            }
            base.Update(gameTime);
        }

        protected void getTarget()
        {
            Target t = new Target(ref thisTA, ref variables, ref game);
            game.Components.Add(t);
        }

        public override void Draw(GameTime gameTime)
        {
            if (targetSet)
            {
                batch.Begin();
                batch.Draw(texture, attackPos, Color.Lerp(Color.White, Color.Transparent, alphavalue));
                batch.End();
            }
            base.Draw(gameTime);
        }

        public void setPosition(Rectangle pos)
        {
            attackPos = new Rectangle(pos.X - 150, pos.Y - 150, 300, 300);
            targetSet = true;
        }

        public void kill()
        {
            this.Dispose(true);
        }

        public global_vars.sender getSender()
        {
            return global_vars.sender.Special;
        }

        public Rectangle getPosition()
        {
            return attackPos;
        }
    }
}
