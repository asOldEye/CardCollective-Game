namespace GameLogic
{
    /// <summary>
    /// Пара объектов
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public struct Pair<T1, T2>
    {
        public Pair(T1 obj1 = default(T1), T2 obj2 = default(T2))
        {
            Obj1 = obj1;
            Obj2 = obj2;
        }

        /// <summary>
        /// Первый объект пары
        /// </summary>
        public T1 Obj1 { get; set; }

        /// <summary>
        /// Второй объект пары
        /// </summary>
        public T2 Obj2 { get; set; }
    }
}