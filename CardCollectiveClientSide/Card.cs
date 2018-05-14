using System;
using CardCollectiveSession;

namespace CardCollectiveSharedSide
{
    [Serializable]
    public class Card
    {
        public Card(Card card,
            string name, string description)
        {
            try
            {
                Representation = card;
                Description = description;
                Name = name;
            }
            catch { throw; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if ((description = value) == string.Empty) throw new ArgumentException("Empty view name");
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if ((name = value) == string.Empty) throw new ArgumentException("Empty name");
            }
        }

        Card representation;
        public Card Representation
        {
            get { return representation; }
            set
            {
                if ((representation = value) == null) throw new ArgumentNullException(nameof(value));
            }
        }
    }
}