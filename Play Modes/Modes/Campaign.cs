using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Ascian
{
    public class Campaign : DrawableGameComponent, Playable
    {
        global_vars variables;
        Campaign thisCampaign;
        Playable play;
        Pause pauseScreen;
        Game1 game;

        AutoSave autosave;
        delegate void callback();

        Player player;          //Holds information about the player (EXP, points, etc)
        Character character;    //Graphical character owned by player
        int creepers;
        int shadows;

        Rectangle mapPosition;
        SpriteBatch mapBatch;
        Texture2D creeperSprite;
        Texture2D shadowSprite;
        Texture2D map;

        Timer shadowEaterStart;
        Timer spawnDelay;
        bool release = false;
        bool spawnShadows = false;

        int creeperConstant = 20;
        int shadowConstant = 3;

        Texture2D shieldIcon;
        //Icon Locations on the Screen
        Rectangle l1;
        Rectangle l2;
        Rectangle l3;
        SpriteFont font;
        //Special Icon on the Screen
        Vector2 s1;
        int shieldsAvaliable = 3;
        bool specialAvaliable = true;
        Timer specTimer;

        bool respawn = false;

        public Campaign(ref global_vars vars, Player player, Texture2D map, Game1 game) : base(game)
        {
            this.variables = vars;
            this.player = player;
            this.map = map;
            this.game = game;
            thisCampaign = this;
            play = this;

            s1 = new Vector2(40, variables.screen_height - 40);
            l1 = new Rectangle(variables.screen_width - 190, variables.screen_height - 70, 50, 50);
            l2 = new Rectangle(variables.screen_width - 130, variables.screen_height - 70, 50, 50);
            l3 = new Rectangle(variables.screen_width - 70, variables.screen_height - 70, 50, 50);

            mapPosition = new Rectangle(0, 0, variables.screen_width, variables.screen_height);
            this.character = this.player.getCharacter(thisCampaign);
            creepers = 0;
            shadows = 0;
        }

        public override void Initialize()
        {
            //Stop theme music
            game.stopTheme();

            mapBatch = new SpriteBatch(variables.manager.GraphicsDevice);
            //Add character to map
            game.Components.Add(character);
            //Add pause screen component
            pauseScreen = new Pause(ref variables, ref play, ref game);
            game.Components.Add(pauseScreen);

            //Start releasing Shadow eaters after 2 minutes
            shadowEaterStart = new Timer(variables.SHADOW_START);
            shadowEaterStart.Elapsed += new ElapsedEventHandler(startShadow);
            shadowEaterStart.Start();

            //Set Up special timer
            specTimer = new Timer(60000);
            specTimer.Elapsed += new ElapsedEventHandler(ActivateSpecial);
            //Set Up autosave
            autosave = new AutoSave(new callback(TimeToSave), 15000);

            //Set up spawn delay thread
            spawnDelay = new Timer(250);
            spawnDelay.Elapsed += new ElapsedEventHandler(setRelease);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            creeperSprite = game.Content.Load<Texture2D>("Enemy\\Creeper");
            shadowSprite = game.Content.Load<Texture2D>("Enemy\\Skully");
            //Load Icons
            font = game.Content.Load<SpriteFont>("Fonts//MyFont");
            shieldIcon = game.Content.Load<Texture2D>("Ability\\shield");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            checkEnemyCountOk();
            Collision_Detector.checkCollisions();
            if (respawn)
                respawnPlayer();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mapBatch.Begin();
            mapBatch.Draw(map, mapPosition, Color.White);
            if (specialAvaliable && !player.getCurrentSpecial().Equals(variables.NO_ABILITY))
                mapBatch.DrawString(font, "SPECIAL", s1, Color.LightYellow);
            
            switch (shieldsAvaliable)
            {
                case 3: mapBatch.Draw(shieldIcon, l3, Color.White);
                        mapBatch.Draw(shieldIcon, l2, Color.White);
                        mapBatch.Draw(shieldIcon, l1, Color.White);break;
                case 2: mapBatch.Draw(shieldIcon, l2, Color.White);
                        mapBatch.Draw(shieldIcon, l1, Color.White);break;
                case 1: mapBatch.Draw(shieldIcon, l1, Color.White);break;
            }
            mapBatch.End();
            base.Draw(gameTime);
        }

        protected void checkEnemyCountOk()
        {
            if (creepers < creeperConstant)
                spawnCreeper();
            if (spawnShadows)
            {
               while (shadows < shadowConstant)
               {
                   spawnShadow();
               }
            }
        }

        public void killEnemy(string Type)
        {
            if(Type.Equals("Ascian.Creeper"))
                killCreeper();
            else if(Type.Equals("Ascian.ShadowEater"))
                killShadow();
        }

        void killCreeper()
        {
            creepers--;
            updatePlayerStats();
        }

        void killShadow()
        {
            shadows--;
            updatePlayerStats();
        }

        void spawnCreeper()
        {
            if (!spawnDelay.Enabled)
                spawnDelay.Start();
            else if (release == true)
            {
                releaseCreeper();
                release = false;
            }
        }

        public void setRelease(object sender, ElapsedEventArgs e)
        {
            release = true;
            spawnDelay.Stop();
        }

        void releaseCreeper()
        {
                Enemy newCreep = new Creeper(ref variables, creeperSprite, ref character, ref thisCampaign, game);
                creepers++;
                game.Components.Add(newCreep);
        }

        void spawnShadow()
        {
            Enemy newShad = new ShadowEater(ref variables, shadowSprite, ref character, ref thisCampaign, game);
            shadows++;
            game.Components.Add(newShad);
        }

        void updatePlayerStats()
        {
            //Double EXP when shadows arrive
            if (spawnShadows)
                player.incExperience(2);
            else
                player.incExperience(1);
        }

        public void quit()
        {
            //Wait for any autosave thread to finish (if exists) before continuing
            try
            {
                autosave.GetAutoSaveThread().Join();
            }
            catch (Exception) {}
            Saver.saveGame(player.EXP, player.CREDIT, player.ABILITY, player.getCurrentSpecial());
            Loader l = new Loader(global_vars.loadType.Main, ref variables, ref game);
            //Delete Timers
            specTimer.Dispose();
            shadowEaterStart.Dispose();
            spawnDelay.Dispose();
            autosave.delete();
            //Remove Objects from 
            Collision_Detector.removeAll();
            game.Components.Clear();
            game.Components.Add(l);
        }

        void startShadow(object sender, ElapsedEventArgs e)
        {
            shadowEaterStart.Stop();
            spawnShadows = true;
        }

        public String[] getStats()
        {
            int exp = player.EXP;
            int credit = player.CREDIT;
            String[] stat = { exp.ToString(), credit.ToString() };
            return stat;
        }

        public bool notify(global_vars.notification request)
        {
            if (request.Equals(global_vars.notification.ShieldRequest))
            {
                if (shieldsAvaliable == 0)
                    return false;
                else
                    shieldsAvaliable--;
            }
            else if (request.Equals(global_vars.notification.SpecialRequest))
            {
                if (!specialAvaliable)
                    return false;
                else
                {
                    specialAvaliable = false;
                    specTimer.Start();
                }
            }
            else if(request.Equals(global_vars.notification.PlayerDeath))
            {
                playerDeath();
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void playerDeath()
        {
                this.character = this.player.getNewCharacter(thisCampaign);
                //Block the pause screen
                pauseScreen.pauseBlock();
                //Clear the screen with a firewave
                game.Components.Add(new FireWave(5000, new Rectangle(-100, 0, 50, variables.screen_height), 'r', global_vars.sender.Special, ref variables, ref game));
                //Give back all specials and sheilds
                specialAvaliable = true;
                shieldsAvaliable = 3;
                //Don't allow enemies to spawn during respawn period
                creeperConstant = 0;
                //No shadows
                shadowEaterStart.Stop();
                spawnShadows = false;
                //Respawn delay
                Timer t = new Timer(3000);
                t.Elapsed += new ElapsedEventHandler(setRespawn);
                t.AutoReset = false;
                t.Start();
        }

        public void setRespawn(object sender, ElapsedEventArgs e)
        {
            respawn = true;
        }

        public void respawnPlayer()
        {
            try
            {
                game.Components.Add(character);
            }
            catch (Exception) { }
            //Start spawning enemies again
            creeperConstant = 20;
            //Restart shadow timer
            shadowEaterStart.Start();
            respawn = false;
            pauseScreen.pauseUnblock();
        }

        public void TimeToSave()
        {
            autosave.createSaveThread(player.EXP, player.CREDIT, player.ABILITY, player.getCurrentSpecial());
        }

        public void ActivateSpecial(object sender, ElapsedEventArgs e)
        {
            specialAvaliable = true;
            specTimer.Stop();
        }
    }
}
