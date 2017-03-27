using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AgeRanger.WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PersonController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<string> GetPersons()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet]
        public string GetUser(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void CreateUser([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void EditUser(int id, [FromBody]string value)
        {
        }

    }
}