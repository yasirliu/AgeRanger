using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Command
{
    public abstract class CommnadBase : ICommand
    {
        public string CommandId { get {
                return $"{this.GetType().Name}-{DateTime.Now.Ticks}";
            } }

        public byte[] CommandVersion { get; set; }
    }
}
