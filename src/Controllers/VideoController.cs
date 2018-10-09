using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenApi.DataAccess;
using ScreenApi.DataAccess.Exceptions;
using ScreenApi.Models;

namespace ScreenApi.Controllers
{
    [Route("api/screens/{screenId}/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        // TODO: Load VideoRepository using Dependency Injection
        // TODO: Centralize exception handling (handle known exceptions)
        private const string DATA_DIR = "./Data/Videos";
        private IVideoRepository db = new VideoRepository(DATA_DIR);

        // GET api/screens/5/video
        [HttpGet]
        public IActionResult Get(Guid screenId)
        {
            try
            {
                var fs = db.GetVideo(screenId);
                if (fs == null)
                    return NotFound("The specified screen has no video assigned");

                return new FileStreamResult(fs, new MediaTypeHeaderValue("video/mp4").MediaType);
            }
            catch (Exception ex)
            {
                return InternalServerError($"Unable to process request.  Details: { ex.ToString()}");
            }
        }

        private ObjectResult InternalServerError(string message)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }

        // // PUT api/screens/5/video
        // [HttpPut]
        // public ActionResult Put(Guid screenId, [FromBody] Screen screen)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState.ValidationState);

        //     try
        //     {
        //         if (id != screen.Id)
        //             screen.Id = id;

        //         db.UpdateScreen(screen);
        //         return NoContent();
        //     }
        //     catch (NotFoundException aex)
        //     {
        //         return NotFound(aex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return InternalServerError($"Unable to process request.  Details: { ex.ToString()}");
        //     }
        // }
    }
}
