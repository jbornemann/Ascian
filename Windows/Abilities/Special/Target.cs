using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AscianXbox
{
    public class Target : DrawableGameComponent
    {
        Game1 game;
        global_vars variables;
        SpriteBatch batch;
        Texture2D crosshair;
        Rectangle position;
        Target thisTarget;
        TargetAttack attack;

        public Target(ref TargetAttack ta, ref global_vars vars, ref Game1 game) : base(game)
        {
            this.game = game;
            this.variables = vars;
            thisTarget = this;
            position = new Rectangle(vars.screen_width/2, vars.screen_height/2, 100, 100);
            attack = ta;
        }

        public override void Initialize()
        {
            pauseGameComponents();
            batch = new SpriteBatch(variables.manager.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            crosshair = Game.Content.Load<Texture2D>("Gameplay//CrossHair_Red");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            ControllerInput.handleTargetCommands(ref thisTarget, ref variables);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            batch.Begin();
            batch.Draw(crosshair, position, Color.White);
            batch.End();
            base.Draw(gameTime);
        }

        public void move(char dir)
        {
            switch (dir)
            {
                case 'u' : position.Y = position.Y > 0 ? position.Y-5 : position.Y;break;
                case 'd' : position.Y = position.Y < variables.screen_height - variables.CHARACTER_HEIGHT ? position.Y+5 : position.Y;break;
                case 'l' : position.X = position.X > 0 ? position.X-5 : position.X;break;
                case 'r' : position.X = position.X < variables.screen_width - variables.CHARACTER_WIDTH ? position.X+5 : position.X;break;
            }
        }

        public void select()
        {
            attack.setPosition(position);
            unpauseGameComponents();
            this.Dispose(true);
        }

        void pauseGameComponents()
        {
            foreach (GameComponent gc in game.Components.Where(gc => !gc.Equals(thisTarget)))
            {
                gc.Enabled = false;
            }
        }

        void unpauseGameComponents()
        {
            foreach (GameComponent gc in game.Components.Where(gc => !gc.Equals(thisTarget)))
            {
                gc.Enabled = true;
            }
        }
    }
}
