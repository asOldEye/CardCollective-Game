using System;

namespace GameLogic
{
    /// <summary>
    /// Модификатор
    /// </summary>
    public class Modifier
    {
        //модифицируемый объект
        IModified modified;

        ModifierType type;
        /// <summary>
        /// То, на что воздействует модификатор
        /// </summary>
        public ModifierType Type
        { get { return type; } }
   
        int impact;
        /// <summary>
        /// Сила воздействия модификатора
        /// </summary>
        public int Impact
        {
            get { return impact; }
            protected set { impact = value; }
        }

        public Modifier(IModified modified, ModifierType type, int impact)
        {
            this.modified = modified ?? throw new ArgumentNullException();
            
            this.type = type;
            Impact = impact;
        }

        /// <summary>
        /// Воздействие на модифицируемый объект
        /// </summary>
        public virtual void Action()
        {
            switch (type)
            {
                case ModifierType.health:
                    if (modified is IDestroyable)
                        (modified as IDestroyable).DeltaHealth(impact);
                    break;
                case ModifierType.loyality:
                    if (modified is SoliderCard)
                        (modified as SoliderCard).DeltaLoyality(impact);
                    break;
                case ModifierType.mana:
                    if (modified is ICaster)
                        (modified as ICaster).DeltaMana(impact);
                    break;
                case ModifierType.power:
                    if (modified is IAttacker)
                        (modified as IAttacker).DeltaPower(impact);
                    break;
            }
        }
    }
}
