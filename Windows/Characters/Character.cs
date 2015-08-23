using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Media;

namespace AscianXbox
{
    public class Character : DrawableGameComponent
    {
        protected Game1 game;
        protected Playable playable;
        //Used to pass pointers of this object
        protected Character thisCharacter;

        //Character Variables
        protected String special_ability;
        protected Rectangle position;
        public global_vars.movestate move_state;
        public bool shieldOut = false;
        protected Timer attackDelayTimer;
        public bool attackReady = false;
     
        //Graphical Variables
        protected SpriteBatch charsprite;       //The collection of sprites to be drawn -- Just 1
                                                
        protected Texture2D character_default;  //<-- Sprite Variables   
        protected Texture2D character_right;    //<--
        protected Texture2D character_left;     //<--
        protected Texture2D character_down;     //<--
        protected Texture2D character_up;       //<--

        //Other Control Variables
        protected global_vars variables;
        
        public Character(String special_ability, ref global_vars vars, ref Game1 game) : base(game)
        {
            this.game = game;
            thisCharacter = this;
            this.variables = vars;

            this.special_ability = special_ability;
            this.position = new Rectangle(variables.CHARACTER_START_POSITION.X, variables.CHARACTER_START_POSITION.Y, variables.CHARACTER_WIDTH, variables.CHARACTER_HEIGHT);

            base.UpdateOrder = variables.CHARACTER_UPDATE_PRIORITY;
            base.DrawOrder = variables.CHARACTER_UPDATE_PRIORITY;

            this.move_state = variables.CHARACTER_DEFAULT_MOVESTATE;
            this.charsprite = new SpriteBatch(variables.manager.GraphicsDevice);

            attackDelayTimer = new Timer(variables.ATTACK_DELAY);
            attackDelayTimer.Elapsed += new ElapsedEventHandler(attackIsReady);
        }

        public override void Initialize()
        {
            Collision_Detector.addCharacter(ref thisCharacter);
            attackDelayTimer.Start();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            switch(special_ability)
            {
                default :
                    character_default = Game.Content.Load<Texture2D>("Player_Characters\\Grey_Center");
                    character_right = Game.Content.Load<Texture2D>("Player_Characters\\Grey_Right");
                    character_left = Game.Content.Load<Texture2D>("Player_Characters\\Grey_Left");
                    character_down = Game.Content.Load<Texture2D>("Player_Characters\\Grey_Down");
                    character_up = Game.Content.Load<Texture2D>("Player_Characters\\Grey_Up");
                    break;
                case "fire" :
                    character_default = Game.Content.Load<Texture2D>("Player_Characters\\Fire_Center");
                    character_right = Game.Content.Load<Texture2D>("Player_Characters\\Fire_Right");
                    character_left = Game.Content.Load<Texture2D>("Player_Characters\\Fire_Left");
                    character_down = Game.Content.Load<Texture2D>("Player_Characters\\Fire_Down");
                    character_up = Game.Content.Load<Texture2D>("Player_Characters\\Fire_Up");
                    break;
                case "ice" :
                    character_default = Game.Content.Load<Texture2D>("Player_Characters\\Ice_Center");
                    character_right = Game.Content.Load<Texture2D>("Player_Characters\\Ice_Right");
                    character_left = Game.Content.Load<Texture2D>("Player_Characters\\Ice_Left");
                    character_down = Game.Content.Load<Texture2D>("Player_Characters\\Ice_Down");
                    character_up = Game.Content.Load<Texture2D>("Player_Characters\\Ice_Up");
                    break;
                case "smoke" :
                    character_default = Game.Content.Load<Texture2D>("Player_Characters\\Gas_Center");
                    character_right = Game.Content.Load<Texture2D>("Player_Characters\\Gas_Right");
                    character_left = Game.Content.Load<Texture2D>("Player_Characters\\Gas_Left");
                    character_down = Game.Content.Load<Texture2D>("Player_Characters\\Gas_Down");
                    character_up = Game.Content.Load<Texture2D>("Player_Characters\\Gas_Up");
                    break;
            }
            base.LoadContent();
        }

        public String getSpecial()
        {
            return special_ability;
        }

        public Rectangle getPosition()
        {
            return position;
        }

        public void setPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void setPlayable(Playable playable)
        {
            this.playable = playable;
        }

        protected void getUserInput()
        {
            ControllerInput.handleCommand(ref thisCharacter, ref playable, ref variables, ref game);
        }

        public override void Update(GameTime gameTime)
        {
            getUserInput();
 	        base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            charsprite.Begin();
            switch (move_state)
            {
                case global_vars.movestate.notmoving: charsprite.Draw(character_default, position, Color.White); break;
                case global_vars.movestate.up: charsprite.Draw(character_up, position, Color.White); break;
                case global_vars.movestate.down: charsprite.Draw(character_down, position, Color.White); break;
                case global_vars.movestate.left: charsprite.Draw(character_left, position, Color.White); break;
                case global_vars.movestate.right: charsprite.Draw(character_right, position, Color.White); break;
            }
            charsprite.End();

            base.Draw(gameTime);
        }

        public void shieldExpired()
        {
            shieldOut = false;
        }

        public void attackIsReady(object sender, ElapsedEventArgs e)
        {
            attackReady = true;
        }


        public void kill()
        {
            lock (this)
            {
                try
                {
                    this.Dispose(true);
                    playable.notify(global_vars.notification.PlayerDeath);
                }
                catch (Exception) { }
            }
        }
    }
}
