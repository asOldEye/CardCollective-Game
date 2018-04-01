using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// Игровая сессия
    /// </summary>
    public abstract class Session
    {
        /// <summary>
        /// Экземпляр рандома
        /// </summary>
        internal static readonly Random random = new Random();

        //TODO наследовать рандом

        //список игроков
        List<Pair<Player, Map>> players;

        int turn = 1;
        /// <summary>
        /// Текущий ход
        /// </summary>
        public int Turn
        { get { return turn; } }

        readonly Map map;

        public static int ProbabilisticRandom()
        {
            return 0;
        }

        public void AddPlayer(Player player)
        {

        }
        public void DelPlayer(Player player)
        {

        }

        readonly List<InGameEvent> events;


        internal void AddEvent()
        {

        }

        //TODO урон от класса к классу

        public int[,] SoliderClassesAttackMatrix;

        public void Serialize()
        {
            //TODO
        }

        public Player hisTurn;

        internal SessionObject Find()
        {
            return null;
        }

        public bool Move()
        {
            return false;
        }
    }
}