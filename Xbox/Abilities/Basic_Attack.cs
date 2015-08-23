using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Media;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace AscianXbox
{
    /**
     * The basic attack 
     **/

    class Basic_Attack : DrawableGameComponent, Spell
    {
        protected Timer active;
        protected bool activate;

        protected Game1 game;
        protected global_vars variables;
        protected int timeAvaliable;

        protected Texture2D attackSprite;
        protected SpriteBatch attackBatch;
        protected SoundEffect attackSound;

        protected Rectangle position;
        protected char direction;

        protected Spell thisAttack;  //Copy of this spell so it can be passed by ref
        protected global_vars.sender sender;  //Enemy or Character

        public Basic_Attack(int timeAvaliable, Rectangle startpos, char direction, global_vars.sender sender, ref global_vars variables, ref Game1 game) : base(game)
        {
            this.game = game;
            this.variables = variables;
            this.sender = sender;
            this.timeAvaliable = timeAvaliable;
            this.position = startpos;
            this.direction = direction;
            thisAttack = this;

            attackBatch = new SpriteBatch(variables.manager.GraphicsDevice);

            active = new Timer(timeAvaliable);
            active.Elapsed += new ElapsedEventHandler(spellTimeout);
            activate = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            addToCollisionDetector();
            active.Start();
        }

        protected virtual void addToCollisionDetector()
        {
            Collision_Detector.addSpell(ref thisAttack);
        }

        protected override void LoadContent()
        {
            loadAttackSpriteType();
            //doAttackSound();
            base.LoadContent();
        }

        protected virtual void loadAttackSpriteType()
        {
            attackSprite = game.Content.Load<Texture2D>("Ability\\BasicAttack");
        }

        protected virtual void doAttackSound()
        {
            attackSound = game.Content.Load<SoundEffect>("Sounds\\CharacterShot");
            attackSound.Play();
        }

        protected virtual void attackUpdate()
        {
            switch (direction)
            {
                case 'u': position.Y -= variables.ATTACK_SPELL_SPEED; break;
                case 'd': position.Y += variables.ATTACK_SPELL_SPEED; break;
                case 'l': position.X -= variables.ATTACK_SPELL_SPEED; break;
                case 'r': position.X += variables.ATTACK_SPELL_SPEED; break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            attackUpdate();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (activate == true)
            {
                attackBatch.Begin();
                attackBatch.Draw(attackSprite, position, Color.White);
                attackBatch.End();
            }
            else
                kill();
            base.Draw(gameTime);
        }

        public void spellTimeout(object sender, ElapsedEventArgs e)
        {
            activate = false;
            active.Stop();
        }

        public Rectangle getPosition()
        {
            return position;
        }

        public virtual void kill()
        {
            this.Dispose(true);
        }

        public global_vars.sender getSender()
        {
            return sender;
        }
    }
}
