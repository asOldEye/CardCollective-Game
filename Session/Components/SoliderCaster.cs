using AuxiliaryLibrary;
using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    public class SoliderCaster : Component
    {
        public SoliderCaster(List<SoliderCard> soliders)
        {
            if ((Soliders = soliders) == null) throw new ArgumentNullException("Null soliders list");
        }

        /// <summary>
        /// Колода солдат игрока
        /// </summary>
        public List<SoliderCard> Soliders
        { get; } = new List<SoliderCard>();

        /// <summary>
        /// Вытаскивает карту солдата из колоды на стол
        /// </summary>
        /// <param name="solider"></param>
        [ControllerCommand]
        [Modified]
        public void CastSolider(SoliderCard solider, Position position)
        {
            if (position.CompareTo(Container.Session.Map.Size) >= 0) throw new ArgumentException("Too big position");
            var f = Container.Session.Map.FindByPosition(position);
            if (f.Positioned != null) throw new ArgumentException("Another target already in this position");
            var Mana = Container.GetComponent<Mana>();
            if (Mana == null) throw new ArgumentException("No mana in container");
            if (solider == null) throw new ArgumentNullException(nameof(solider));
            if (!Soliders.Remove(solider)) throw new ArgumentException("It is'not my spell");

            if (Mana.Value < solider.GetComponent<Cost>().Value) throw new ArgumentException("Low mana");
            Mana.DeltaMana(-solider.GetComponent<Cost>().Value);
            
            solider.SetOwner(Container.Owner);
            solider.GetComponent<Positionable>().Position = position;
            solider.GetComponent<Positionable>().Positioned = true;

            if (OnSoliderCasted != null)
                OnSoliderCasted.Invoke(new SessionChange()); //TODO
        }

        public override void Appear(Container container)
        {
            base.Appear(container);
            OnSoliderCasted += container.Session.SessionChanged;
        }

        /// <summary>
        /// Вызывается при вызове карты из колоды на стол
        /// </summary>
        public event NonParametrizedEventHandler<SessionChange> OnSoliderCasted;
    }
}