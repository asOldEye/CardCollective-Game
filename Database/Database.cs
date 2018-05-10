using System;
using System.Collections.Generic;
using System.IO;
using AuxiliaryLibrary;

namespace SimpleDatabase
{
    public class Database
    {
        public string Folder { get; private set; }

        public string Name { get; private set; }

        public Database(string name, string directory)
        {
            if (!Directory.Exists(directory)) throw new ArgumentException("Can't find directory");

            Folder = directory + @"\" + name;

            if (Directory.Exists(Folder))
            {
                for (int i = 1; i < int.MaxValue; i++)
                    if (!Directory.Exists(Folder + "_last" + i))
                    {
                        Directory.Move(Folder, Folder + "_last" + i);
                        break;
                    }
            }
            else Directory.CreateDirectory(Folder);
        }

        public Database(string directory)
        {
            if (!Directory.Exists(directory)) throw new ArgumentException("Can't find directory");
            Folder = directory;
        }

        public virtual void WriteObject(string category, string key, object obj)
        {
            if (obj == null) throw new ArgumentNullException("Null obj");
            try
            { IsExists(category, key); }
            catch (ArgumentException e)
            {
                if (e.Message == "Can't find file")
                {
                    FileStream file = null;
                    if (category == null) File.Create(Folder + @"\" + key + ".database");
                    else File.Create(Folder + @"\" + category + @"\" + key + ".database");
                    try
                    { BinarySerializer.Serialize(obj, file); }
                    catch { throw; }
                }
                else throw;
            }
            catch { throw; }
        }
        public virtual object ReadObject(string category, string key)
        {
            try
            { IsExists(category, key); }
            catch { throw; }

            FileStream fs;
            if (category == null)
                fs = new FileStream(Folder + @"\" + key + ".database", FileMode.Open);
            else
                fs = new FileStream(Folder + @"\" + category + @"\" + key + ".database", FileMode.Open);

            try
            { return BinarySerializer.Deserialize(fs); }
            catch { throw; }
        }
        public virtual string[] Find(string category, string keyPattern)
        {
            Directory.GetFiles(Folder + @"\" + category + @"\" + keyPattern);

            string[] files;
            string name;
            if (category == null) name = Folder + @"\";
            else
            {
                if (!Directory.Exists(Folder + @"\" + category + @"\"))
                    throw new ArgumentNullException("Category dsoes'nt exists");
                name = Folder + @"\" + category;
            }
            files = Directory.GetFiles(name, keyPattern);

            foreach (var f in files)
                f.Replace(name, "");

            return files;
        }

        public virtual void ReWriteObject(string category, string key, object obj)
        {
            if (obj == null) throw new ArgumentNullException("Null obj");
            try
            { IsExists(category, key); }
            catch { throw; }

            try
            {
                if (category != null)
                    BinarySerializer.Serialize(obj, new FileStream(Folder + @"\" + category + @"\" + key + ".database", FileMode.OpenOrCreate));
                else
                    BinarySerializer.Serialize(obj, new FileStream(Folder + @"\" + key + ".database", FileMode.OpenOrCreate));
            }
            catch { throw; }
        }
        public virtual void CreateDirectory(string name)
        {
            if (Directory.Exists(Folder + @"\" + name))
                throw new ArgumentException("Directory already exists");
            Directory.CreateDirectory(Folder + @"\" + name);
        }
        public virtual void DeleteObject(string category, string key)
        {
            try
            { IsExists(category, key); }
            catch { throw; }
            if (category == null) File.Delete(Folder + @"\" + key + ".database");
            else File.Delete(Folder + @"\" + category + @"\" + key + ".database");
        }
        public virtual void DeleteCategory(string name)
        {
            if (!Directory.Exists(Folder + @"\" + name)) throw new ArgumentException("Can't find directory");
            Directory.Delete(Folder + @"\" + name);
        }
        public virtual bool Exists(string category, string key)
        {
            try
            {
                IsExists(category, key);
                return true;
            }
            catch (ArgumentException) { return false; }
            catch { throw; }
        }
        public virtual bool Exists(string category)
        { return Directory.Exists(Folder + @"\" + category); }

        protected void IsExists(string category, string key)
        {
            try
            {
                if (key == null) throw new ArgumentNullException("Null key");

                if (category == null)
                    if (!File.Exists(Folder + @"\" + key + ".database"))
                        throw new ArgumentException("Can't find file");
                    else
                    {
                        if (!Directory.Exists(Folder + @"\" + category))
                            throw new ArgumentException("Can't find category");

                        if (File.Exists(Folder + @"\" + category + @"\" + key + ".database"))
                            throw new ArgumentException("Can't find file");
                    }
            }
            catch { throw; }
        }
    }
}