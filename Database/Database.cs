using System;
using System.Collections.Generic;
using System.IO;
using AuxiliaryLibrary;

namespace SimpleDatabase
{
    public class Database
    {
        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string Name { get; private set; }
        string folder;

        /// <summary>
        /// Создать новую, либо открыть существующую базу данных
        /// </summary>
        /// <param name="name">Имя базы данных</param>
        /// <param name="directory">Директория расположения базы данных</param>
        public Database(string name, string directory)
        {
            if (!Directory.Exists(folder = directory + @"\" + name))
                Directory.CreateDirectory(folder);
        }

        /// <summary>
        /// Записать объект
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        /// <param name="obj">Содержимое объекта</param>
        public virtual void WriteObject(string key, object obj)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (Exists(key)) throw new ArgumentException("This node already exists");
            else
                try
                {
                    BinarySerializer.Serialize(obj, File.Create(folder + @"\" + key + ".database"));
                }
                catch { throw; }
        }
        /// <summary>
        /// Прочитать объект
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        /// <returns>Прочитанное содержимое объекта</returns>
        public virtual object ReadObject(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!Exists(key)) throw new ArgumentException("This node are'nt exists");
            else
                try
                {
                    var fs = new FileStream(folder + @"\" + key + ".database", FileMode.Open);
                    return BinarySerializer.Deserialize(fs);
                }
                catch { throw; }
        }
        /// <summary>
        /// Найти записи, соответствующие заданному шаблону ключа
        /// </summary>
        /// <param name="keyPattern">Шаблон</param>
        /// <returns>Ключи, соответствующие шаблону</returns>
        public virtual string[] Find(string keyPattern)
        {
            if (keyPattern == null) throw new ArgumentNullException(nameof(keyPattern));
            string[] files = Directory.GetFiles(folder, keyPattern);
            string[] ret = new string[files.Length];
            foreach (var f in files) f.Remove(f.Length - ".database".Length);
            return ret;
        }

        /// <summary>
        /// Перезаписать объект
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        /// <param name="obj">Новое содержимое объекта</param>
        public virtual void ReWriteObject(string key, object obj)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (!Exists(key)) throw new ArgumentException("Node does'nt exist");
            else
                try
                {
                    BinarySerializer.Serialize(obj, new FileStream(folder + @"\" + key + ".database", FileMode.OpenOrCreate));
                }
                catch { throw; }
        }
        /// <summary>
        /// Удалить запись по заданному ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        public virtual void DeleteObject(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!Exists(key)) throw new ArgumentException("Node does'nt exist");
            File.Delete(folder + @"\" + key + ".database");
        }
        /// <summary>
        /// Существует ли запись с заданным ключем
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Существует ли</returns>
        public virtual bool Exists(string key)
        {
            if (File.Exists(folder + @"\" + key + ".database")) return true;
            return false;
        }
    }
}