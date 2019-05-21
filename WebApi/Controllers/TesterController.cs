﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE.MainObjects;


namespace WebApi.Controllers
{
    /// <summary>
    /// Get Testers API
    ///
    /// </summary>
    [Route("api/tester")]
    [ApiController]
    public class TesterController : ControllerBase
    {
        IEnumerable<Tester> testers = BL.FactoryBl.GetObject.AllTesters;
        // GET: api/Tester
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Tester>>> Get()
        {
            return testers.ToList();
        }

        // GET: api/Tester/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tester>> Get(int id)
        {
            return testers.FirstOrDefault(x=>x.Id==id);
        }

        // POST: api/Tester
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Tester/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}