using System.Collections.Generic;
using AuxiliaryLibrary;
using Chat;

namespace CardCollectiveEnvironment
{
    public class Player : ChatOwner
    {
        public List<Session.Session> Sessions { get; } = new List<Session.Session>();

        public Player(PlayerInfo ownerInfo,
            ParametrizedEventHandler<Chat.Chat, Entry> onNewMessage,
            ParametrizedEventHandler<Chat.Chat, IChatOwnerInfo> onOwnerJoined,
            ParametrizedEventHandler<Chat.Chat, IChatOwnerInfo> onOwnerLeft)
            : base(ownerInfo, onNewMessage, onOwnerJoined, onOwnerLeft)
        {

        }
    }
}