using System;

namespace GameLogic
{
    /// <summary>
    /// Долговременный модификатор
    /// </summary>
    public class DurableModifier : Modifier
    {
        /// <summary>
        /// Оставшееся время действия. -1 => бесконечно
        /// </summary>
        public int Timing { get; set; } = -1;

        public DurableModifier(Context type, int impact, int timing = -1, IModified modified = null)
            : base(type, impact, modified)
        {
            if (timing == 0) throw new ArgumentException("Wrong timing");
            Timing = timing;
        }

        public DurableModifier(Modifier original, IModified modified, int timing = -1)
            : base(original, modified)
        {
            if (timing == 0) throw new ArgumentException("Wrong timing");
            Timing = timing;
        }

        /// <summary>
        /// Воздействует на объект
        /// </summary>
        public override void Action()
        {
            if (Timing == 0) return;

            base.Action();
            if (Timing > 0) Timing--;
        }
    }
}
