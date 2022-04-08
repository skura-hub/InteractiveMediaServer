using Backend.Application.DTOs.ArtworkResponse;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers.v1
{
    [Route("api/artworks")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        readonly private IArtworkRepository _artworkRepository;
        readonly private IArtworkService _artworkService;

        public ArtworkController(IArtworkRepository artworkRepository, IArtworkService artworkService)
        {
            _artworkRepository = artworkRepository;
            _artworkService = artworkService;
        }
        private string FindOwner()
        {
            return User.FindFirst("uid").Value;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ArtworkResponse>), 200)]
        public async Task<IActionResult> GetAllFromOwner()
        {
            var userId = FindOwner();
            var artworks = await _artworkRepository.FindAllArtworksOwnedAsync(userId);
            return Ok(artworks);
        }

        [HttpGet("title")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ArtworkResponse>), 200)]
        public async Task<IActionResult> GetAllPublic(string title)
        {
            var artworks = await _artworkRepository.FindAllArtworksByName(title);
            return Ok(artworks);
        }

        [HttpGet("path")]
        [ProducesResponseType(typeof(string), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtwork(string path)
        {
            var artwork = await _artworkService.GetArtwork(path);
            return Ok(artwork.File);
        }

        [HttpDelete("path")]
        public async Task<IActionResult> DeleteArtwork(string path)
        {
            var userId = FindOwner();
            await _artworkService.DeleteArtwork(userId, path);
            return Ok();
        }
    }
}
