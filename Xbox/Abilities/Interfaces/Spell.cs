using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AscianXbox
{
    public interface Spell : Ability
    {
        Rectangle getPosition();
        global_vars.sender getSender();
        void kill();
    }
}
