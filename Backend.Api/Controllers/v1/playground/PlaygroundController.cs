using Backend.Application.DTOs.ArtworkResponse;
using Backend.Application.Interfaces;
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
    [Route("api/playgrounds")]
    [ApiController]
    [Authorize]
    public class PlaygroundController : ControllerBase
    {
        readonly private IPlaygroundService _playgroundService;

        public PlaygroundController(IPlaygroundService playgroundService)
        {
            _playgroundService = playgroundService;
        }

        private string FindOwner()
        {
            return User.FindFirst("uid").Value;
        }
        /// <summary>
        /// Zwraca tablicę w json gotową do przekazania jako initialElements 
        /// do biblioteki React Flow
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayground(int id)
        {
            var userId = FindOwner();
            return Ok(await _playgroundService.GetPlaygroundJson(userId, id));
        }
        /// <summary>
        /// Nie działa
        /// Publikuje tworzone dzieło
        /// W przyszłości doda się dodatkowe parametry
        /// Zwraca id opublikowanego dzieła
        /// </summary>
        /// <param name="artworkRequest"></param>
        [HttpPost()]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> PublishArtwork([FromBody] ArtworkPostRequest artworkRequest)
        {
            if (artworkRequest.Title == "") return BadRequest();

            var userId = FindOwner();
            return Ok(await _playgroundService.PublishArtwork(userId, artworkRequest.WipArtworkId, artworkRequest.Title, artworkRequest.Description, artworkRequest.IsPrivate, artworkRequest.Hashtags));
        }
        /// <summary>
        /// Generuje schemat json do odtwarzania dla tworzonego dzieła
        /// Zwraca json
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}/json")]
        public async Task<IActionResult> GenerateJson(int id)
        {var userId = FindOwner();
            return Ok(await _playgroundService.GeneratePlayerJson(userId, id));
        }


    }
}
