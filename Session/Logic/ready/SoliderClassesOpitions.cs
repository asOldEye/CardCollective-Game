using System;
using System.Collections.Generic;
using AuxiliaryLibrary;

namespace Session
{
    public class SoliderClassesOpitions
    {
        List<Pair<string, List<Pair<string, int>>>> critChance;

        public SoliderClassesOpitions(List<Pair<string, List<Pair<string, int>>>> critChance)
        {
            if (critChance == null) throw new ArgumentNullException("Null critChance");
            try
            {
                foreach (var f in critChance)
                    ClassCheck(f);
            }
            catch { throw; }
            this.critChance = new List<Pair<string, List<Pair<string, int>>>>(critChance);
        }

        void ClassCheck(Pair<string, List<Pair<string, int>>> cl)
        {
            if (cl == null) throw new ArgumentNullException("Null class opitions");
            if (cl.Obj1 == null || cl.Obj1.Length < 1) throw new ArgumentException("Wrong name excepted");
        }

        public int CritChance(string attacking, string target)
        {
            foreach (var f in critChance)
            {
                if (f.Obj1 == attacking)
                    foreach (var targ in f.Obj2)
                        if (targ.Obj1 == target)
                            return targ.Obj2;
                return -1;
            }
            return -1;
        }
    }
}