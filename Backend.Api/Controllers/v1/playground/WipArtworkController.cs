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
    [Route("api/playground/artworks")]
    [ApiController]
    [Authorize]
    public class WipArtworkController : ControllerBase
    {
        readonly private IPlaygroundService _playgroundService;
        readonly private IWipArtworkRepository _artworkRepository;
        readonly private IMapper _mappper;

        public WipArtworkController(IPlaygroundService playgroundService,
            IWipArtworkRepository artworkRepository,
            IMapper mapper
            )
        {
            _playgroundService = playgroundService;
            _artworkRepository = artworkRepository;
            _mappper = mapper;
        }

        private string FindOwner()
        {
            return User.FindFirst("uid").Value;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<PlaygroundArtworkResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var userId = FindOwner();
            var artworks = await _artworkRepository.FindAllArtworksAsync(userId);
            var result = _mappper.Map<List<PlaygroundArtworkResponse>>(artworks);
            return Ok(result);
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(PlaygroundArtworkResponse), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var userId = FindOwner();
            var artwork = await _artworkRepository.FindByIdAsync(userId, id);
            var result = _mappper.Map<PlaygroundArtworkResponse>(artwork);
            return Ok(result);
        }


        [HttpPatch]
        public async Task<IActionResult> Update(PlaygroundArtworkRequestPatch a)
        {
            var userId = FindOwner();
            
            if (a.Description != null && a.Description != "")
            {
                await _playgroundService.UpdateArtworkByDescription(userId, a.Id, a.Description);
            }
            else if(a.Title != null && a.Title != "")
            {
                await _playgroundService.UpdateArtworkByTitle(userId, a.Id, a.Title);
            }
            return Ok();
        }

        [ProducesResponseType(typeof(int), 200)]
        [HttpPost]
        public async Task<IActionResult> Add(PlaygroundArtworkRequestPost a)
        {
            var userId = FindOwner();
            var result = await _playgroundService.AddArtwork(userId, a.Title, a.Description);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int artworkId)
        {
            var userId = FindOwner();
            await _playgroundService.DeleteArtwork(userId, artworkId);
            return Ok();
        }

    }


}
