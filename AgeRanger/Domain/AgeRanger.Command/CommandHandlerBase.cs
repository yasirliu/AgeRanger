using AgeRanger.DataContracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Command
{
    public abstract class CommandHandlerBase<TEntity> : IDisposable
    {
        internal IEntityWriterContract<TEntity> _repository;
        public CommandHandlerBase(IEntityWriterContract<TEntity> repository)
        {
            if (ReferenceEquals(null, repository))
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;
        }

        /// <summary>
        /// Command To Entity
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        internal abstract TEntity CommandMapper<TCommand>(TCommand command) where TCommand : CommnadBase;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
