using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenApi.DataAccess;
using ScreenApi.DataAccess.Exceptions;
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
                return InternalServerError($"Unable to process request.  Details: {ex.ToString()}");
            }
        }

        private ObjectResult InternalServerError(string message)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }

        private ActionResult<IEnumerable<Screen>> FindScreens(string searchTerm)
        {
            try
            {
                return Ok(db.FindScreens(searchTerm));
            }
            catch (Exception ex)
            {
                return InternalServerError($"Unable to process request.  Details: {ex.ToString()}");
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
                return InternalServerError($"Unable to process request.  Details: {ex.ToString()}");
            }
        }

        // POST api/screens
        [HttpPost]
        public ActionResult Post([FromBody] Screen screen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            try
            {
                db.AddScreen(screen);
                return NoContent();
            }
            catch (AlreadyExistsException aex)
            {
                return Conflict(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError($"Unable to process request.  Details: { ex.ToString()}");
            }
        }

        // PUT api/screens/5
        [HttpPut("{id}")]
        public ActionResult Put(Guid id, [FromBody] Screen screen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            try
            {
                if (id != screen.Id)
                    screen.Id = id;

                db.UpdateScreen(screen);
                return NoContent();
            }
            catch (NotFoundException aex)
            {
                return NotFound(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError($"Unable to process request.  Details: { ex.ToString()}");
            }
        }

        // DELETE api/screens/5
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                db.DeleteScreen(id);
                return NoContent();
            }
            catch (NotFoundException aex)
            {
                return NotFound(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError($"Unable to process request.  Details: { ex.ToString()}");
            }
        }
    }
}
