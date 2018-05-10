using AuxiliaryLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    /// <summary>
    /// Игровая сессия
    /// </summary>
    public class Session
    {
        internal static readonly DistributionRandom Random = new DistributionRandom();

        List<FreePair<Player, TurnStatus>> players = new List<FreePair<Player, TurnStatus>>();
        List<SessionObject> sessionObjects = new List<SessionObject>();

        public Session(SoliderClassesOpitions classesOpitions,
            Position mapSize, Pair<Modifier, int>[] possibleModifiersRarity = null)
        {
            Map = new Map(mapSize, possibleModifiersRarity);
            ClassesOpitions = classesOpitions;
        }

        public void AddPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException("Null player");
            if (Playing) throw new NotSupportedException("Can't add player while playing");

            players.Add(new FreePair<Player, TurnStatus>(player, TurnStatus.Expected));
        }

        public void AddSessionObject(SessionObject obj)
        {
            if (obj == null) throw new ArgumentNullException("Null session object");
            if (Exists(obj)) throw new ArgumentException("Already exists in this session");

            sessionObjects.Add(obj);
            if (obj is Player)
                (obj as Player).OnDeath += PlayerDeath;
        }

        public List<SessionObject> Find<T>() where T : SessionObject
        { return sessionObjects.FindAll(new Predicate<SessionObject>(f => f.GetType() == typeof(T))); }

        public bool Exists<T>(T obj) where T : SessionObject
        { return sessionObjects.Exists(new Predicate<SessionObject>(f => f.GetType() == typeof(T) && f == obj)); }

        void PlayerDeath(IDestroyable player)
        {
            var f = player as Player;
            players.Remove(players.Find(new Predicate<FreePair<Player, TurnStatus>>(q => q.Obj1 == f)));

            foreach (var q in sessionObjects)
                if (q.Owner == f)
                    q.Destroy();
        }

        public Player master;

        void Start()
        {
            Playing = true;

            master = players[Random.Next(0, players.Count)].Obj1;
        }
        void Finish()
        {

        }
        void TurnEnds()
        {

        }
        void RefreshTurn()
        {

        }

        public int Turn { get; private set; } = 1;
        public bool Playing { get; private set; } = false;

        public Map Map { get; }
        public SoliderClassesOpitions ClassesOpitions { get; }

        public event NonParametrizedEventHandler<Player> OnTurn;
        public event NonParametrizedEventHandler<Session> OnFinish;
        public event NonParametrizedEventHandler<Session> OnStart;

        public void MakeMove(Player onBehalfOf,
            SessionObject subject, string methodName, SessionObject target)
        {
            try
            {
                subject.GetType().GetMethod(methodName).
                  Invoke(subject, new object[] { target });
            }
            catch { throw; }
        }

    }

    public enum TurnStatus
    { Did, Expected, Abstained }
}