namespace GameLogic
{
    /// <summary>
    /// Долговременный модификатор
    /// </summary>
    public class DurableModifier : Modifier
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
                    if (OnTimeOut != null)
                        OnTimeOut.Invoke(this, new GameEventArgs(GameEventArgs.Means.ModifierEnd));
                }
                timing = value;
            }
        }

        public DurableModifier(Context type, int impact, IModified modified = null, int timing = -1)
            : base(type, impact, modified)
        { this.timing = timing; }

        /// <summary>
        /// Воздействует на прикрепленный объект объект
        /// </summary>
        public override void Action()
        {
            if (timing != 0)
            {
                base.Action();
                timing--;
            }
        }

        /// <summary>
        /// Вызывается по истечении срока действия
        /// </summary>
        public event InGameEvent OnTimeOut;
    }
}