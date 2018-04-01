using System;

namespace GameLogic
{
    /// <summary>
    /// Модификатор
    /// </summary>
    public class Modifier
    {
        public IModified Modified { get; set; } = null;

        /// <summary>
        /// То, на что воздействует модификатор
        /// </summary>
        public Context Type { get; }

        /// <summary>
        /// Сила воздействия модификатора
        /// </summary>
        public int Impact { get; protected set; }

        /// <summary>
        /// Воздействие на модифицируемый объект
        /// </summary>
        public virtual void Action()
        {
            try
            {
                switch (Type)
                {
                    case Context.health:
                        (Modified as IDestroyable).DeltaHealth(Impact);
                        break;
                    case Context.loyality:
                        (Modified as ILoyal).DeltaLoyality(Impact);
                        break;
                    case Context.mana:
                        (Modified as ICaster).DeltaMana(Impact);
                        break;
                    case Context.power:
                        (Modified as IAttacker).DeltaPower(Impact);
                        break;
                }
            }
            catch { throw; }
        }

        public Modifier(Context type, int impact, IModified modified = null)
        {
            Type = type;
            Impact = impact;

            if (modified != null)
                Modified = modified;
        }

        public Modifier(Modifier original, IModified modified)
        {
            if (original == null || modified == null)
                throw new ArgumentNullException();

            Type = original.Type;
            Impact = original.Impact;

            Modified = modified;
        }
    }
}