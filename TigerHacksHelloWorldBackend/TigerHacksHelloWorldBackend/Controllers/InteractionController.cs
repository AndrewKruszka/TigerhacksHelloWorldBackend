using System;
using Library.Models;
using System.Net;
using Library.Workers;
using Library.Workers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TigerHacksHelloWorldBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InteractionController : ControllerBase
	{
		private readonly IInteractionWorker worker;

		public InteractionController(IInteractionWorker worker)
		{
			this.worker = worker;
		}

        /// <summary>
        /// Get interaction
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="userId"></param>
        /// <returns>interaction or failure</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Interaction), (int)HttpStatusCode.OK)]
        public IActionResult GetInteraction([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] Int64 userId)
        {
            try
            {
                var result = worker.GetInteraction(latitude, longitude, userId);

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

