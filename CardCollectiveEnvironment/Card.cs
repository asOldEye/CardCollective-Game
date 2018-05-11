using System;
using CardCollectiveSession;

namespace CardCollectiveServerSide
{
    [Serializable]
    public class Card
    {
        public Card(ICard card,
            string name, string viewName)
        {
            try
            {
                Representation = card;
                ViewName = viewName;
                Name = name;
            }
            catch { throw; }
        }

        string viewName;
        public string ViewName
        {
            get { return viewName; }
            set
            {
                if (value == null) throw new ArgumentNullException("Null view name");
                if ((viewName = value) == string.Empty) throw new ArgumentException("Empty view name");
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == null) throw new ArgumentNullException("Null view name");
                if ((name = value) == string.Empty) throw new ArgumentException("Empty name");
            }
        }

        ICard representation;
        public ICard Representation
        {
            get { return representation; }
            set
            {
                if ((representation = value) == null) throw new ArgumentNullException("Null card");
            }
        }
    }
}