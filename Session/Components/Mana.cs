using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    /// <summary>
    /// Мана
    /// </summary>
    public class Mana : Component
    {
        /// <summary>
        /// Максимальное количество маны
        /// </summary>
        public int MaxValue { get; }
        int value;
        /// <summary>
        /// Текущее количество маны
        /// </summary>
        public int Value
        {
            get => value;
            protected set
            {
                if (value < 0) value = 0;
                if (value > Value) value = Value;

                int delta = value - this.value;
                this.value = value;

                if (OnManaChanged != null)
                    OnManaChanged.Invoke(new SessionChange()); //todo
            }
        }
        /// <summary>
        /// Изменение количества маны
        /// </summary>
        [Modified]
        public void DeltaMana(int delta)
        { Value += delta; }

        public Mana(int maxValue, int value)
        {
            MaxValue = maxValue;
            Value = value;
        }
        public override void Appear(Container container)
        {
            base.Appear(container);
            OnManaChanged += container.Session.SessionChanged;
        }

        /// <summary>
        /// Событие, вызывающееся при изменении количества маны
        /// </summary>
        public event NonParametrizedEventHandler<SessionChange> OnManaChanged;
    }
}