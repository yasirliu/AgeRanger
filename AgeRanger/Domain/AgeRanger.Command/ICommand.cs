using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Command
{
    public interface ICommand
    {

        /// <summary>
        /// Command Id
        /// </summary>
        string CommandId { get; }
    }
}
