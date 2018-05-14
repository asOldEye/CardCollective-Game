using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuxiliaryLibrary;
using CardCollectiveSession;

namespace CardCollectiveServerSide
{
    public class InGameAPI : API
    {
        Player player;
        Supervisor supervisor;

        public InGameAPI(Player player, Supervisor supervisor)
        {
            InitializeAPICommands(new Dictionary<string, Action<APICommand>>()
            {
                {"Unlogin", new Action<APICommand>(Unlogin) },
                {"LoadSessions", new Action<APICommand>(LoadSessions) },
                {"LoadChats", new Action<APICommand>(LoadChats) }
            });

            if ((this.player = player) == null) throw new ArgumentNullException(nameof(player));
            if ((this.supervisor = supervisor) == null) throw new ArgumentNullException(nameof(supervisor));

            player.OnNewMessage = OnNewMessage;
            player.OnOwnerJoined = OnOwnerJoined;
            player.OnOwnerLeft = OnOwnerLeft;

            if (player.Sessions.Count > 0)
                LoadSessions(new APICommand("LoadSessions", new object[] { }));

            if (player.Chats.Count > 0)
                LoadChats(new APICommand("LoadChats", new object[] { }));
        }

        void OnNewMessage(Chat.Chat chat, Chat.Entry entry)
        {

        }
        void OnOwnerJoined(Chat.Chat chat, Chat.IChatOwnerInfo owner)
        {

        }
        void OnOwnerLeft(Chat.Chat chat, Chat.IChatOwnerInfo owner)
        {
            if (chat == null) throw new ArgumentNullException(nameof(chat));
        }

        public override void OnDisconnected()
        {
            throw new NotImplementedException();
        }

        #region API commands
        [APICommandAttr]
        void Unlogin(APICommand command)
        {

        }

        void StartSession(APICommand command)
        {

        }
        
        void MakeMove(APICommand command)
        {

        }
        [APICommandAttr(new Type[] { typeof(Session) })]
        void LeaveSession(APICommand command)
        {

        }
        [APICommandAttr]
        void LoadSessions(APICommand command)
        {

        }
        [APICommandAttr(new Type[] { typeof(string) })]
        void LoadPlayerInfo(APICommand command)
        {

        }





        void StartChat(APICommand command)
        {

        }
        [APICommandAttr(new Type[] { typeof(Chat.Chat), typeof(Chat.Entry) })]
        void MakeMessageInChat(APICommand command)
        {

        }
        [APICommandAttr(new Type[] { typeof(Chat.Chat) })]
        void LeaveChat(APICommand command)
        {

        }
        [APICommandAttr]
        void LoadChats(APICommand command)
        {

        }
        #endregion
    }
}