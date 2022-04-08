using AutoMapper;
using Backend.Application.DTOs.Wip;
using Backend.Application.Enums;
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
    [Route("api/playground/medias")]
    [ApiController]
    [Authorize]
    public class WipMediaController : ControllerBase
    {
        readonly private IPlaygroundService _playgroundService;
        readonly private IWipMediaRepository _wipMediaRepository;
        readonly private IMapper _mapper;

        public WipMediaController(IPlaygroundService playgroundService, 
            IWipMediaRepository wipMediaRepository,
            IMapper mapper)
        {
            _playgroundService = playgroundService;
            _wipMediaRepository = wipMediaRepository;
            _mapper = mapper;
        }

        private string FindOwner()
        {
            return User.FindFirst("uid").Value;
        }

        [ProducesResponseType(typeof(List<PlaygroundMediaResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAll(string? type)
        {
            var userId = FindOwner();
            int typeId;
            if(type == null || type == "")
            {
                var allMedias = await _wipMediaRepository.FindAllMediasAsync(userId);
                List<PlaygroundMediaResponse> allResult = _mapper.Map<List<PlaygroundMediaResponse>>(allMedias);
                return Ok(allResult);
            }
            if (type == MediaTypes.AUDIO.ToString())
            {
                typeId = (int)MediaTypes.AUDIO;
            }
            else if (type == MediaTypes.DOCUMENT.ToString())
            {
                typeId = (int)MediaTypes.DOCUMENT;
            }
            else if (type == MediaTypes.IFFRAME.ToString())
            {
                typeId = (int)MediaTypes.IFFRAME;
            }
            else if (type == MediaTypes.IMAGE.ToString())
            {
                typeId = (int)MediaTypes.IMAGE;
            }
            else if (type == MediaTypes.VIDEO.ToString())
            {
                typeId = (int)MediaTypes.VIDEO;
            }
            else
            {
                throw new Exception("Type: " + type + " is unsuported");
            }

            var medias = await _wipMediaRepository.FindAllMediaByTypeAsync(userId, typeId);
            List<PlaygroundMediaResponse> result = _mapper.Map<List<PlaygroundMediaResponse>>(medias);
            return Ok(result);
        }

        [ProducesResponseType(typeof(PlaygroundMediaResponse), 200)]
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = FindOwner();
            var media = await _wipMediaRepository.FindByIdAsync(userId, id);
            var result = _mapper.Map<PlaygroundMediaResponse>(media);
            return Ok(result);
        }

        [ProducesResponseType(typeof(PlaygroundMediaResponse), 200)]
        [HttpPost]
        public async Task<IActionResult> Add(PlaygroundMediaRequestPost m)
        {
            var userId = FindOwner();

            PlaygroundMediaResponse media = await _playgroundService.AddMedia(userId, m.Path, m.Name);
            return Ok(media);
        }

        [ProducesResponseType(typeof(PlaygroundMediaResponse), 200)]
        [HttpPost("document")]
        public async Task<IActionResult> AddDocument(PlaygroundMediaRequestPost m)
        {
            var userId = FindOwner();

            PlaygroundMediaResponse media = await _playgroundService.AddMediaDocument(userId, m.Path, m.Name);
            return Ok(media);
        }

        [HttpPatch("document")]
        public async Task<IActionResult> UpdateDocument(PlaygroundMediaRequestPatch a)
        {
            var userId = FindOwner();
            await _playgroundService.UpdateMediaDocument(userId, a.Id, a.Content, a.Name);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int mediaId)
        {
            if (mediaId == 1) return BadRequest();
            var userId = FindOwner();
            await _playgroundService.DeleteMedia(userId, mediaId);
            return Ok();
        }

    }


}
