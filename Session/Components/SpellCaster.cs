using AuxiliaryLibrary;
using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    public class SpellCaster : Component
    {
        public List<Spell> Spells { get; }
        public SpellCaster(List<Spell> toCast)
        {
            if ((Spells = toCast) == null) throw new ArgumentNullException(nameof(toCast));
        }

        public override void Appear(Container container)
        {
            base.Appear(container);
            OnSpellCast += container.Session.SessionChanged;
        }
        /// <summary>
        /// Колода заклинаний
        /// </summary>
        public List<SpellCard> SpellCards
        { get; } = new List<SpellCard>();

        /// <summary>
        /// Создать заклинание
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// <param name="target">Цель</param>
        [Modified]
        [ControllerCommand]
        public void Cast(SpellCard spell, Position target)
        {
            if (target.CompareTo(Container.Session.Map.Size) >= 0) throw new ArgumentException("Too big position");
            var f = Container.Session.Map.FindByPosition(target).Positioned;
            if (f == null) throw new ArgumentException("No target in this position");
            try
            { Cast(spell, f); }
            catch { throw; }
        }
        /// <summary>
        /// Создать заклинание
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// <param name="target">Цель</param>
        [Modified]
        [ControllerCommand]
        public void Cast(SpellCard spell, Container target)
        {
            var Mana = Container.GetComponent<Mana>();
            if (Mana == null) throw new ArgumentException("No mana in container");
            if (spell == null) throw new ArgumentNullException(nameof(spell));
            if (!SpellCards.Contains(spell)) throw new ArgumentException("It is'not my spell");

            if (Mana.Value < spell.GetComponent<Cost>().Value) throw new ArgumentException("Low mana");

            Mana.DeltaMana(-spell.GetComponent<Cost>().Value);
            spell.GetComponent<Spell>().Use(target);

            if (OnSpellCast != null)
                OnSpellCast.Invoke(new SessionChange()); //TODO
        }

        /// <summary>
        /// Событие вызывается при создании объекта
        /// </summary>
        public event NonParametrizedEventHandler<SessionChange> OnSpellCast;
    }
}