namespace AuxiliaryLibrary
{
    /// <summary>
    /// Основа событийных аргументов без параметров
    /// </summary>
    /// <typeparam name="T">Тип вызвавший объекта</typeparam>
    /// <param name="sender">Объект, вызвавший событие</param>
    public delegate void NonParametrizedEventHandler<T>(T sender);
}