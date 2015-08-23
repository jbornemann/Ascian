using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;
using System.Threading;

namespace Ascian
{
    class AutoSave
    {
        Thread thread;
        System.Timers.Timer timer;
        Delegate notify;

        public AutoSave(Delegate notify, double saveTime)
        {
            this.notify = notify;
            timer = new System.Timers.Timer(saveTime);
            timer.Elapsed += new ElapsedEventHandler(SaveDue);
            timer.Start();
        }

        public void createSaveThread(int playerExperience, int playerCredit, string[] abilites, string selectedAbility)
        {
            thread = new Thread(() => autosave(playerExperience, playerCredit, abilites, selectedAbility));
            thread.Name = "Autosave";
            thread.Start();
        }

        void autosave(int playerExperience, int playerCredit, string[] abilites, string selectedAbility)
        {
            lock (this)
            {
                Saver.saveGame(playerExperience, playerCredit, abilites, selectedAbility);
            }
        }

        public void SaveDue(object sender, ElapsedEventArgs e)
        {
            try
            {
                notify.DynamicInvoke();
                Console.WriteLine("AUTOSAVED");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Autosave failed");
                Console.WriteLine(ex);
            }
        }

        public Thread GetAutoSaveThread()
        {
            return thread;
        }

        public void delete()
        {
            timer.Dispose();
        }
    }
}
