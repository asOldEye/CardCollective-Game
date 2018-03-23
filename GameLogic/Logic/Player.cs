//using System;
//using System.Collections.Generic;

//namespace GameLogic
//{
//    //игрок
//    public class Player : IDestroyable, IAttacking, ICaster, IModifiedObject<Modifier>
//    {
//        //мана
//        private int mana = -1;
//        public int Mana
//        {
//            get { return mana; }
//            set { if (value < 0) throw new ArgumentException("Wrong mana value"); mana = value; }
//        }

//        //атака
//        private int power = -1;
//        public int Power
//        {
//            get { return power; }
//            set { if (value < 0) throw new ArgumentException("Wrong power value"); power = value; }
//        }

//        //метод для атаки
//        public bool Attack(IDestroyable target)
//        {
//            if (target == null) throw new ArgumentException("Invalid target value");
//            return false;
//            //TODO
//        }

//        //колода игрока
//        private Deck<SoliderCard> playerDeck;
//        public Deck<SoliderCard> PlayerDeck
//        { get { return playerDeck; } }

//        //конструктор игрока
//        public Player(int mana, int power, int health, Deck<SoliderCard> deck)
//        {
//            try
//            {
//                Mana = mana;
//                Power = power;
//                Health = health;
//                playerDeck = deck;
//            }
//            catch (Exception e) { throw e; }
//        }

//        //здоровье
//        private int health = -1;
//        public int Health
//        {
//            get { return health; }
//            set { if (value < 0) throw new ArgumentException("Wrong health value"); health = value; }
//        }

//        //получение урона
//        public bool TakeDamage(int damage)
//        {
//            if (damage < 0) throw new ArgumentException("Invalid damage value");
//            return false;
//            //TODO
//        }

//        //лечение
//        public void Cure(int health)
//        {
//            if (health < 0) throw new ArgumentException("Invalid health value");

//            //TODO
//        }
        
//        Deck<SpellCard> spells;
//        public Deck<SpellCard> Spells
//        { get { return spells; } }

//        public void Cast(SpellCard spell, IDestroyable target = null)
//        {
//            //TODO
//        }

//        Queue<KeyValuePair<Modifier, int>> modifiers;
//        public Queue<KeyValuePair<Modifier, int>> Modifiers
//        { get { return modifiers; } }

//        public void AddModifier(Modifier modifier, int time)
//        {
//            //TODO
//        }

//        public void TurnTick()
//        {
//            //TODO
//        }
//    }
//}