using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// Модификатор
    /// </summary>
    public class Modifier
    {
        //модифицируемый объект
        object modified;

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

            switch(type)
            {
                case ModifierType.health:
                    {
                        if(!(modified is IDestroyable))
                            throw new ArgumentException();
                        break;
                    }
                case ModifierType.loyality:
                    {
                        if (!(modified is SoliderCard))
                            throw new ArgumentException();
                        break;
                    }
                case ModifierType.mana:
                    {
                        if (!(modified is ICaster))
                            throw new ArgumentException();
                        break;
                    }
                case ModifierType.power:
                    {
                        if (!(modified is IAttacker))
                            throw new ArgumentException();
                        break;
                    }
            }

            this.type = type;
            Impact = impact;
        }

        /// <summary>
        /// Воздействие на модифицируемый объект
        /// </summary>
        public void Action()
        {
            switch (type)
            {
                case ModifierType.health:
                    {
                        (modified as IDestroyable).DeltaHealth(impact);
                        break;
                    }
                case ModifierType.loyality:
                    {
                        (modified as SoliderCard).DeltaLoyality(impact);
                        break;
                    }
                case ModifierType.mana:
                    {
                        (modified as ICaster).DeltaMana(impact);
                        break;
                    }
                case ModifierType.power:
                    {
                        (modified as IAttacker).DeltaPower(impact);
                        break;
                    }
            }
        }
    }
}
