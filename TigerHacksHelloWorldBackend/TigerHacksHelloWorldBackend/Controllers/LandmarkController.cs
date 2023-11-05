using System;
using System.Net;
using Library.DataAccess.Interfaces;
using Library.Models;
using Library.Workers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TigerHacksHelloWorldBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LandmarkController : ControllerBase
	{
		private readonly ILandmarkDataAccess dataAccess;
        private readonly IGridWorker gridWorker;

		public LandmarkController(ILandmarkDataAccess dataAccess, IGridWorker gridWorker)
		{
			this.dataAccess = dataAccess;
            this.gridWorker = gridWorker;
		}

        /// <summary>
        /// Get Landmarks
        /// </summary>
        /// <returns>landmarks or failure</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Landmark>), (int)HttpStatusCode.OK)]
        public IActionResult GetLandmarks()
        {
            try
            {
                var result = dataAccess.GetLandmarks();

                return result != null
                     ? Ok(result)
                     : BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Create Landmark
        /// </summary>
        /// <param name="name"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>Ok or failure</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public IActionResult CreateLandmark([FromQuery] string name, [FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest();

                var hash = gridWorker.GenerateHash(new Coordinate() { Latitude = latitude, Longitude = longitude });

                var result = dataAccess.UpsertLandmark(hash, name);

                return result != null
                     ? Ok(result)
                     : BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

