using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Ascian
{
    public static class Saver
    {
        public static void saveGame(int playerExperience, int playerCredit, string[] abilites, string selectedAbility)
        {
            try
            {
                //If A saved game file already exists. Delete it and create a new one
                if (File.Exists("save.sav") == true)
                {
                    File.Delete("save.sav");
                }
                File.WriteAllText(
                        "save.sav",
                        playerExperience.ToString() +
                        System.Environment.NewLine +
                        playerCredit.ToString() +
                        System.Environment.NewLine +
                        formatAbilitiesToString(abilites) +
                        System.Environment.NewLine +
                        selectedAbility
                                    );
            }
            catch (Exception e) 
                {
                    Console.WriteLine(e);
                }
        }

        static String formatAbilitiesToString(String[] ab)
        {
            String result = "";
            foreach (String x in ab)
            {
                result += x + " ";
            }
            return result;
        }
    }
}
