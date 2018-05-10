using AuxiliaryLibrary;
using System.Collections.Generic;

namespace Session
{
    /// <summary>
    /// Даёт возможность расположить объект на игровом поле
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// Радиус передвижения
        /// </summary>
        int MoveRadius
        { get; }
        /// <summary>
        /// Изменить радиус перемещения
        /// </summary>
        /// <param name="delta">Изменение</param>
        void DeltaMoveRadius(int delta);

        /// <summary>
        /// Вызывается при изменении радиуса передвижения
        /// </summary>
        event ParametrizedEventHandler<IMovable, int> OnMoveRadiusChanged;
    }
}