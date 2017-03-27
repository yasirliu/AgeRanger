using AgeRanger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Dtos
{
    public class PersonAgeGroupDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public int Id { get; set; }

        public AgeGroup Group { get; set; }
    }
}
