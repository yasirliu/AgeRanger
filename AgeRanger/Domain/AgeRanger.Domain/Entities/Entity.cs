using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Domain.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
