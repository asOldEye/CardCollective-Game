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

        Context type;
        /// <summary>
        /// То, на что воздействует модификатор
        /// </summary>
        public Context Type
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

        public Modifier(Context type, int impact, IModified modified = null)
        {
            this.type = type;
            Impact = impact;

            if (modified != null)
                this.modified = modified;
        }

        /// <summary>
        /// Воздействие на модифицируемый объект
        /// </summary>
        public virtual void Action()
        {
            switch (type)
            {
                case Context.health:
                    if (modified is IDestroyable)
                        (modified as IDestroyable).DeltaHealth(impact);
                    break;
                case Context.loyality:
                    if (modified is ILoyal)
                        (modified as ILoyal).DeltaLoyality(impact);
                    break;
                case Context.mana:
                    if (modified is ICaster)
                        (modified as ICaster).DeltaMana(impact);
                    break;
                case Context.power:
                    if (modified is IAttacker)
                        (modified as IAttacker).DeltaPower(impact);
                    break;
            }
        }
    }
}
