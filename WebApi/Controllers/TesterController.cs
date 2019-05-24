using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE.MainObjects;
using Newtonsoft.Json.Linq;
using BE.Routes;

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
        [HttpPost("add")]
        public async Task<ActionResult<string>> Post([FromBody] Object v)
        {
            try
            {
                Tester tester = new Tester();
                var value = (Newtonsoft.Json.Linq.JObject)v;
                if(!BE.Tools.CheckID_IL(uint.Parse( (String)value["Id"])))
                    return "Invalid Id!";

                //   BL.FactoryBl.GetObject.AddTester(value);
                foreach (var prop in tester.GetType().GetProperties())
                {
                    if ((value[prop.Name]) is JArray)
                    {
                        var list = (value[prop.Name]).ToObject<List<BE.LicenseType>>();
                        prop.SetValue(tester, list);
                    }
                    else if(prop.Name == "Address")
                    {
                        prop.SetValue(tester, new Address((string)(value[prop.Name])));
                    }else{
                        prop.SetValue(tester, Convert.ChangeType((value[prop.Name]), prop.PropertyType));
                    }
                }
                tester.Schedule = new BE.WeekSchedule();
                BL.FactoryBl.GetObject.AddTester(tester);
                return "Successfully added " + value["FirstName"] + " " + value["LastName"]+".";
            }catch(Exception ex)
            {
                return ex.Message;
            }
            
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
