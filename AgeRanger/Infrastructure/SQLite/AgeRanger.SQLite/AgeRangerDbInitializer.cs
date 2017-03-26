using AgeRanger.DataContracts.DataBase;
using AgeRanger.DataSeeds;
using AgeRanger.Domain.Entities;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.SQLite
{
    internal sealed class AgeRangerDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<AgeRangerDbContext>, IAgeRangerDbInitializerContract
    {
        public AgeRangerDbInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
        { }

        protected override void Seed(AgeRangerDbContext context)
        {
            context.Set<AgeGroup>().AddRange(new AgeGroupDataSeed().GetAll());
        }
    }
}
