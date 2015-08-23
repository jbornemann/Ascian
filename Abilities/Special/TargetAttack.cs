using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ascian
{
    public abstract class TargetAttack : DrawableGameComponent
    {
        global_vars variables;
        Game1 game;
        TargetAttack thisTA;
        Texture2D texture;
        SpriteBatch batch;
        int alphavalue = 255;
        Rectangle attackPos;
        bool targetSet = false;

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
                alphavalue--;
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
                batch.Draw(texture, attackPos, new Color(255, 255, 255, (byte)MathHelper.Clamp(alphavalue, 0, 256)));
                batch.End();
            }
            base.Draw(gameTime);
        }

        public void setPosition(Rectangle pos)
        {
            attackPos = pos;
            targetSet = true;
        }

        public void kill()
        {

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
