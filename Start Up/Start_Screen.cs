using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Ascian
{
   
    public class Start_Screen : DrawableGameComponent
    {
        Game1 game;

        Texture2D startscreen;
        SpriteBatch screenbatch;
        KeyboardState keystate;
        global_vars variables;

        //For Fade In effect
        bool fadedone;
        float alphavalue;
        float alphadec;


        public Start_Screen(global_vars vars, Game1 game) : base(game)
        {
            this.game = game;
            variables = vars;
            fadedone = false;
            alphavalue = 0;
            alphadec = .50f;
        }

        public override void Initialize()
        {
            screenbatch = new SpriteBatch(variables.manager.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            startscreen = Game.Content.Load<Texture2D>("Main_Menu\\Start_Menu");
        }
   
        public override void Update(GameTime gameTime)
        {
            if (fadedone == false)
            {
                alphavalue += alphadec;
                if (alphavalue == 255)      //If fading in is not done, increment the fade value
                    fadedone = true;

            }
                keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Enter))
                {
                    Game.Components.Add(new Main_Menu(variables, game));
                    this.Dispose(true);
                }

                base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

                screenbatch.Begin();
                screenbatch.Draw(startscreen, new Rectangle(0, 0, variables.screen_width, variables.screen_height), new Color(150, 150, 150, (byte)MathHelper.Clamp(alphavalue, 0, 256)));
                screenbatch.End();

            base.Draw(gameTime);
        }

    }
}