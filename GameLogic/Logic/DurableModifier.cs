using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// Долговременный модификатор
    /// </summary>
    class DurableModifier : Modifier
    {
        private int timing = -1;
        /// <summary>
        /// Оставшееся время действия. -1 => бесконечно
        /// </summary>
        public int Timing
        {
            get { return timing; }
            protected set
            {
                if (timing == 0)
                {
                    OnTimeOut.Invoke(this, new GameEventArgs(/*TODO*/));
                }
                timing = value;
            }
        }

        public DurableModifier(IModified modified, ModifierType type, int impact, int timing = -1)
            : base(modified, type, impact)
        { this.timing = timing; }

        /// <summary>
        /// Воздействует на прикрепленный объект объект
        /// </summary>
        public override void Action()
        {
            base.Action();
            timing--;
        }

        /// <summary>
        /// Вызывается по истечении срока действия
        /// </summary>
        public event InGameEvent OnTimeOut;
    }
}
