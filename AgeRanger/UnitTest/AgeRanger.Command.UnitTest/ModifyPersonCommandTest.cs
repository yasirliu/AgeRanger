using AgeRanger.Command.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DIManager;
using AgeRanger.Event.PersonEvent;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;

namespace AgeRanger.Command.UnitTest
{
    [TestFixture]
    public class ModifyPersonCommandTest
    {
        private IDIProvider<IContainer> iocProvider;
        private IPersonCommandHandler handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            iocProvider = new AutofacProvider($@"{ AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.reader.json",
                       $@"{AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.writer.json");
            iocProvider.Build();
            handler = iocProvider.GetContainer().Resolve<IPersonCommandHandler>();
        }

        [Test]
        public void Update_Person_Invalid_Age()
        {
            var person = new ModifyExistingPersonCommand() { Id = 1, FirstName = "1", LastName = "2", Age = -1 };
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(person, new ValidationContext(person), result);
            IsTrue(result.First().MemberNames.First() == nameof(person.Age));
        }

        [Test]
        public void Update_Person_Invalid_FirstName()
        {
            var person = new ModifyExistingPersonCommand() { Id = 1, LastName = "2", Age = 1 };
            var result = person.Validate(new ValidationContext(person));
            IsTrue(result.First().MemberNames.First() == nameof(person.FirstName));
        }

        [Test]
        public void Update_Person_Invalid_Id()
        {
            var person = new ModifyExistingPersonCommand() { Id = 0, LastName = "2", Age = 1, FirstName="2" };
            var result = person.Validate(new ValidationContext(person));
            IsTrue(result.First().MemberNames.First() == nameof(person.Id));
        }

        [Test]
        public async Task Update_Person_Valid_Entity()
        {
            var person = new ModifyExistingPersonCommand() { Id=1, FirstName = "Adam111111", LastName = "Liu11111", Age = 1000 };
            await handler.HandleAsync(person);
        }

        [Test]
        public void Update_Person_InValid_Entity()
        {
            ThrowsAsync<PersonNotCreatedEvent>(async delegate
            {
                var person = new CreateNewPersonCommand() { Age = 100 };
                await handler.HandleAsync(person);
            });
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            iocProvider.Dispose();
            iocProvider = null;
        }
    }
}
