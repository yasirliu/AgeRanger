using AgeRanger.DataContracts.DataBase;
using AgeRanger.Domain.Entities;
using AgeRanger.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.SQLite
{
    public class AgeRangerWriterDbContext : AgeRangerDbContextBase, IAgeRangerWriterDbContextContract
    {
        public AgeRangerWriterDbContext()
            : base("name=AgeRangerDBWriter")
        {
        }
    }
}
