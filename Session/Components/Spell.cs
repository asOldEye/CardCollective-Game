using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    public class Spell : Component
    {
        public List<Modifier> Modifiers { get; }

        public Spell(List<Modifier> modifiers)
        {
            if (modifiers == null) throw new ArgumentNullException(nameof(modifiers));
            if (modifiers.Count < 1)
                throw new ArgumentException("When spell is initialized, there must be at least one modifier");
            Modifiers = modifiers;
        }

        [Modified]
        public void Use(Container container)
        {
            if(container == null) throw new ArgumentNullException(nameof(container));
            try
            { Use(new List<Container> { container }); }
            catch { throw; }
        }
        [Modified]
        public void Use(List<Container> containers)
        {
            if (containers == null) throw new ArgumentNullException(nameof(containers));

            foreach(var mod in Modifiers)
                foreach(var cont in containers)
                {
                    if (mod is DurableModifier)
                        (mod as DurableModifier).AddModified(cont);
                }
        }
    }
}
