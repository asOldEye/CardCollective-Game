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
        public int Size
        { get { return map.GetLength(0); } }

        public Map(int size, Pair<Modifier, float>[] possibleModifiersRarity = null)
        {
            if (size < 2 || size % 2 != 0)
                throw new ArgumentException();

            map = new Pair<Modifier, IPositionable>[size, size];

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
            if (obj == null) throw new ArgumentNullException();
            if (pos.X > Size || pos.Y > Size) throw new ArgumentOutOfRangeException();
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
        }

        /// <summary>
        /// Удалить заданный объект с карты
        /// </summary>
        /// <param name="obj">объект</param>
        public void RemoveObj(IPositionable obj)
        {
            if (obj == null) throw new ArgumentNullException();

            var f = FindByPosition(obj.Position);

            if (f != obj) throw new ArgumentException("Obj not on map");

            var pos = obj.Position;

            map[pos.X, pos.Y].Obj2 = null;
            obj.Position = null;

            if (obj is IModifiedDurable)
            {
                (obj as IModifiedDurable).Modifiers.
                    Find(new Predicate<DurableModifier>(n => n == f.Obj1 as DurableModifier))
                    .Timing = 0;
            }
        }

        /// <summary>
        /// Установить модификатор заданной позиции
        /// </summary>
        /// <param name="mod">модификатор</param>
        /// <param name="position">позиция</param>
        public void SetMod(Modifier mod, Position position)
        {
            
        }

        /// <summary>
        /// Удалить модификатор с заданной позиции
        /// </summary>
        /// <param name="position">позиция</param>
        public void RemoveMod(Position position)
        {
            
        }

        /// <summary>
        /// Найти объект по позиции
        /// </summary>
        /// <param name="position">позиция</param>
        /// <returns></returns>
        public Pair<Modifier, IPositionable> FindByPosition(Position position)
        {
            if (position.X > Size || position.Y > Size) throw new IndexOutOfRangeException();

            return map[position.X, position.Y];
        }

        /// <summary>
        /// Переместить на позицию
        /// </summary>
        /// <param name="position">позиция</param>
        /// <param name="obj">перемещаемый</param>
        public void MoveTo(Position position, IPositionable obj)
        {
            
        }

        /// <summary>
        /// Возвращает карту в виде массива модификатор - поставленный объект
        /// </summary>
        /// <returns></returns>
        public Pair<Modifier, IPositionable>[,] ToArray()
        {
            var f = new Pair<Modifier, IPositionable>[Size, Size];
            Array.Copy(map, f, 0);
            return f;
        }

        public List<Pair<Modifier, IPositionable>> ByRadius(Position position)
        {
            //TODO
            return null;
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
        {
            RemoveObj(sender as IPositionable);
        }

        //TODO при добавлении карты подписывать на удаление с карты
    }
}