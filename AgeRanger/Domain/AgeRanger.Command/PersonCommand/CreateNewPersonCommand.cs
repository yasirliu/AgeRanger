using AgeRanger.Command.CommandValidaters;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Command.PersonCommand
{
    public sealed class CreateNewPersonCommand : CommnadBase, IValidatableObject
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Age { get; set; }

        /// <summary>
        /// Validate properties only
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(this.FirstName, new ValidationContext(this) { MemberName = nameof(this.FirstName) }, results);
            Validator.TryValidateProperty(this.LastName, new ValidationContext(this) { MemberName = nameof(this.LastName) }, results);
            Validator.TryValidateProperty(this.Age, new ValidationContext(this) { MemberName = nameof(this.Age) }, results);
            return results;
        }
    }
}
