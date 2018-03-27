using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public abstract class Session
    {
        List<KeyValuePair<Player, IPositionable[]>> players;

        public void AddPlayer(Player player)
        {

        }
        public void DelPlayer(Player player)
        {

        }

        int turn = 1;
        public int Turn
        { get { return turn; } }

        Position mapSize;

        public Player hisTurn;

        internal static readonly Random random = new Random();
    }
}