using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ScreenApi.DataAccess;
using ScreenApi.Models;

namespace ScreenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        // TODO: Load ScreenRepository using Dependency Injection
        // TODO: Centralize exception handling (handle known exceptions)
        private const string DATA_FILE = "./Data/screens.json";
        private IScreenRepository db = new ScreenRepository(DATA_FILE);

        // GET api/screens
        [HttpGet]
        public ActionResult<IEnumerable<Screen>> Get(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
                return FindScreens(searchTerm);

            return GetAllScreens();
        }

        private ActionResult<IEnumerable<Screen>> GetAllScreens()
        {
            try
            {
                return Ok(db.GetAllScreens());
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Unable to process request.  Details: {ex.ToString()}");
            }
        }

        private ActionResult<IEnumerable<Screen>> FindScreens(string searchTerm)
        {
            try
            {
                return Ok(db.FindScreens(searchTerm));
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Unable to process request.  Details: {ex.ToString()}");
            }
        }

        // GET api/screens/5
        [HttpGet("{id}")]
        public ActionResult<Screen> Get(Guid id)
        {
            try
            {
                var screen = db.GetScreenById(id);
                if (screen == null)
                    return NotFound("The requested screen could not be found");

                return Ok(screen);
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Unable to process request. Details: {ex.ToString()}");
            }
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
