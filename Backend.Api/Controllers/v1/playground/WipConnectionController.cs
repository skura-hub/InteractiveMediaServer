using AutoMapper;
using Backend.Application.DTOs.Wip;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers.v1
{
    [Route("api/playground/connections")]
    [ApiController]
    [Authorize]
    public class WipConnectionController : ControllerBase
    {
        readonly private IPlaygroundService _playgroundService;
        readonly private IWipConnectionRepository _wipConnectionRepository;
        readonly private IMapper _mapper;

        public WipConnectionController(IPlaygroundService playgroundService, 
            IWipConnectionRepository wipConnectionRepository,
            IMapper mapper)
        {
            _playgroundService = playgroundService;
            _wipConnectionRepository = wipConnectionRepository;
            _mapper = mapper;
        }

        private string FindOwner()
        {
            return User.FindFirst("uid").Value;
        }

        [HttpPatch]
        public async Task<IActionResult> Update(PlaygroundConnectionRequestPatch c)
        {
            var userId = FindOwner();
            if(c.IsDefault.HasValue)
            {
                
                await _playgroundService.UpdateConnectionByIsDefault(userId, c.ArtworkId, c.Id, c.IsDefault.Value);
            }
            else if (c.LongName != null && c.LongName != "")
            {

                await _playgroundService.UpdateConnectionByLongName(userId, c.ArtworkId, c.Id, c.LongName);
            }
            else if (c.ShortName != null && c.ShortName != "")
            {

                await _playgroundService.UpdateConnectionByShortName(userId, c.ArtworkId, c.Id, c.ShortName);
            }
            return Ok();
        }

        [ProducesResponseType(typeof(string), 200)]
        [HttpPost]
        public async Task<IActionResult> Add(PlaygroundConnectionRequestPost c)
        {
            if (c.ShortName == null || c.ShortName == "") BadRequest();
            var userId = FindOwner();
            var result = await _playgroundService.AddConnection(userId, c.ArtworkId, c.NodeStartingId, c.NodeEndingId, c.IsDefault, c.ShortName, "");
            return Ok(result);
        }

        [ProducesResponseType(typeof(PlaygroundConnectionResponse), 200)]
        [HttpGet("artworkId, connectionId")]
        public async Task<IActionResult> Get(int artworkId, int connectionId)
        {
            var userId = FindOwner();
            var connection = await _wipConnectionRepository.FindByIdAsync(userId, artworkId, connectionId);
            var result = _mapper.Map<PlaygroundConnectionResponse>(connection);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int artworkId, int connectionId)
        {
            var userId = FindOwner();
            await _playgroundService.DeleteConnection(userId, artworkId, connectionId);
            return Ok();
        }

    }


}
