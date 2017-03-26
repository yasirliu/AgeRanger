using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DIManager
{
    public interface IDIProvider<out TConfiguration>
    {
        TConfiguration Build(params string[] configFiles);
    }
}
