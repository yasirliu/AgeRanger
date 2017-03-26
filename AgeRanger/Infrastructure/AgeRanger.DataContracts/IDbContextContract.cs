using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DataContracts
{
    public interface IDbContextContract : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
