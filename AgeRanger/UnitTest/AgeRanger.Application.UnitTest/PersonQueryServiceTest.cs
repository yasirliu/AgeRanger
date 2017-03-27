using AgeRanger.Application.Contracts;
using AgeRanger.Command.Contracts;
using AgeRanger.DIManager;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DataContracts.Repositories;
using static NUnit.Framework.Assert;

namespace AgeRanger.Application.UnitTest
{
    [TestFixture]
    public class PersonQueryServiceTest
    {
        private IDIProvider<IContainer> iocProvider;
        private IPersonQueryServiceContract handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            iocProvider = new AutofacProvider($@"{ AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.reader.json",
                            $@"{AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.writer.json");
            iocProvider.Build();
            handler = iocProvider.GetContainer().Resolve<IPersonQueryServiceContract>();
        }

        [Test]
        [TestCase(50)]
        public async Task Person_Query_All(int range)
        {
            for (int i = 0; i < range; i++)
            {
                var person = new CreateNewPersonCommand() { FirstName = "Adam", LastName = "Liu", Age = new Random().Next(10) };
                iocProvider.GetContainer().Resolve<IPersonCommandHandler>().Handle(person);
            }
            var result = await handler.Query();
            AreEqual(result.Count(), range);
        }

        [Test]
        [TestCase(2)]
        public async Task Person_Query_Toddler(int range)
        {
            for (int i = 0; i < 50; i++)
            {
                var person = new CreateNewPersonCommand() { FirstName = "Adam", LastName = "Liu", Age = new Random().Next(range) };
                iocProvider.GetContainer().Resolve<IPersonCommandHandler>().Handle(person);
            }
            var result = await handler.Query("Age < 2");
            IsTrue(result.Count() > 0);
            AreEqual(result.FirstOrDefault().Group.Description, "Toddler");
        }

        [Test]
        [TestCase(10000)]
        public async Task Person_Query_KauriTree(int range)
        {
            for (int i = 0; i < 50; i++)
            {
                var person = new CreateNewPersonCommand() { FirstName = "Adam", LastName = "Liu", Age = new Random().Next(range) };
                iocProvider.GetContainer().Resolve<IPersonCommandHandler>().Handle(person);
            }
            var result = await handler.Query("Age >= 4999");
            IsTrue(result.Count() > 0);
            AreEqual(result.FirstOrDefault().Group.Description, "Kauri tree");
        }

        [Test]
        [TestCase(1)]
        public async Task Person_Query_GetOne(int id)
        {
            for (int i = 0; i < 1; i++)
            {
                var person = new CreateNewPersonCommand() { FirstName = "Adam", LastName = "Liu", Age = new Random().Next(3) };
                iocProvider.GetContainer().Resolve<IPersonCommandHandler>().Handle(person);
            }
            var result = await handler.GetById(id);
            AreEqual(result.Id, 1);
        }


        [TearDown]
        public void Dispose()
        {
            iocProvider.GetContainer().Resolve<IPersonWriterRepositoryContract>().Delete(null);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            iocProvider.Dispose();
            iocProvider = null;
        }
    }
}
