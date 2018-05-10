using AuxiliaryLibrary;
using System;
using System.Collections.Generic;

namespace Session
{
    /// <summary>
    /// Долговременный модификатор
    /// </summary>
    public class DurableModifier : Modifier
    {
        List<FreePair<IModified, int>> modified = new List<FreePair<IModified, int>>();

        /// <summary>
        /// Время действия модификатора
        /// </summary>
        public int Timing
        { get; }
        
        public DurableModifier(Session session, int impact, string deltaMethodName, int timing = -1)
            : base(session, impact, deltaMethodName)
        {
            if ((Timing = timing) == 0)
                throw new ArgumentException("Wrong timing");
        }

        /// <summary>
        /// Установить тайминг для модифицируемого объекта
        /// </summary>
        /// <param name="target">Цель</param>
        /// <param name="newTiming">Новый тайминг</param>
        public void SetTiming(IModified target, int newTiming)
        {
            if (target == null) throw new ArgumentNullException("Null target");
            var f = modified.Find(mod => mod.Obj1 == target);
            if (f != null) f.SetObj2(newTiming);
            else throw new ArgumentException("Does'nt contain this target");
        }

        /// <summary>
        /// Добавляет новый модифицируемый объект
        /// </summary>
        /// <param name="target"></param>
        public void AddModified(IModified target)
        {
            if (target == null) throw new ArgumentNullException("Null target");
            var f = modified.Find(mod => mod.Obj1 == target);
            if (f == null) modified.Add(new FreePair<IModified, int>(target, Timing));
            else throw new ArgumentException("Does'nt contain this target");
        }

        /// <summary>
        /// Воздействует на объекты
        /// </summary>
        protected virtual void Action(Player player)
        {
            foreach (var f in modified)
            {
                if ((f.Obj1 as SessionObject).Owner == player)
                {
                    if (f.Obj2 > 0) f.SetObj2(f.Obj2 - 1);

                    if (f.Obj2 == 0)
                    {
                        modified.Remove(f);
                        f.Obj1.DelModifier(this);
                    }
                    else base.Action(f.Obj1 as SessionObject);
                }
            }
        }

        protected override void OnTurn(Player player)
        { Action(player); }

        public override void Destroy()
        {
            base.Destroy();

            foreach (var f in modified)
                f.Obj1.DelModifier(this);

            modified = null;
            Session.OnTurn -= OnTurn;
        }
    }
}