using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Игровая карта
    /// </summary>
    public struct Map
    {
        //карта
        Pair<Modifier, IPositionable>[,] map;

        /// <summary>
        /// Размеры карты
        /// </summary>
        public Position Size
        {
            get
            { return new Position(map.GetLength(0), map.GetLength(1)); }
        }

        public Map(Position size, Pair<Modifier, float>[] possibleModifiersRarity = null)
        {
            if (size == null) throw new ArgumentNullException();

            map = new Pair<Modifier, IPositionable>[size.X, size.Y];

            if (possibleModifiersRarity != null)
                GenerateMap(possibleModifiersRarity);
        }

        /// <summary>
        /// Поставить объект на заданную позицию карты
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="pos">позиция</param>
        public void SetObj(IPositionable obj, Position pos)
        {
            if (obj == null || pos == null) throw new ArgumentNullException();
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException();
            if (FindByPosition(pos).Obj2 != null) throw new ArgumentException();

            obj.Position = pos;
            map[pos.X, pos.Y].Obj2 = obj;

            var mod = map[pos.X, pos.Y].Obj1;

            if (obj is IModified)
            {
                if (obj is IModifiedDurable)
                    (obj as IModifiedDurable).TakeModifier(mod);
                else
                    (obj as IModified).TakeModifier(mod);

                mod.Modified = obj as IModified;
            }

            if (obj is IDestroyable)
                (obj as IDestroyable).OnDeath += OnDeath;
        }

        /// <summary>
        /// Удалить заданный объект с карты
        /// </summary>
        /// <param name="obj">объект</param>
        public void RemoveObj(IPositionable obj)
        {
            if (obj == null) throw new ArgumentNullException();

            var f = FindByPosition(obj.Position);

            if (f.Obj2 != obj) throw new ArgumentException("Obj not on map");

            var pos = obj.Position;

            map[pos.X, pos.Y].Obj2 = null;
            obj.Position = null;

            if (obj is IModifiedDurable)
            {
                var q = (obj as IModifiedDurable).Modifiers.
                    Find(new Predicate<DurableModifier>(n => n == f.Obj1 as DurableModifier));

                q.Modified = null;
                (obj as IModifiedDurable).Modifiers.Remove(q);
            }

            if (obj is IDestroyable)
                (obj as IDestroyable).OnDeath -= OnDeath;
        }

        /// <summary>
        /// Установить модификатор заданной позиции
        /// </summary>
        /// <param name="mod">модификатор</param>
        /// <param name="position">позиция</param>
        public void SetMod(Modifier mod, Position pos)
        {
            if (pos == null || mod == null) throw new ArgumentNullException();
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException();

            var dot = FindByPosition(pos);
            if (dot.Obj1 != null) RemoveMod(pos);

            dot.Obj1 = mod;

            if (dot.Obj2 != null)
            {
                if (dot.Obj2 is IModified)
                {
                    if (dot.Obj2 is IModifiedDurable)
                        (dot.Obj2 as IModifiedDurable).TakeModifier(mod);
                    else
                        (dot.Obj2 as IModified).TakeModifier(mod);

                    mod.Modified = dot.Obj2 as IModified;
                }
            }
        }

        /// <summary>
        /// Удалить модификатор с заданной позиции
        /// </summary>
        /// <param name="position">позиция</param>
        public void RemoveMod(Position pos)
        {
            if (pos == null) throw new ArgumentNullException();
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException();

            var dot = FindByPosition(pos);

            if (dot.Obj1 == null) return;

            if (dot.Obj2 != null)
            {
                if (dot.Obj2 is IModifiedDurable)
                {
                    var q = (dot.Obj2 as IModifiedDurable).Modifiers.
                        Find(new Predicate<DurableModifier>(n => n == dot.Obj1 as DurableModifier));

                    q.Modified = null;
                    (dot.Obj2 as IModifiedDurable).Modifiers.Remove(q);
                }
            }

            dot.Obj1 = null;
        }

        /// <summary>
        /// Найти объект по позиции
        /// </summary>
        /// <param name="position">позиция</param>
        /// <returns></returns>
        public Pair<Modifier, IPositionable> FindByPosition(Position position)
        {
            if (position.X > Size.X || position.Y > Size.Y) throw new IndexOutOfRangeException();

            return map[position.X, position.Y];
        }

        /// <summary>
        /// Переместить на позицию
        /// </summary>
        /// <param name="position">позиция</param>
        /// <param name="obj">перемещаемый</param>
        public void MoveTo(Position pos, IPositionable obj)
        {
            if (obj == null || pos == null) throw new ArgumentNullException();
            if (pos.X > Size.X || pos.Y > Size.Y) throw new ArgumentOutOfRangeException();
            if (FindByPosition(pos).Obj2 != null) throw new ArgumentException();

            map[pos.X, pos.Y].Obj2 = obj;
            map[obj.Position.X, obj.Position.Y].Obj2 = null;

            var mod = map[pos.X, pos.Y].Obj1;

            if (obj is IModified)
            {
                if (obj is IModifiedDurable)
                    (obj as IModifiedDurable).TakeModifier(mod);
                else
                    (obj as IModified).TakeModifier(mod);

                mod.Modified = obj as IModified;
            }

            mod = map[obj.Position.X, obj.Position.Y].Obj1;

            if (obj is IModifiedDurable)
            {
                var f = (obj as IModifiedDurable).Modifiers.Find(new Predicate<DurableModifier>(
                    n => n == mod as DurableModifier));

                f.Modified = null;
                (obj as IModifiedDurable).Modifiers.Remove(f);
            }

            obj.Position = pos;
        }

        /// <summary>
        /// Возвращает карту в виде массива модификатор - поставленный объект
        /// </summary>
        /// <returns></returns>
        public Pair<Modifier, IPositionable>[,] ToArray()
        {
            var f = new Pair<Modifier, IPositionable>[Size.X, Size.Y];
            Array.Copy(map, f, 0);
            return f;
        }

        /// <summary>
        /// Возвращает списко объектов, находящихся в заданном радиусе
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public List<IPositionable> ByRadius(Position position, int radius)
        {
            var list = new List<IPositionable>();

            Position current = new Position(0, 0);
            for (int i = position.X - radius; i < position.X + radius; i++)
                for (int j = position.Y - radius; j < position.Y + radius; j++)
                {
                    if (i >= 0 && i < Size.X &&
                        j >= 0 && j < Size.Y)
                    {
                        current.X = i;
                        current.Y = j;
                        if (Position.Distance(position, current) <= radius)
                            if (map[i, j].Obj2 != null)
                                list.Add(map[i, j].Obj2);
                    }
                }
            return list;
        }

        /// <summary>
        /// Генерирует карту
        /// </summary>
        /// <param name="possibleModifiersRarity"></param>
        void GenerateMap(Pair<Modifier, float>[] possibleModifiersRarity)
        {
            //TODO
        }

        public void OnDeath(object sender, SessionEventArgs e)
        { RemoveObj(sender as IPositionable); }
    }
}