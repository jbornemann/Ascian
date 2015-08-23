using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AscianXbox
{
    public class Upgrade : DrawableGameComponent
    {
        Game1 game;
        Upgrade thisUpgrade;
        Dictionary<String, Texture2D[]> texturePack;
        Player player;
        global_vars variables;
        SpriteBatch textureBatch;
        Rectangle layoverPosition;

        Timer selectionDelay;
        bool selectReady = false;

        Texture2D defaultSelection;
        Texture2D overlay;
        Texture2D selection;
        Texture2D slotOne;
        Texture2D slotTwo;
        Texture2D slotThree;

        List<abilityState> ability;

        int currentSlot;

        //For cost and current points display
        SpriteFont font;
        Vector2 pointLoc;
        Vector2 costLoc;
        String purchased = "Purchased";
        String points = "";
        String cost = "";

        public Upgrade(Dictionary<String, Texture2D[]> texPack, Texture2D overlay, Texture2D defaultSelection, Player player, ref global_vars vars, ref Game1 game) : base(game)
        {
            this.currentSlot = 0;
            this.texturePack = texPack;
            this.overlay = overlay;
            this.player = player;
            this.variables = vars;
            this.game = game;
            this.defaultSelection = defaultSelection;
            this.selection = defaultSelection;
            thisUpgrade = this;

            pointLoc = variables.PLOC;
            costLoc = variables.CLOC;
      
            layoverPosition = new Rectangle(0, 0, vars.screen_width, vars.screen_height);
            selectionDelay = new Timer(vars.UPGRADE_DELAY, game);
            selectionDelay.Elapsed = selectDelay;
        }

        public override void Initialize()
        {
            textureBatch = new SpriteBatch(variables.manager.GraphicsDevice);
            //Set Original Display Set Up
            setUp();

            selectionDelay.Start();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = game.Content.Load<SpriteFont>("Fonts\\MyFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            ControllerInput.handleUpgradeScreenCommands(ref thisUpgrade, ref variables);
            //Update Screen
            slotOne = ability.ElementAt(0).current;
            slotTwo = ability.ElementAt(1).current;
            slotThree = ability.ElementAt(2).current;

            //Cost / Point
            points = Convert.ToString(player.CREDIT);
            if (ability.ElementAt(currentSlot).bought)
                cost = purchased;
            else
                cost = Convert.ToString(Ability_Manager.getAbilityAmount(currentSlot+1));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            textureBatch.Begin();
            textureBatch.Draw(overlay, layoverPosition, Color.White);
            textureBatch.Draw(selection, layoverPosition, Color.White);
            textureBatch.Draw(slotOne, layoverPosition, Color.White);
            textureBatch.Draw(slotTwo, layoverPosition, Color.White);
            textureBatch.Draw(slotThree, layoverPosition, Color.White);
            textureBatch.DrawString(font, points, pointLoc, Color.Black);
            textureBatch.DrawString(font, cost, costLoc, Color.Black);
            textureBatch.End();
            base.Draw(gameTime);
        }

        void setUp()
        {
            ability = new List<abilityState>(texturePack.Count);
            Texture2D[] curr;
            bool buyable;
            bool bought;
            bool selectable;


            //Create Ability States
            texturePack.TryGetValue("one", out curr);
            getAbilityParameter(1, out buyable, out bought, out selectable);
            ability.Add(new abilityState(curr[0], curr[1], curr[2], curr[3], selectable, buyable, bought));
    
            texturePack.TryGetValue("two", out curr);
            getAbilityParameter(2, out buyable, out bought, out selectable);
            ability.Add(new abilityState(curr[0], curr[1], curr[2], curr[3], selectable, buyable, bought));
            
            texturePack.TryGetValue("three", out curr);
            getAbilityParameter(3, out buyable, out bought, out selectable);
            ability.Add(new abilityState(curr[0], curr[1], curr[2], curr[3], selectable, buyable, bought));

            //Make slot one originally highlighted
            abilityState one = ability.ElementAt(0);
            one.current = one.highlighted;
            ability.RemoveAt(0);
            ability.Insert(0, one);

            //Assign selected special graphic
            switch (player.getCurrentSpecial())
            {
                case "fire": selection = ability.ElementAt(0).selected; break;
                case "ice": selection = ability.ElementAt(1).selected; break;
                case "smoke": selection = ability.ElementAt(2).selected; break;
            }

            //assign slots
            slotOne = ability.ElementAt(0).current;
            slotTwo = ability.ElementAt(1).current;
            slotThree = ability.ElementAt(2).current;

        }

        void getAbilityParameter(int ability_number, out bool buyable, out bool bought, out bool selectable)
        {
            bought = Ability_Manager.abilityIsAvaliable(ability_number, ref player, ref variables);
            if (bought == true)
                buyable = false;
            else if (Ability_Manager.getAbilityAmount(ability_number) > player.CREDIT)
                buyable = false;
            else
                buyable = true;
            selectable = bought ? true : false;
        }

        void selectDelay()
        {
            selectReady = true;
            selectionDelay.Stop();
        }

        public void setAbility()
        {
            if (selectReady)
            {
                if (ability.ElementAt(currentSlot).selectable)
                {
                    player.setAbility(currentSlot);
                    selection = ability.ElementAt(currentSlot).selected;

                    selectionDelay.Start();
                    selectReady = false;
                }
            }
        }

        public void buyAbility()
        {
            if (selectReady)
            {
                if (ability.ElementAt(currentSlot).buyable)
                {
                    int purchasePrice = Ability_Manager.getAbilityAmount(currentSlot+1);
                    //grant ability to player
                    String abil = Ability_Manager.getAbilityName(currentSlot);
                    player.grantAbility(abil, currentSlot);

                    //Update ability
                    abilityState ab = ability.ElementAt(currentSlot);
                    ab.buyable = false;
                    ab.bought = true;
                    ab.selectable = true;
                    ab.current = ab.normal;
                    ability.RemoveAt(currentSlot);
                    ability.Insert(currentSlot, ab);

                    //Deduct points
                    player.CREDIT = player.CREDIT - purchasePrice;

                    selectionDelay.Start();
                    selectReady = false;
                }
            }
        }

        public void moveAbilityRight()
        {
            if (selectReady)
            {
                int lastSlot;
                if (currentSlot == 2)
                {
                    currentSlot = 0;
                    lastSlot = 2;
                }
                else
                {
                    currentSlot++;
                    lastSlot = currentSlot - 1;
                }

                //Update last box 
                abilityState ab = ability.ElementAt(lastSlot);
                ab.current = ab.bought ? ab.normal : ab.greyed;
                ability.RemoveAt(lastSlot);
                ability.Insert(lastSlot, ab);

                //Update new box
                ab = ability.ElementAt(currentSlot);
                ab.current = ab.highlighted;
                ability.RemoveAt(currentSlot);
                ability.Insert(currentSlot, ab);

                selectionDelay.Start();
                selectReady = false;
            }
        }

        public void moveAbilityLeft()
        {
            if (selectReady)
            {
                int lastSlot;
                if (currentSlot == 0)
                {
                    currentSlot = 2;
                    lastSlot = 0;
                }
                else
                {
                    currentSlot--;
                    lastSlot = currentSlot + 1;
                }

                //Update last box 
                abilityState ab = ability.ElementAt(lastSlot);
                ab.current = ab.bought ? ab.normal : ab.greyed;
                ability.RemoveAt(lastSlot);
                ability.Insert(lastSlot, ab);

                //Update new box
                ab = ability.ElementAt(currentSlot);
                ab.current = ab.highlighted;
                ability.RemoveAt(currentSlot);
                ability.Insert(currentSlot, ab);

                selectionDelay.Start();
                selectReady = false;
            }
        }

        public void remove()
        {
            if (selectReady)
            {
                player.defaultAbility();
                selection = defaultSelection;
                selectionDelay.Start();
                selectReady = false;
            }
        }

        public void quit()
        {
            Saver.saveGame(player.EXP, player.CREDIT, player.ABILITY, player.getCurrentSpecial());
            Loader l = new Loader(global_vars.loadType.Main, ref variables, ref game);
            game.Components.Clear();
            game.Components.Add(l);
        }  

        struct abilityState
        {
            public abilityState(Texture2D greyed,
                                Texture2D highlighted,
                                Texture2D normal,
                                Texture2D selected,
                                bool selectable,
                                bool buyable,
                                bool bought)
            {
                this.greyed = greyed;
                this.highlighted = highlighted;
                this.normal = normal;
                this.selected = selected;
                this.selectable = selectable;
                this.buyable = buyable;
                this.bought = bought;

                this.current = bought ? normal : greyed;
            }

            public Texture2D current;

            public Texture2D greyed;
            public Texture2D highlighted;
            public Texture2D normal;
            public Texture2D selected;

            public bool selectable;
            public bool buyable;
            public bool bought;
            
        }

    }

  }

