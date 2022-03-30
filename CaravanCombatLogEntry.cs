using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Caravanalyzer
{
    internal struct CaravanCombatLogEntry
    {
        public string name;
        //public CaravanCombatResult result;
        public BattleSideEnum winningSide;
        //public int troopsLost;
        public Vec2 location;
    }
}
