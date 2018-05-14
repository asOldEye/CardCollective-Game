using System;
using System.Collections.Generic;
using AuxiliaryLibrary;
using System.Threading.Tasks;

namespace CardCollectiveServerSide
{
    public class Supervisor
    {
        TimeSpan retentionAwait;
        List<Pair<Player, DateTime>> retentionPlayers = new List<Pair<Player, DateTime>>();
        List<Player> onlinePlayers = new List<Player>();
        Task retention = Task.Delay(0);

        void StartRetentionTask()
        {
            if (retention.Status == TaskStatus.RanToCompletion)
                retention = new Task(new Action(RetentionWork));
        }
        void RetentionWork()
        {
            while (retentionPlayers.Count > 0)
            {
                var time = DateTime.Now;
                foreach (var f in retentionPlayers.ToArray())
                    if (time - f.Obj2 > retentionAwait)
                    {
                        lock (retentionPlayers)
                            retentionPlayers.Remove(f);
                        lock (onlinePlayers)
                            onlinePlayers.Remove(f.Obj1);
                    }
                Task.Delay(1000).Wait();
            }
        }

        public Supervisor(TimeSpan retentionAwait)
        {
            if ((this.retentionAwait = retentionAwait) < new TimeSpan(0, 0, 10))
                throw new ArgumentException("Wrong retention await itme, must be more than 10 sec");
        }

        public void Disconnect(Player player, bool correctly)
        {
            if(correctly)
            {
                lock (onlinePlayers)
                    onlinePlayers.Remove(player);
            }
        }

        public Player GetPlayer(PlayerInfo playerInfo)
        {
            var pair = retentionPlayers.Find(new Predicate<Pair<Player, DateTime>>(q => q.Obj1.Info == playerInfo));
            Player player = null;
            if (pair != null)
            {
                lock (retentionPlayers)
                    retentionPlayers.Remove(pair);
                player = pair.Obj1;
            }
            else
            {
                var onl = onlinePlayers.Find(new Predicate<Player>(q => q.Info == playerInfo));
                if (onl == null)
                {
                    player = new Player(playerInfo);
                    lock (onlinePlayers)
                    {
                        onlinePlayers.Add(player);
                        //if (onlinePlayers.Count == 1)
                    }
                }
            }
            return player;
        }
    }
}