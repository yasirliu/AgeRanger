using AgeRanger.DataContracts.DataBase;
using AgeRanger.Domain.Entities;
using AgeRanger.ModelConfiguration;
using EntityFramework.Toolkit;
using EntityFramework.Toolkit.Core;
using EntityFramework.Toolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.SQLite
{
    /// <summary>
    /// Database context under EntityFramework.Toolkit
    /// </summary>
    public class AgeRangerDbContext : AgeRangerDbContextBase, IAgeRangerReaderDbContextContract
    {
        public AgeRangerDbContext()
            : base("name=AgeRangerDB")
        {
        }
    }
}
