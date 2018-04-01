namespace GameLogic
{
    /// <summary>
    /// Классы существ
    /// </summary>
    public enum SoliderClass
    {
        triangle,
        square,
        circle
    }
    
    /// <summary>
    /// Направление воздействия модификатора
    /// </summary>
    public enum Context
    {
        health,
        loyality,
        mana,
        power
    }
    /// <summary>
    /// Значение события
    /// </summary>
    public enum Means
    {
        Positive,
        Negative
    }
}