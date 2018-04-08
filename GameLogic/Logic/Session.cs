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
        
        //список игроков
        List<Player> players;

        /// <summary>
        /// Текущий ход
        /// </summary>
        public int Turn { get; protected set; } = 1;
        public bool IsPlay { get; protected set; } = false;

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
        
        public int[,] SoliderClassesAttackMatrix;

        public bool Move()
        {
            return false;
        }
    }
}