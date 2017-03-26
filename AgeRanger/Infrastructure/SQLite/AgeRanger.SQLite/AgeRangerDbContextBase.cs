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
    public class AgeRangerDbContextBase :DbContext
    {
        public AgeRangerDbContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Overwrite initializer with SQLite initializer
            Database.SetInitializer(new AgeRangerDbInitializer(modelBuilder));

            //Config entity
            modelBuilder.Configurations.Add(new PersonEntityConfiguration<Person>());
            modelBuilder.Configurations.Add(new AgeGroupConfiguration());
        }
    }
}
