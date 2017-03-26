using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DIManager
{
    /// <summary>
    /// IoC provider interface
    /// </summary>
    /// <typeparam name="TContainer">the type of IoC container</typeparam>
    public interface IDIProvider<out TContainer> : IDisposable
    {
        void Build();
        TContainer GetContainer();
    }
}
