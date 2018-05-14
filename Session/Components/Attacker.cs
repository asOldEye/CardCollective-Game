using AuxiliaryLibrary;
using System;

namespace CardCollectiveSession
{
    public class Attacker : Component
    {
        public Attacker(int attackPowerMax, int attackPower)
        {
            if ((AttackPowerMax = attackPowerMax) < 0)
                throw new ArgumentException("Wrong attack power max, it must be more not less than zero");
            if ((AttackPower = attackPower) < 0 || AttackPower > AttackPowerMax)
                throw new ArgumentException("Wrong attack power max, it must be more not less than zero and not bigger than attack power max");
        }

        public override void Appear(Container container)
        {
            base.Appear(container);
            OnPowerChanged += container.Session.SessionChanged;
            OnAttack += container.Session.SessionChanged;
        }

        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        public int AttackPowerMax { get; private set; }

        private int attackPower;
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int AttackPower
        {
            get { return attackPower; }
            protected set
            {
                if (value < 1) value = 1;
                if (value > AttackPowerMax) value = AttackPowerMax;

                int delta = attackPower - value;
                attackPower = value;

                if (OnPowerChanged != null)
                    OnPowerChanged.Invoke(new SessionChange()); //TODO
            }
        }

        /// <summary>
        /// Атака уничтожаемого объекта
        /// </summary>
        /// <param name="target">Атакуемый объект</param>
        /// <returns></returns>
        [Modified]
        [ControllerCommand]
        public void Attack(Container target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            Destroyable destr;
            if ((destr = target.GetComponent<Destroyable>()) == null)
                throw new ArgumentException("target have'nt Destroyable component");

            if (OnAttack != null)
                OnAttack.Invoke(new SessionChange()); //TODO
            int attackPower = this.attackPower;
            destr.DeltaHealth(-attackPower);
        }

        /// <summary>
        /// Изменение атаки объекта
        /// </summary>
        [Modified]
        public virtual void DeltaPower(int delta)
        { AttackPower += delta; }
        
        public event NonParametrizedEventHandler<SessionChange> OnPowerChanged;
        public event NonParametrizedEventHandler<SessionChange> OnAttack;
    }
}