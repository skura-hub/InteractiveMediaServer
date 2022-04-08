using Backend.Application.DTOs.ApplicationUser;
using Backend.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers.v1
{

    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class ApplicationUserController : ControllerBase
    {
        readonly private IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserController(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        /// <summary>
        /// Get public information about user
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PublicUserResponse), 200)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _applicationUserRepository.GetByIdAsync(id);
            return Ok(user);
        }
        /// <summary>
        /// Find all users
        /// ?username -> first characters of username
        /// </summary>
        /// <param name="username"></param>
        [HttpGet()]
        //[AllowAnonymous]
        [ProducesResponseType(typeof(List<PublicUserResponse>), 200)]
        public async Task<IActionResult> GetByParametrs(string? username)
        {
            var user = await _applicationUserRepository.FindAllAsync(username);
            return Ok(user);
        }
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<PublicUserResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var user = await _applicationUserRepository.GetAllAsync();
            return Ok(user);
        }
    }
}
