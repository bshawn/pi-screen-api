using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScreenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        // GET api/screens
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "screen1", "screen2" };
        }

        // GET api/screens/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/screens
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/screens/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/screens/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
