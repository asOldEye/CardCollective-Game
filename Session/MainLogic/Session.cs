using System;
using System.Collections.Generic;
using System.Reflection;
using AuxiliaryLibrary;
using System.Threading.Tasks;

namespace CardCollectiveSession
{
    public class Session
    {
        public Session(Position mapSize, Pair<Modifier, int>[] mapModifiersProbability = null)
        {
            Map = mapModifiersProbability == null ? new Map(mapSize) : new Map(mapSize, mapModifiersProbability);
        }

        public bool IsPlay { get; private set; }

        static public readonly DistributionRandom Random = new DistributionRandom();
        int maxID = 0;
        List<Container> sessionObjects = new List<Container>();

        List<FreePair<Controller, TurnStatus>> controllers = new List<FreePair<Controller, TurnStatus>>();

        internal Map Map { get; }
        /// <summary>
        /// Этот игрок ходит прямо сейчас
        /// </summary>
        public Controller HisTurn { get; private set; }
        /// <summary>
        /// Добавить контроллер
        /// </summary>
        public void AddController(Controller controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (controllers.Find(new Predicate<FreePair<Controller, TurnStatus>>(q => q.Obj1 == controller)) != null) throw new ArgumentException("Already in session");
            if (IsPlay) throw new ArgumentException("Can't add controller on play");
            controllers.Add(new FreePair<Controller, TurnStatus>(controller, TurnStatus.expected));
        }
        /// <summary>
        /// Сделать ход
        /// </summary>
        public void MakeMove(Controller sender, Type componentType, int subjectID, string command, object[] param)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (param == null) throw new ArgumentNullException(nameof(param));
            if (componentType == null) throw new ArgumentNullException(nameof(componentType));

            var controller = controllers.Find(new Predicate<FreePair<Controller, TurnStatus>>(q => q.Obj1 == sender));
            if (controller == null)
                throw new ArgumentException("Controller is'nt player of this session");
            if (controller.Obj2 != TurnStatus.expected)
                throw new ArgumentException("Controller already make his move");

            var subject = GetObject(subjectID);
            if (subject == null) throw new ArgumentException("No subject with this ID in this session");

            var component = subject.GetComponent(componentType);
            if (component == null) throw new ArgumentException("No component " + componentType + " in this object");

            List<Type> types = new List<Type>();
            foreach (var f in param) types.Add(f.GetType());

            var method = component.GetType().GetMethod(command, types.ToArray());
            if (method == null) throw new ArgumentException("Wrong method or params");

            var attr = method.GetCustomAttribute(typeof(ControllerCommand), true);
            if (attr == null) throw new ArgumentException("Method is'nt controller command");
            if ((attr as ControllerCommand).OnMyTurn != (HisTurn == sender)) throw new ArgumentException("Turn error, not your turn");
            method.Invoke(component, param);
        }

        /// <summary>
        /// Добавить объект в игру
        /// </summary>
        public int AddObject(Container obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (sessionObjects.Contains(obj)) throw new ArgumentException("Already in session");
            sessionObjects.Add(obj);
            return maxID++;
        }
        /// <summary>
        /// Получает объект по его ID в сессии
        /// </summary>
        public Container GetObject(int id)
        {
            return sessionObjects.Find(new Predicate<Container>(f => f.ID == id));
        }
        /// <summary>
        /// Получает объекты определенного типа
        /// </summary>
        public List<Container> GetObjects<T>()
        {
            return sessionObjects.FindAll(new Predicate<Container>(f => f.GetType() == typeof(T)));
        }

        public NonParametrizedEventHandler<Controller> OnTurnEnds;
        public NonParametrizedEventHandler<Controller> OnSessionEnded;
        public EmptyEventHandler OnSessionStart;
        public EmptyEventHandler OnSessionChanged;

        internal void SessionChanged(SessionChange change)
        {
            if (IsPlay)
            {
                Changes.Add(change);
                if (OnSessionChanged != null) OnSessionChanged.Invoke();
            }
        }

        /// <summary>
        /// Изменения с начала сессии
        /// </summary>
        internal List<SessionChange> Changes { get; } = new List<SessionChange>();
        /// <summary>
        /// Статус хода
        /// </summary>
        public enum TurnStatus
        {
            expected,
            completed,
            missed
        }

        /// <summary>
        /// Начать игровую сессию
        /// </summary>
        public void StartSession()
        {
            if (IsPlay) throw new ArgumentException("Already in game");
            if (controllers.Count < 2) throw new ArgumentException("Not enought players");
            IsPlay = true;
            if (OnSessionStart != null) OnSessionStart.Invoke();
        }
        /// <summary>
        /// Вызывается для проверки условия завершения сессии
        /// </summary>
        public virtual bool SessionEndsCondition()
        {
            if (controllers.Count == 1) return true;
            return false;
        }
        /// <summary>
        /// Вызывается для проверки, выбыл ли игрок
        /// </summary>
        public virtual bool SessionRemoveControllerCondition(Controller controller)
        {
            if (controller.OwnedObjects.Find(q => q.GetType() == typeof(Castle)) == null)
                return true;
            return false;
        }
        /// <summary>
        /// Вызывается по окончании сессии для выяснения победителя
        /// </summary>
        /// <returns></returns>
        public virtual Controller Winner()
        {
            if (controllers.Count == 1) return controllers[0].Obj1;
            return null;
        }
        /// <summary>
        /// Удаляет игрока и его солдат из игры
        /// </summary>
        /// <param name="controller"></param>
        public void RemoveController(Controller controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            var c = controllers.Find(new Predicate<FreePair<Controller, TurnStatus>>(q => q.Obj1 == controller));
            if (c == null) throw new ArgumentException("This controller is'nt in this session");
            controllers.Remove(c);
            sessionObjects.RemoveAll(new Predicate<Container>(cont => cont.Owner == controller));
        }
    }
}