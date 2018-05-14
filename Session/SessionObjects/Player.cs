using System.Collections.Generic;

namespace CardCollectiveSession
{
    /// <summary>
    /// Игрок
    /// </summary>
    public class Castle : Container
    {
        public Castle(int healthMax, int health,
            int attackPowerMax, int attackPower,
            int moveRadius, 
            int manaMax, int mana,
            List<Spell> spells,
            List<SoliderCard> soliders)
        {
            try
            {
                AddComponent(new Destroyable(healthMax, health));
                AddComponent(new Attacker(attackPower, health));
                AddComponent(new Mana(manaMax, mana));
                AddComponent(new SpellCaster(spells));
                AddComponent(new SoliderCaster(soliders));
            }
            catch { throw; }
        }
    }
}