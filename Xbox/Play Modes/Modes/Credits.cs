using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AscianXbox
{
    public class Credits : DrawableGameComponent
    {
        Game1 game;
        Credits thisCredit;
        global_vars variables;
        SpriteBatch batch;
        Texture2D tex;
        Rectangle area;

        public Credits(ref global_vars vars, ref Game1 game)
            : base(game)
        {
            this.game = game;
            this.variables = vars;
            thisCredit = this;
            area = new Rectangle(0, 0, variables.screen_width, variables.screen_height);
        }

        public override void Initialize()
        {
            batch = new SpriteBatch(variables.manager.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            tex = game.Content.Load<Texture2D>("GamePlay//Credits");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            ControllerInput.handleCreditCommands(ref thisCredit);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            batch.Begin();
            batch.Draw(tex, area, Color.White);
            batch.End();
            base.Draw(gameTime);
        }

        public void exit()
        {
            Loader l = new Loader(global_vars.loadType.Main, ref variables, ref game);
            this.Dispose(true);
            game.Components.Add(l);
        }
    }
}
