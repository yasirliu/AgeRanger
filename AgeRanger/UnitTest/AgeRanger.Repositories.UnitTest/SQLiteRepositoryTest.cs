using AgeRanger.Domain.Entities;
using AgeRanger.Repositories;
using AgeRanger.SQLite;
using NUnit.Framework;
using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EntityFramework.Toolkit.Core;
using EntityFramework.Toolkit;
using static NUnit.Framework.Assert;
using AgeRanger.DataContracts.Repositories;
using AgeRanger.DIManager;

namespace AgeRanger.Repositories.UnitTest
{
    [TestFixture]
    public class SQLiteRepositoryTest
    {
        private IAgeGroupReaderRepositoryContract agRepo;
        private IPersonReaderRepositoryContract pRepo;
        private IAgeGroupWriterRepositoryContract agWRepo;
        private IPersonWriterRepositoryContract pWRepo;
        private IDIProvider<IContainer> iocProvider;
        private IContainer container;

        [OneTimeSetUp]
        public void SetUp()
        {
            iocProvider = new AutofacProvider();
            container = iocProvider
                .Build($@"{ AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.reader.json",
                            $@"{AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.writer.json");
            agRepo = container.Resolve<IAgeGroupReaderRepositoryContract>();
            pRepo = container.Resolve<IPersonReaderRepositoryContract>();

            agWRepo = container.Resolve<IAgeGroupWriterRepositoryContract>();
            pWRepo = container.Resolve<IPersonWriterRepositoryContract>();

        }

        [Test]
        public void Get_AgeGroup_All()
        {
            var allAgeGroup = agRepo.Query();
        }

        [Test]
        public void Get_Person_All()
        {
            var allPersons = pRepo.Query();
        }

        [Test]
        public void Get_Person_AgeEqual4()
        {
            var persons = pRepo.Query(person => person.Age == 4);
        }

        [Test]
        public void Get_Person_AgeEqual3_OrderByIDDesceding()
        {
            var persons = pRepo.Query(person => person.Age == 3,
                person => person.OrderByDescending(p => p.Id));
        }

        [Test]
        public void Get_Person_AgeEqual3_OrderByIDDesceding_Page2_PageCount5()
        {
            var persons = pRepo.Query(person => person.Age == 3,
                person => person.OrderByDescending(p => p.Id),
                2,5);
        }


        [Test]
        public async Task Insert_Person()
        {
            var expect = new Person { FirstName = "A", LastName = "B", Age = 3 };
            pWRepo.Create(new Person { FirstName = "A", LastName = "B", Age = 3 });
            await pWRepo.CommitAsync();
        }

        [Test]
        public async Task Update_Person()
        {
            var expect = new Person { Id = 15, FirstName = "AA", LastName = "BB", Age = 4 };
            pWRepo.Update(expect);
            await pWRepo.CommitAsync();
        }

        [Test]
        public async Task Delete_Person()
        {
            pWRepo.Delete(1);
            await pWRepo.CommitAsync();
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            container.Dispose();
            container = null;
        }
    }
}
