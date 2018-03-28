using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    /// <summary>
    /// Игровая карта
    /// </summary>
    public struct Map
    {
        /// <summary>
        /// Размеры карты
        /// </summary>
        public int Size
        { get { return map.GetLength(0); } }

        public Map(int size, DurableModifier[] possibleModifiers)
        {
            if (size < 2 || size % 2 != 0)
                throw new ArgumentException();

            map = new KeyValuePair<DurableModifier, IPositionable>[size, size];
            
            this.possibleModifiers = possibleModifiers;
        }

        KeyValuePair<DurableModifier, IPositionable>[,] map;

        DurableModifier[] possibleModifiers;

        public void AddObj(IPositionable obj, Position position)
        {
            //TODO
        }

        public void RemoveObj(IPositionable obj)
        {
            //TODO
        }
        

        public void AddMod(DurableModifier mod, Position position)
        {
            //TODO
        }

        public void RemoveMod(Position position)
        {
            //TODO
        }


        public IPositionable FindByPosition(Position position)
        {
            return null;
            //TODO
        }


        public void MoveTo(Position position, IPositionable obj)
        {
            //TODO
        }
        

        public void GenerateMap()
        {
            //TODO
        }
    }
}