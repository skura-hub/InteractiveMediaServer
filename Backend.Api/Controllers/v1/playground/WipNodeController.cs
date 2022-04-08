using AutoMapper;
using Backend.Application.DTOs.Wip;
using Backend.Application.Enums;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities.Catalog;
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
    [Route("api/playground/nodes")]
    [ApiController]
    [Authorize]
    public class WipNodeController : ControllerBase
    {
        readonly private IPlaygroundService _playgroundService;
        readonly private IWipNodeRepository _wipNodeRepository;
        readonly private IWipMediaRepository _wipMediaRepository;
        readonly private IMapper _mapper;

        public WipNodeController(IPlaygroundService playgroundService, IWipNodeRepository wipNodeRepository, 
            IWipMediaRepository wipMediaRepository,
            IMapper mapper)
        {
            _playgroundService = playgroundService;
            _wipNodeRepository = wipNodeRepository;
            _wipMediaRepository = wipMediaRepository;
            _mapper = mapper;
        }

        private string FindOwner()
        {
            return User.FindFirst("uid").Value;
        }

        [ProducesResponseType(typeof(PlaygroundNodeResponse), 200)]
        [HttpGet("artworkId, nodeId")]
        public async Task<IActionResult> Get(int artworkId, int nodeId)
        {
            var userId = FindOwner();
            /*
            var node = await _wipNodeRepository.FindByIdAsync(userId, artworkId, nodeId);
            var result = _mapper.Map<PlaygroundNodeResponse>(node);
            if(node.FkeyWipMedia.HasValue)
            {
                var media = await _wipMediaRepository.FindByIdAsync(userId, node.FkeyWipMedia.Value);
                result.MediaPath = media.Path;
                result.MediaType = ((MediaTypes)media.FkeyMediaType).ToString();
            }
            */
            var nodeWithMedia = await _wipNodeRepository.FindByIdWithMediaAsync(userId, artworkId, nodeId);
            return Ok(nodeWithMedia);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(PlaygroundNodeRequestPatch n)
        {
            var userId = FindOwner();
            if(n.Name != null && n.Name != "")
            {
                
                await _playgroundService.UpdateNodeByName(userId, n.ArtworkId, n.Id, n.Name);
            }
            else if (n.X.HasValue && n.Y.HasValue)
            {

                await _playgroundService.UpdateNodeByPosition(userId, n.ArtworkId, n.Id, n.X.Value, n.Y.Value);
            }
            else if (n.MediaId.HasValue)
            {

                await _playgroundService.UpdateNodeByMedia(userId, n.ArtworkId, n.Id, n.MediaId.Value);
            }
            return Ok();
        }

        [ProducesResponseType(typeof(string), 200)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PlaygroundNodeRequestPost n)
        {
            if (n.Name == null) n.Name = "";
            var userId = FindOwner();
            var response = await _playgroundService.AddNode(userId, n.ArtworkId, n.MediaId, n.X, n.Y, n.Name);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int artworkId, int nodeId)
        {
            if (nodeId == 1) return BadRequest();
            var userId = FindOwner();
            await _playgroundService.DeleteNode(userId, artworkId, nodeId);
            return Ok();
        }

    }


}
