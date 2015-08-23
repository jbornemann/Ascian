using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AscianXbox
{
    public interface Playable
    {
        void quit();
        String[] getStats();
        //Notify of action, response in form of boolean
        bool notify(global_vars.notification note);
    }
}
