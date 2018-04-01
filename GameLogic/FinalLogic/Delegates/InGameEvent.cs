namespace GameLogic
{
    /// <summary>
    /// Делегирует внутрисессийные события
    /// </summary>
    /// <param name="sender">объект-отправитель</param>
    /// <param name="e">сопровождающие аргументы</param>
    public delegate void InGameEvent(object sender, SessionEventArgs e);
}
