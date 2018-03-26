namespace GameLogic
{
    /// <summary>
    /// Даёт возможность расположить объект на игровом поле
    /// </summary>
    public interface IPositionable
    {
        /// <summary>
        /// Вызывается при появлении на поле
        /// </summary>
        event InGameEvent OnPositionAppears;

        /// <summary>
        /// Вызывается при убирании с поля
        /// </summary>
        event InGameEvent OnPositionDisappears;

        /// <summary>
        /// Вызывается при изменении позиции
        /// </summary>
        event InGameEvent OnPositionSet;

        /// <summary>
        /// Позиция на поле, либо ее отсутствие
        /// </summary>
        Position? Position
        { get; set; }
    }
}