using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat
{
    public class LongChat : Chat
    {
        public LongChat(List<IChatOwnerInfo> owners, Queue<Entry> messages = null) : base(owners, messages)
        {
        }
    }
}