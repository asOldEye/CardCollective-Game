using System.Collections.Generic;
using AuxiliaryLibrary;
using Chat;
using System;

namespace CardCollectiveServerSide
{
    [Serializable]
    public class Player : ChatOwner
    {
        public PlayerInfo PlayerInfo { get; }
        public List<CardCollectiveSession.Session> Sessions { get; } = new List<CardCollectiveSession.Session>();

        public Player(PlayerInfo ownerInfo) : base(ownerInfo.SharedPlayerInfo)
        {
            if (ownerInfo == null) throw new ArgumentNullException(nameof(ownerInfo));
        }
    }
}