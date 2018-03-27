using System;

namespace GameLogic
{
    /// <summary>
    /// Модификатор
    /// </summary>
    public class Modifier
    {
        //модифицируемый объект
        readonly IModified modified = null;

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
        }

        /// <summary>
        /// Воздействие на модифицируемый объект
        /// </summary>
        public virtual void Action()
        {
            try
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
            catch (Exception e)
            { throw e; }
        }

        public Modifier(Modifier original, IModified modified)
        {
            if (original == null || modified == null)
                throw new ArgumentNullException();

            this.MemberwiseClone();
            this.modified = modified;
        }
    }
}