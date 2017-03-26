using AgeRanger.DataContracts.DataBase;
using AgeRanger.DataContracts.Repositories;
using AgeRanger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Repositories.Readers
{
    public sealed class AgeGroupReaderRepository : ReaderRepositoryBase<AgeGroup>, IAgeGroupReaderRepositoryContract
    {
        public AgeGroupReaderRepository(IAgeRangerReaderDbContextContract context) : base(context) { }
    }
}
