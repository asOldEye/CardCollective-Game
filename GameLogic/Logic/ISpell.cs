using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    interface ISpell<T> where T : Modifier
    {
        List<T> modifiers
        { get; }

        void Cast(IModifiedObject<T> modifiedObject);


    }
}
