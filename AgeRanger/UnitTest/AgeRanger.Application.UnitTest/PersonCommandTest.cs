using AgeRanger.Application.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DataContracts.Repositories;
using AgeRanger.DIManager;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Application.UnitTest
{
    [TestFixture]
    public class PersonCommandTest
    {
        private IDIProvider<IContainer> iocProvider;
        private IPersonCommandServiceContract handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            iocProvider = new AutofacProvider($@"{ AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.reader.json",
                            $@"{AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.writer.json");
            iocProvider.Build();
            handler = iocProvider.GetContainer().Resolve<IPersonCommandServiceContract>();
        }


        [Test]
        [TestCase("11234", "2345", 56)]
        public void Person_Create_Test(string firstName, string lastName, int age)
        {
            handler.Apply(new CreateNewPersonCommand()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            });
        }


        [Test]
        [TestCase(1, "11234", "2345", 56)]
        public void Person_Modify_Test(int id, string firstName, string lastName, int age)
        {
            handler.Apply(new CreateNewPersonCommand()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            });

            handler.Dispose();
            handler = iocProvider.GetContainer().Resolve<IPersonCommandServiceContract>();

            handler.Apply(new ModifyExistingPersonCommand()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Age = age
            });
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
