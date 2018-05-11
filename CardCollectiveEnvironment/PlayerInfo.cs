using System;
using System.Collections.Generic;
using Chat;

namespace CardCollectiveServerSide
{
    [Serializable]
    public class PlayerInfo : IChatOwnerInfo
    {
        public string Name { get; set; }

        public List<Card> Soliders { get; set; }
        public List<Card> Spells { get; set; }
    }
}