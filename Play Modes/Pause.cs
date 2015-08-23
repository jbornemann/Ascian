using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Ascian
{

    public class Pause : DrawableGameComponent
    {
        Game1 game;
        Pause thisPause;
        Playable playmode;
        SpriteBatch pauseBatch;
        Texture2D pause;
        global_vars variables;
        Rectangle layoverPosition;

        //Used For HUD
        SpriteFont font;
        Vector2 HUDLocation;
        String[] stats;

        Timer pauseTimer;
        bool PauseReady = true;
        bool paused = false;

        bool blocked = false;

        public Pause(ref global_vars vars, ref Playable playmode, ref Game1 game):base(game)
        {
            this.game = game;
            this.playmode = playmode;
            thisPause = this;
            variables = vars;
            HUDLocation = variables.HUDLOC;
            layoverPosition = new Rectangle(0, 0, variables.screen_width, variables.screen_height);
            pauseTimer = new Timer(vars.PAUSE_DELAY);
            pauseTimer.Elapsed += new ElapsedEventHandler(pauseDelay);
        }

        public override void Initialize()
        {
            pauseBatch = new SpriteBatch(variables.manager.GraphicsDevice);
            //Used so that the pause overlay is rendered on top of everything else
            this.DrawOrder = 1;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            pause = game.Content.Load<Texture2D>("GamePlay\\Pause_Layover");
            font = game.Content.Load<SpriteFont>("Fonts\\MyFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            stats = playmode.getStats();
            //Check for Input
            ControllerInput.handlePauseScreenCommands(ref thisPause, ref variables);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (paused)
            {
                pauseBatch.Begin();
                pauseBatch.Draw(pause, layoverPosition, Color.White);
                pauseBatch.DrawString(font, "Experience  "+stats[0]+Environment.NewLine+"Game Credit  "+stats[1], HUDLocation, Color.LightSlateGray);
                pauseBatch.End();
            }
            base.Draw(gameTime);
        }

        public void PauseGame()
        {
            if (!blocked)
            {
                if (PauseReady)
                {
                    if (!paused)
                    {
                        paused = true;
                        PauseReady = false;
                        pauseGameComponents();
                        pauseTimer.Start();
                    }
                    else
                    {
                        paused = false;
                        PauseReady = false;
                        unpauseGameComponents();
                        pauseTimer.Start();
                    }
                }
            }
        }

        void pauseGameComponents()
        {
            foreach (GameComponent gc in game.Components.Where(gc => !gc.Equals(thisPause)))
            {
                gc.Enabled = false;
            }

        }

        void unpauseGameComponents()
        {
            foreach (GameComponent gc in game.Components.Where(gc => !gc.Equals(thisPause)))
            {
                gc.Enabled = true;
            }
        }

        void pauseDelay(object sender, ElapsedEventArgs e)
        {
            PauseReady = true;
            pauseTimer.Stop();
        }

        public void pauseBlock()
        {
            blocked = true;
        }

        public void pauseUnblock()
        {
            blocked = false;
        }

        public void Quit()
        {
            if(paused)
                playmode.quit();
        }


    }
}
