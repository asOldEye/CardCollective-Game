namespace GameLogic
{
    /// <summary>
    /// Даёт возможность расположить объект на игровом поле
    /// </summary>
    public interface IPositionable
    {
        /// <summary>
        /// Вызывается при изменении позиции
        /// </summary>
        event InGameEvent OnPositionChanged;

        /// <summary>
        /// Позиция на поле, либо ее отсутствие
        /// </summary>
        Position Position
        { get; set; }
    }
}