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


namespace AscianXbox
{

    public class Timer : Microsoft.Xna.Framework.GameComponent
    {
        protected delegate void elapsed();
        public elapsed Elapsed;
        public bool AutoReset = true;
        Game1 game;
        double time;
        int initialElapsed;
        bool setUp;
        bool start;
        Timer thisTimer;


        public Timer(double time, Game1 game)
            : base(game)
        {
            this.time = time;
            setUp = false;
            start = false;
            thisTimer = this;
            this.game = game;
        }

        public override void Initialize()
        {
            game.Components.Add(thisTimer);
            base.Initialize();
        }

        public void Start()
        {
            start = true;
        }

        public void Stop()
        {
            start = false;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public override void Update(GameTime gameTime)
        {
            if(!setUp && start)
            {
                initialElapsed = gameTime.ElapsedGameTime.Milliseconds;
                setUp = true;
                start = false;
            }
            if (initialElapsed - gameTime.ElapsedGameTime.Milliseconds >= time)
            {
                try
                {
                    Elapsed.Invoke();
                }
                catch (Exception) { }
                if(AutoReset) start = true;
            }
            base.Update(gameTime);
        }
    }
}
