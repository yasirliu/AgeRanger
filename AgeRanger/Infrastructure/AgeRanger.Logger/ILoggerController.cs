using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Logger
{
    public interface ILoggerController<out TCategory>
    {
         ILogger<TCategory> Logger { get; }
    }
}
