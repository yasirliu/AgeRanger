using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Domain.ServiceBus.Interfaces
{
    public interface ICommand
    {

        /// <summary>
        /// Command Id
        /// </summary>
        Guid CommandId { get; }
    }
}
