using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AscianXbox
{
    public class Player
    {
        protected Character character;
        protected int experience;
        protected String[] avaliableSpecials; 
        protected String selectedSpecial;
        protected int game_credit;
        protected global_vars variables;
        protected Game1 game;

        public Player(ref global_vars vars, int experience, String[] special, String selectedSpecial, int game_credit, ref Game1 g)
        {
            this.experience = experience;
            this.avaliableSpecials = special;
            this.game_credit = game_credit;
            variables = vars;
            game = g;
            this.selectedSpecial = selectedSpecial;
            character = new Character(selectedSpecial, ref variables, ref g);
        }
        public Character getCharacter(Playable playable)
        {
            character.setPlayable(playable);
            return character;
        }

        public Character getCharacter()
        {
            return character;
        }

        public Character getNewCharacter(Playable playable)
        {
            Character c = new Character(selectedSpecial, ref variables, ref game);
            c.setPlayable(playable);
            character = c;
            return c;
        }

        public String getCurrentSpecial()
        {
            return selectedSpecial;
        }

        public void defaultAbility()
        {
            selectedSpecial = variables.NO_ABILITY;
        }

        public void setAbility(int num)
        {
            try
            {
                selectedSpecial = avaliableSpecials[num];
            }
            catch (Exception e) { throw e; }
            //Create a new character with new selected ability
            character = new Character(selectedSpecial, ref variables, ref game);
        }

        public void grantAbility(String identifier, int abilityNumber)
        {
            avaliableSpecials[abilityNumber] = identifier;
        }

        public int EXP
        {
            get
            {
                return experience;
            }
            set
            {
                experience = value;
            }
        }

        public string[] ABILITY
        {
            get
            {
                return avaliableSpecials;
            }
        }

        public int CREDIT
        {
            get
            {
                return game_credit;
            }
            set
            {
                game_credit = value;
            }
        }

        public void incExperience(int amount)
        {
            int oldexp = this.experience;
            this.experience = this.experience + amount;
            if (this.experience / 100 > oldexp / 100)
                incGameCredit();
        }

        int GameCredit
        {
            get
            {
                return game_credit;
            }
            set
            {
                game_credit = value;
            }
        }

        void incGameCredit()
        {
            game_credit++;
        }

    }
}
