using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace AscianXbox
{
    public class Main_Menu : DrawableGameComponent
    {

        Game1 game;
        Main_Menu thisMain;
        Song theme;
        int current_delay;
        int selection_delay;

        //Graphical items
        Texture2D menu;
        Texture2D menu2;
        Texture2D menu3;
        Texture2D menu4;
        SpriteBatch menubatch;
        global_vars variables;

        bool deviceSelected;

        protected enum option
        {
            campaign,
            deathmatch,
            upgrade,
            credits
        };

        //Option selection items
        option optionselected; 

        public Main_Menu(global_vars vars, Game1 game): base(game)
        {
            this.game = game;
            thisMain = this;
            selection_delay = vars.MENU_DELAY;
            current_delay = selection_delay;
            variables = vars;
            optionselected = option.campaign;
            deviceSelected = false;
        }

        public override void Initialize()
        {
            menubatch = new SpriteBatch(variables.manager.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            menu = Game.Content.Load<Texture2D>("Main_Menu\\Main_Menu2");
            menu2 = Game.Content.Load<Texture2D>("Main_Menu\\Main_Menu3");
            menu3 = Game.Content.Load<Texture2D>("Main_Menu\\Main_Menu4");
            menu4 = Game.Content.Load<Texture2D>("Main_Menu\\Main_Menu5");
            //If the theme song is not already playing, it needs to be loaded and played
            if (MediaPlayer.State != MediaState.Playing)
            {
                theme = Game.Content.Load<Song>("Sounds\\theme");
                MediaPlayer.Play(theme);
            }
            base.LoadContent();
        }

        public void SelectOption()
        {
            if (deviceSelected)
            {
                switch (optionselected)
                {
                    case option.campaign:
                        {
                            Game.Components.Add(new Loader(global_vars.loadType.Campaign, ref variables, ref game));
                        } break;
                    case option.deathmatch:
                        {
                            Game.Components.Add(new Loader(global_vars.loadType.Campaign, ref variables, ref game));
                            //Avaliable on Xbox.  Fill in Campaign for PC
                        } break;
                    case option.upgrade:
                        {
                            Game.Components.Add(new Loader(global_vars.loadType.Upgrade, ref variables, ref game));
                        } break;
                    case option.credits:
                        {
                            Game.Components.Add(new Loader(global_vars.loadType.Credits, ref variables, ref game));
                        } break;
                }
                this.Dispose(true);
            }
        }

        public void NextOption()
        {
            switch (optionselected)
            {
                case option.campaign:
                    {
                        optionselected = option.deathmatch;
                        return;
                    }
                case option.deathmatch:
                    {
                        optionselected = option.upgrade;
                        return;
                    }
                case option.upgrade:
                    {
                        optionselected = option.credits;
                        return;
                    }
                case option.credits:
                    {
                        optionselected = option.campaign;
                        return;
                    }
            }
        }

        public void PreviousOption()
        {
                switch (optionselected)
                {
                    case option.campaign:
                        {
                            optionselected = option.credits;
                            return;
                        }
                    case option.deathmatch:
                        {
                            optionselected = option.campaign;
                            return;
                        }
                    case option.upgrade:
                        {
                            optionselected = option.deathmatch;
                            return;
                        }
                    case option.credits:
                        {
                            optionselected = option.upgrade;
                            return;
                        }
                }
        }

        public override void Update(GameTime gameTime)
        {
            if (current_delay == 0)
            {
                ControllerInput.handleMenuCommands(thisMain);
                current_delay = selection_delay;
            }
            current_delay--;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D selectedmenu = menu;

            switch (optionselected)
            {
                case option.campaign: selectedmenu = menu; break;
                case option.deathmatch: selectedmenu = menu2; break;
                case option.upgrade: selectedmenu = menu3; break;
                case option.credits: selectedmenu = menu4; break;
            }

            menubatch.Begin();
            menubatch.Draw(selectedmenu, new Rectangle(0, 0, variables.screen_width, variables.screen_height), Color.White);
            menubatch.End();

            if (!deviceSelected && !Guide.IsVisible)
                pickDevice();

            base.Draw(gameTime);
        }

        protected void pickDevice()
        {
            try
            {
                variables.storage = StorageDevice.BeginShowSelector(storageCallback, null);
            }
            catch (Exception) { }
        }

        public void storageCallback(IAsyncResult result)
        {
            if(result == 
            deviceSelected = true;
        }

    }
}
