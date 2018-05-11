using System;
using System.Collections.Generic;
using System.IO;
using AuxiliaryLibrary;

namespace SimpleDatabase
{
    public class Database
    {
        public string Name { get; private set; }
        string folder;

        public Database(string name, string directory)
        {
            if (!Directory.Exists(directory)) throw new ArgumentException("Can't find directory");

            folder = directory + @"\" + name;

            if (Directory.Exists(folder))
            {
                for (int i = 1; i < int.MaxValue; i++)
                    if (!Directory.Exists(folder + "_last" + i))
                    {
                        Directory.Move(folder, folder + "_last" + i);
                        break;
                    }
            }
            else Directory.CreateDirectory(folder);
        }

        public Database(string directory)
        {
            if (!Directory.Exists(directory)) throw new ArgumentException("Can't find directory");
            folder = directory;
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
                    if (category == null) File.Create(folder + @"\" + key + ".database");
                    else File.Create(folder + @"\" + category + @"\" + key + ".database");
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
                fs = new FileStream(folder + @"\" + key + ".database", FileMode.Open);
            else
                fs = new FileStream(folder + @"\" + category + @"\" + key + ".database", FileMode.Open);

            try
            { return BinarySerializer.Deserialize(fs); }
            catch { throw; }
        }
        public virtual string[] Find(string category, string keyPattern)
        {
            Directory.GetFiles(folder + @"\" + category + @"\" + keyPattern);

            string[] files;
            string name;
            if (category == null) name = folder + @"\";
            else
            {
                if (!Directory.Exists(folder + @"\" + category + @"\"))
                    throw new ArgumentNullException("Category dsoes'nt exists");
                name = folder + @"\" + category;
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
                    BinarySerializer.Serialize(obj, new FileStream(folder + @"\" + category + @"\" + key + ".database", FileMode.OpenOrCreate));
                else
                    BinarySerializer.Serialize(obj, new FileStream(folder + @"\" + key + ".database", FileMode.OpenOrCreate));
            }
            catch { throw; }
        }
        public virtual void CreateDirectory(string name)
        {
            if (Directory.Exists(folder + @"\" + name))
                throw new ArgumentException("Directory already exists");
            Directory.CreateDirectory(folder + @"\" + name);
        }
        public virtual void DeleteObject(string category, string key)
        {
            try
            { IsExists(category, key); }
            catch { throw; }
            if (category == null) File.Delete(folder + @"\" + key + ".database");
            else File.Delete(folder + @"\" + category + @"\" + key + ".database");
        }
        public virtual void DeleteCategory(string name)
        {
            if (!Directory.Exists(folder + @"\" + name)) throw new ArgumentException("Can't find directory");
            Directory.Delete(folder + @"\" + name);
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
        { return Directory.Exists(folder + @"\" + category); }

        protected void IsExists(string category, string key)
        {
            try
            {
                if (key == null) throw new ArgumentNullException("Null key");

                if (category == null)
                    if (!File.Exists(folder + @"\" + key + ".database"))
                        throw new ArgumentException("Can't find file");
                    else
                    {
                        if (!Directory.Exists(folder + @"\" + category))
                            throw new ArgumentException("Can't find category");

                        if (File.Exists(folder + @"\" + category + @"\" + key + ".database"))
                            throw new ArgumentException("Can't find file");
                    }
            }
            catch { throw; }
        }
    }
}