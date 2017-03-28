using AgeRanger.DataContracts.Repositories;
using AgeRanger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.DataContracts;
using AgeRanger.DataContracts.DataBase;

namespace AgeRanger.Repositories.Writers
{
    public sealed class AgeGroupWriterRepository : WriterRepository<AgeGroup>, IAgeGroupWriterRepositoryContract
    {
        public AgeGroupWriterRepository(IAgeRangerWriterDbContextContract context) : base(context)
        {
        }
    }
}
