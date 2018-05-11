using AuxiliaryLibrary;
using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    /// <summary>
    /// Игровая карта
    /// </summary>
    public struct Map
    {
        MapSquare[,] map;
        /// <summary>
        /// Размеры карты
        /// </summary>
        public Position Size
        { get { return new Position(map.GetLength(0), map.GetLength(1)); } }

        public Map(Position size, Pair<Modifier, int>[] possibleModifiersRarity = null)
        {
            map = new MapSquare[size.X, size.Y];
            if (possibleModifiersRarity != null)
                GenerateMap(possibleModifiersRarity);
        }

        /// <summary>
        /// Поставить объект на заданную позицию карты
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="pos">позиция</param>
        public void SetObj(PositionableSessionObject obj, Position pos)
        {
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException("Too big position");
            if (FindByPosition(pos).Positioned != null) throw new ArgumentException("Here's already another obj in this position");

            map[pos.X, pos.Y].Positioned = obj ?? throw new ArgumentNullException("Null object");

            foreach (var f in map[pos.X, pos.Y].Modifiers)
                if (obj is IModified && f is DurableModifier)
                    (obj as IModified).AddModifier(f as DurableModifier);
        }

        /// <summary>
        /// Установить модификатор заданной позиции
        /// </summary>
        /// <param name="mod">модификатор</param>
        /// <param name="position">позиция</param>
        public void SetMod(Modifier mod, Position pos)
        {
            if (mod == null) throw new ArgumentNullException("Null modifier");
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException("Too big position");
            var square = map[pos.X, pos.Y];

            square.Modifiers.Add(mod);

            if (square.Positioned != null && mod is DurableModifier)
                if (square.Positioned is IModified)
                    (mod as DurableModifier).AddModified(square.Positioned as IModified);
        }

        /// <summary>
        /// Поставить объект на заданную позицию карты
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="pos">Позиция</param>
        public void DelObj(Position pos)
        {
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException("Too big position");
            if (FindByPosition(pos).Positioned == null) throw new ArgumentException("Here's already no positioned objects");

            if (map[pos.X, pos.Y].Positioned is IModified)
                foreach (var mod in map[pos.X, pos.Y].Modifiers)
                    if (mod is DurableModifier)
                        (mod as DurableModifier).SetTiming(map[pos.X, pos.Y].Positioned as IModified, 0);

            map[pos.X, pos.Y].Positioned = null;
        }

        /// <summary>
        /// Установить модификатор заданной позиции
        /// </summary>
        /// <param name="mod">Модификатор</param>
        /// <param name="position">Позиция</param>
        public void DelMod(Modifier mod, Position pos)
        {
            if (mod == null) throw new ArgumentNullException("Null modifier");
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException("Too big position");

            var square = map[pos.X, pos.Y];

            if (!square.Modifiers.Remove(mod)) throw new ArgumentException("Here is'nt this modifier");

            if (square.Positioned != null && mod is DurableModifier)
                if (square.Positioned is IModified)
                    (mod as DurableModifier).SetTiming(map[pos.X, pos.Y].Positioned as IModified, 0);
        }

        /// <summary>
        /// Найти объект по позиции
        /// </summary>
        /// <param name="position">позиция</param>
        /// <returns></returns>
        public MapSquare FindByPosition(Position position)
        {
            if (position.X > Size.X || position.Y > Size.Y)
                throw new IndexOutOfRangeException("Wrong position");
            return map[position.X, position.Y];
        }

        /// <summary>
        /// Найти позицию по объекту
        /// </summary>
        /// <param name="positioned">Объект</param>
        /// <returns>Позиция объекта</returns>
        public Position FindPosition(PositionableSessionObject positioned)
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j].Positioned == positioned) return new Position(i, j);
            return Position.Empty;
        }

        /// <summary>
        /// Возвращает карту в виде массива модификатор - поставленный объект
        /// </summary>
        /// <returns></returns>
        public MapSquare[,] ToArray()
        {
            var f = new MapSquare[Size.X, Size.Y];
            Array.Copy(map, f, map.Length);
            return f;
        }

        /// <summary>
        /// Возвращает список объектов, находящихся в заданном радиусе
        /// </summary>
        /// <param name="position">Центр</param>
        /// <param name="radius">Радиус</param>
        /// <returns></returns>
        public List<MapSquare> ByRadius(Position position, int radius)
        {
            var list = new List<MapSquare>();

            Position current = new Position(0, 0);
            for (int i = position.X - radius; i < position.X + radius; i++)
                for (int j = position.Y - radius; j < position.Y + radius; j++)
                    if (i >= 0 && i < Size.X &&
                        j >= 0 && j < Size.Y)
                    {
                        current.X = i;
                        current.Y = j;
                        if (Position.Distance(position, current) <= radius)
                            list.Add(map[i, j]);
                    }
            return list;
        }

        /// <summary>
        /// Генерирует карту
        /// </summary>
        /// <param name="possibleModifiersRarity"></param>
        void GenerateMap(Pair<Modifier, int>[] possibleModifiersRarity)
        {
            foreach (var f in map)
                foreach (var mod in possibleModifiersRarity)
                    if (Session.Random.NextPercent(mod.Obj2))
                        f.Modifiers.Add(mod.Obj1);
        }
    }
}