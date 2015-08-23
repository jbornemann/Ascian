using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AscianXbox
{
    static class Ability_Manager
    {

        static String[] ability;
        static int[] ability_cost;

        public static void setAbilities(String[] abil, int[] costs)
        {
            ability = abil;
            ability_cost = costs;
        }

        public static String getAbilityName(int number)
        {
            return ability[number];
        }

        public static bool abilityIsAvaliable(int ability_number, ref Player player, ref global_vars vars)
        {
            return player.ABILITY[ability_number-1] != vars.NO_ABILITY ? true : false;
        }

        public static int getAbilityAmount(int ability_number)
        {
            return ability_cost[ability_number-1];
        }

        public static void performSpecialAbility(String identifier, ref global_vars vars, ref Game1 game)
        {
            //Account for null (AKA No Special Ability)
            if (identifier == null)
                return;
            //Fire
            else if(identifier.Equals(vars.ABIL_ONE))
            {
                FireWave f = new FireWave(5000, new Rectangle(0, 0, 50, vars.screen_height), 'r', global_vars.sender.Special, ref vars, ref game);
                game.Components.Add(f);
            }
            //Ice
            else if(identifier.Equals(vars.ABIL_TWO))
            {
                IceAttack ia = new IceAttack(ref vars, ref game);
                game.Components.Add(ia);
            }
            //Smoke
            else if(identifier.Equals(vars.ABIL_THREE))
            {
                SmokeAttack ms = new SmokeAttack(ref vars, ref game);
                game.Components.Add(ms);
            }   
        }

    }
}
