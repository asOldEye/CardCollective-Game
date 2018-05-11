using System.Collections.Generic;
using AuxiliaryLibrary;
using Chat;

namespace CardCollectiveServerSide
{
    public class Player : ChatOwner
    {
        public List<CardCollectiveSession.Session> Sessions { get; } = new List<CardCollectiveSession.Session>();

        public Player(PlayerInfo ownerInfo,
            ParametrizedEventHandler<Chat.Chat, Entry> onNewMessage,
            ParametrizedEventHandler<Chat.Chat, IChatOwnerInfo> onOwnerJoined,
            ParametrizedEventHandler<Chat.Chat, IChatOwnerInfo> onOwnerLeft)
            : base(ownerInfo, onNewMessage, onOwnerJoined, onOwnerLeft)
        {

        }
    }
}