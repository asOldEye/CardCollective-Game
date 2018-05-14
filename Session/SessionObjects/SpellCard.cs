using System.Collections.Generic;

namespace CardCollectiveSession
{
    public class SpellCard : Card
    {
        public SpellCard(int cardCost, List<Modifier> modifiers) : base(cardCost)
        {
            try
            { AddComponent(new Spell(modifiers)); }
            catch { throw; }
        }
    }
}