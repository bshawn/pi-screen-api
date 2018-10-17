using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenApi.DataAccess;
using ScreenApi.DataAccess.Exceptions;
using ScreenApi.Models;

namespace ScreenApi.Controllers
{
    [Route("api/screens/{screenId}/[controller]")]
    [ApiController]
    [Consumes("multipart/form-data")]
    public class VideoController : ControllerBase
    {
        // TODO: Load VideoRepository using Dependency Injection
        // TODO: Centralize exception handling (handle known exceptions)
        private const string DATA_DIR = "./Data/Videos";
        private IVideoRepository db = new VideoRepository(DATA_DIR);
        private const string SCREEN_DATA_FILE = "./Data/screens.json";
        private IScreenRepository screenDb = new ScreenRepository(SCREEN_DATA_FILE);

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

        [HttpPut]
        public async Task<IActionResult> Put(Guid screenId, IFormFile file)
        {
            try
            {
                var screen = screenDb.GetScreenById(screenId);
                if (screen == null)
                    throw new NotFoundException("The specified screen ID does not exist");

                var stream = file.OpenReadStream();
                await db.UpdateVideo(screenId, stream);
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError($"Unable to process request.  Details: { ex.ToString() }");
            }

            return NoContent();
        }
    }
}
