using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DataContracts
{
    public interface IEntityWriterContract<TEntity> : IDisposable
    {
        void Delete(int? Id);
        void Create(TEntity entity);
        void Update(TEntity entity, byte[] rowVersion = null);
        void Commit();
        Task CommitAsync();
    }
}
