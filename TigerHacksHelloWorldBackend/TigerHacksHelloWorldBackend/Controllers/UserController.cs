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
    public class UserController : ControllerBase
    {
        private readonly IUserWorker worker;
        private readonly IUserDataAccess dataAccess;

		public UserController(IUserWorker worker, IUserDataAccess dataAccess)
		{
            this.worker = worker;
            this.dataAccess = dataAccess;
		}

        /// <summary>
        /// login user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>user or failure</returns>
        [HttpGet]
        [Route("Login")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
                    return BadRequest();

                var result = worker.Login(username, password);

                return result != null
                     ? Ok(result)
                     : BadRequest();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Ok or failure</returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public IActionResult UpdateUser([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
                    return BadRequest();

                var result = worker.UpdateUserPassword(username, password);

                return result != null || result.IsSuccessful
                     ? Ok(result)
                     : BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// create user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>user or failure</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public IActionResult CreateUser([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
                    return BadRequest();

                var result = worker.CreateUser(username, password);

                return result != null || result.IsSuccessful
                     ? Ok(result)
                     : BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

