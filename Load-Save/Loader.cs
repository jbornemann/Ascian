using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Ascian
{
    class Loader : DrawableGameComponent
    {
        Game1 game;
        
        global_vars variables;
        Texture2D loadingsplash;
        SpriteBatch loadingbatch;
        global_vars.loadType loadtype;
        bool loading;

        public Loader(global_vars.loadType loadtype, ref global_vars vars, ref Game1 game) : base(game)
        {
            this.loadtype = loadtype;
            variables = vars;
            this.game = game;
        }

        void loadAbilityManager()
        {
            String[] possible = { variables.ABIL_ONE, variables.ABIL_TWO, variables.ABIL_THREE };
            int[] costs = { variables.ONE_COST, variables.TWO_COST, variables.THREE_COST };
            //Set Possible Abilities with Ability Manager
            Ability_Manager.setAbilities(possible, costs);
        }

        Player loadPlayer()
        {
            string noAbility = variables.NO_ABILITY;
            string selectedAbility = noAbility;
            int players_experience;
            int players_credit;
            String[] specialAbilities = { noAbility, noAbility, noAbility };
            loadAbilityManager();

            try
             {
             //Return a saved player, or if it does not exist, create a new one
               if (File.Exists("save.sav") == true)
                {
                   String[] savedgame = File.ReadAllLines("save.sav");
                   players_experience = Convert.ToInt32(savedgame[0]);
                   players_credit = Convert.ToInt32(savedgame[1]);
                   specialAbilities = parseAbilities(savedgame[2]);
                   selectedAbility = savedgame[3];
                   return new Player(ref variables, players_experience, specialAbilities, selectedAbility, players_credit, ref game);
                 }
              }
            catch (Exception) { Console.WriteLine("No saved game file loaded.  New game will be created"); }

             return new Player(ref variables, 0, specialAbilities, selectedAbility, 0, ref game);
        }

        String[] parseAbilities(String input)
        {
            String[] result = input.Split(' ');
            return result;
        }

        Campaign loadCampaign()
        {
            Texture2D map = Game.Content.Load<Texture2D>("Arenas\\arena");
            Player player = loadPlayer();
            return new Campaign(ref variables, player, map, game);
        }

        Upgrade loadUpgrade()
        {
            //Create Player
            Player player = loadPlayer();

            //Create a container to keep all the texture layers
            Dictionary<String, Texture2D[]> texturePack = new Dictionary<String, Texture2D[]>();

            //Each Array contains the different textures for each ability
            Texture2D[] one = new Texture2D[4];
            Texture2D[] two = new Texture2D[4];
            Texture2D[] three = new Texture2D[4];

            Texture2D DefaultSelection;
            Texture2D overlay;

            //greyed, highlighted, normal, selected(Character)
            
            //Ability One
            one[0] = Game.Content.Load<Texture2D>("Upgrade\\FireUU");
            one[1] = Game.Content.Load<Texture2D>("Upgrade\\FirePS");
            one[2] = Game.Content.Load<Texture2D>("Upgrade\\FirePU");
            one[3] = Game.Content.Load<Texture2D>("Upgrade\\FireChar");

            //Ability Two
            two[0] = Game.Content.Load<Texture2D>("Upgrade\\IceUU");
            two[1] = Game.Content.Load<Texture2D>("Upgrade\\IcePS");
            two[2] = Game.Content.Load<Texture2D>("Upgrade\\IcePU");
            two[3] = Game.Content.Load<Texture2D>("Upgrade\\IceChar");

            //Ability Three
            three[0] = Game.Content.Load<Texture2D>("Upgrade\\GasUU");
            three[1] = Game.Content.Load<Texture2D>("Upgrade\\GasPS");
            three[2] = Game.Content.Load<Texture2D>("Upgrade\\GasPU");
            three[3] = Game.Content.Load<Texture2D>("Upgrade\\GasChar");

            //Other textures
            DefaultSelection = Game.Content.Load<Texture2D>("Upgrade\\NormalChar");
            overlay = Game.Content.Load<Texture2D>("Upgrade\\BaseLayer");

            texturePack.Add("one", one);
            texturePack.Add("two", two);
            texturePack.Add("three", three);

            return new Upgrade(texturePack, overlay, DefaultSelection, player, ref variables, ref game);
        }

        Credits loadCredits()
        {
            return new Credits(ref variables, ref game);
        }

        public override void Initialize()
        {
            loadingbatch = new SpriteBatch(variables.manager.GraphicsDevice);
            loading = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            loadingsplash = game.Content.Load<Texture2D>("GamePlay\\Loading");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(loading == true)     //First initial call, load must not happen;  Splash screen should be displayed first
            {
                switch (loadtype)
                {
                    case global_vars.loadType.Campaign:
                        Campaign c = loadCampaign();
                        this.Dispose(true);
                        game.Components.Add(c); break;
                    case global_vars.loadType.Main:
                        Main_Menu m = new Main_Menu(variables, game);
                        this.Dispose(true);
                        game.Components.Add(m);break;
                    case global_vars.loadType.Upgrade:
                        Upgrade u = loadUpgrade();
                        this.Dispose(true);
                        game.Components.Add(u); break;
                    case global_vars.loadType.Credits:
                        Credits cr = loadCredits();
                        this.Dispose(true);
                        game.Components.Add(cr);break;
                    default :
                        Campaign ca = loadCampaign();
                        this.Dispose(true);
                        game.Components.Add(ca);break;
                }
            }
            loading = true;         //Allow for next cycle to load
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            loadingbatch.Begin();
            loadingbatch.Draw(loadingsplash, new Rectangle(0, 0, variables.screen_width, variables.screen_height), Color.White);
            loadingbatch.End();
            base.Draw(gameTime);
        }

    }
}
