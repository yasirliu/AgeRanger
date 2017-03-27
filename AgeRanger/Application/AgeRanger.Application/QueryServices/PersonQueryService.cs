using AgeRanger.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Domain.Entities;
using AgeRanger.Dtos;
using AgeRanger.Application.Contracts;
using AgeRanger.DataContracts.Repositories;
using AutoMapper;
using AgeRanger.Dtos.QueryExpressionMappers;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Data.Entity;

namespace AgeRanger.Application.QueryServices
{
    public class PersonQueryService : IPersonQueryServiceContract
    {
        internal IPersonReaderRepositoryContract _personRepo;
        internal IAgeGroupReaderRepositoryContract _ageGroupRepo;
        public PersonQueryService(
            IPersonReaderRepositoryContract personRepo,
            IAgeGroupReaderRepositoryContract ageGroupRepo)
        {
            _personRepo = personRepo;
            _ageGroupRepo = ageGroupRepo;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _personRepo.Dispose();
                    _ageGroupRepo.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        public async Task<IEnumerable<PersonAgeGroupDto>> Query(
            string filter = null,
            string orderBy = null,
            int? pageIndex = default(int?),
            int? pageCount = default(int?),
            params string[] includeProperties)
        {
            var expressionFilter = DtoExpressionMapper.Convert<PersonAgeGroupDto, bool>(filter);
            var personFilter = DtoExpressionMapper.Convert<PersonAgeGroupDto, Person, bool>(expressionFilter);
            var persons = await _personRepo.Query(personFilter, null, pageIndex, pageCount, includeProperties).ToListAsync();

            var personGroup = from person in persons
                              select new PersonAgeGroupDto
                              {
                                  Id = person.Id,
                                  Age = person.Age,
                                  FirstName = person.FirstName,
                                  LastName = person.LastName,
                                  Group = _ageGroupRepo.Query(
                                      ag => person.Age >= (ag.MinAge ?? 0) && person.Age < (ag.MaxAge ?? int.MaxValue),
                                  null, pageIndex, pageCount, includeProperties).FirstOrDefault()
                              };

            return await Task.FromResult(personGroup);
        }

        public async Task<PersonAgeGroupDto> GetById(int Id)
        {
            return await this.Query($"Id = {Id}").ContinueWith((list)=> {
                return list.Result.FirstOrDefault();
            });
        }
        #endregion
    }
}
