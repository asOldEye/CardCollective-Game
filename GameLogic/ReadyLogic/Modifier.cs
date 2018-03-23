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
       

        public Modifier(ModifierType type, int impact)
        {
            this.type = type;
            this.impact = impact;
        }
    }
}
