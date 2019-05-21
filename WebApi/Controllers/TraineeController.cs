using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE.MainObjects;

namespace WebApi.Controllers
{
    [Route("api/trainee")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        IEnumerable<Trainee> trainees = BL.FactoryBl.GetObject.AllTrainees;
        // GET: api/Trainee
        [HttpGet("all")]
        public async Task<ActionResult< IEnumerable<Trainee>>> Get()
        {
            return trainees.ToList();
        }

        // GET: api/Trainee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainee>> Get(int id)
        {
            return trainees.FirstOrDefault(x=>x.Id == id);
        }

        //// POST: api/Trainee
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Trainee/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
