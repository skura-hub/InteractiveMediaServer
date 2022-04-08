using AutoMapper;
using Backend.Application.DTOs.Identity;
using Backend.Application.DTOs.Mail;
using Backend.Application.DTOs.Results;
using Backend.Application.DTOs.Settings;
using Backend.Application.DTOs.Wip;
using Backend.Application.Enums;
using Backend.Application.Exceptions;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Shared;
using Backend.Domain.Entities.Catalog;
using Backend.Infrastructure.DbContexts;
using HeyRed.Mime;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.Infrastructure.Utils;
using Backend.Application.DTOs.Interactive;

namespace Backend.Infrastructure.Identity.Services
{
    public class PlaygroundService : IPlaygroundService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWipArtworkRepository _wipArtworkRepository;
        private readonly IWipConnectionRepository _wipConnectionRepository;
        private readonly IWipMediaRepository _wipMediaRepository;
        private readonly IWipNodeRepository _wipNodeRepository;
        private readonly IArtworkService _artworkService;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PlaygroundService(
            IUnitOfWork unitOfWork,
            ApplicationDbContext applicationDbContext,
            IArtworkService artworkService,
            IWipArtworkRepository wipArtworkRepository,
            IWipConnectionRepository wipConnectionRepository,
            IWipMediaRepository wipMediaRepository,
            IWipNodeRepository wipNodeRepository,
            IApplicationUserRepository applicationUserRepository,
            IMapper mapper,
            UserManager<User> userManager,
            IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _unitOfWork = unitOfWork;
            _applicationDbContext = applicationDbContext;
            _wipArtworkRepository = wipArtworkRepository;
            _wipConnectionRepository = wipConnectionRepository;
            _wipMediaRepository = wipMediaRepository;
            _wipNodeRepository = wipNodeRepository;
            _mapper = mapper;
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
            _artworkService = artworkService;
        }
        //TODO: Replace with UnitOfWork
        private async Task SaveChanges()
        {
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<string> GeneratePlayerJson(string userId, int wipArtworkId)
        {
            //TODO: Wielowątkowość
            var user = await _applicationUserRepository.GetByIdAsync(userId);
            var nodes = await _wipNodeRepository.FindAllNodesWithEverything(userId, wipArtworkId);
            var artwork = await _wipArtworkRepository.FindByIdAsync(userId, wipArtworkId);
            InteractivePlayer result = new InteractivePlayer
            {
                Author = user.UserName,
                Date = _dateTimeService.NowUtc,
                Description = artwork.Description,
                Title = artwork.Title,
                Version = "0.1",
                Nodes = nodes
            };
            var json = MyJsonSerializer.Serialize(result);
            return json;
        }

        public async Task<string> GetPlaygroundJson(string userId, int wipArtworkId)
        {
            //TODO korzystanie z optymalizacji wielowątkowości
            var nodes = await _wipNodeRepository.FindAllNodesWithMedia(userId, wipArtworkId);
            var edges = await _wipConnectionRepository.FindAllConnectionsAsync(userId, wipArtworkId);
            var edgesOk = _mapper.Map<List<PlaygroundConnectionResponse>>(edges);
            List<Object> all = new List<object>();
            all.AddRange(nodes);
            all.AddRange(edgesOk);
            var allJson = MyJsonSerializer.Serialize(all);
            return allJson;
        }

        public async Task<Guid> PublishArtwork(string userId, int wipArtworkId, string title, string description, bool isPrivate, List<String>? hashtags)
        {
            var createdAt = _dateTimeService.NowUtc;
            var guid = Guid.NewGuid();
            var json = await GeneratePlayerJson(userId, wipArtworkId);

            Artwork artwork = new Artwork
            {
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                FKeyWipArtwork = wipArtworkId,
                FKeyUser = userId,
                Description = description,
                Title = title,
                Id = guid,
                isPrivate = isPrivate,
                File = json
            };

            return await _artworkService.PublishArtwork(artwork);
        }

        public async Task DeleteNode(string userId, int artworkId, int nodeId)
        {
            await _wipNodeRepository.DeleteByIdAsync(userId, artworkId, nodeId);
            await SaveChanges();
        }
        public async Task<int> AddNode(string userId, int artworkId, int? mediaId, int x, int y, string? name)
        {
            var createdAt = _dateTimeService.NowUtc;
            WipNode wipNode = new()
            {
                PKeyUser = userId,
                PKeyWipArtwork = artworkId,
                X = x,
                Y = y,
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                FkeyWipMedia = mediaId,
                Name = name
        };
            WipNode addedNode = await _wipNodeRepository.AddAsync(wipNode);
            await SaveChanges();
            return addedNode.Id;
        }

        public async Task UpdateNodeByName(string userId, int artworkId, int nodeId, string name)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipNodeRepository.UpdateByIdAsync(userId, artworkId, nodeId,
            (WipNode wipNode) =>
            {
                wipNode.Name = name;
                wipNode.UpdatedAt = updatedAt;
            });
            await SaveChanges();
        }
        public async Task UpdateNodeByPosition(string userId, int artworkId, int nodeId, int x, int y)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipNodeRepository.UpdateByIdAsync(userId, artworkId, nodeId,
            (WipNode wipNode) =>
            {
                wipNode.X = x;
                wipNode.Y = y;
                wipNode.UpdatedAt = updatedAt;
            });
            await SaveChanges();
        }
        public async Task UpdateNodeByMedia(string userId, int artworkId, int nodeId, int mediaId)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipNodeRepository.UpdateByIdAsync(userId, artworkId, nodeId,
            (WipNode wipNode) =>
            {
                wipNode.FkeyWipMedia = mediaId;
                wipNode.UpdatedAt = updatedAt;
            });
            await SaveChanges();
        }
        public async Task DeleteMedia(string userId, int mediaId)
        {
            await _wipMediaRepository.DeleteByIdAsync(userId, mediaId);
            await SaveChanges();
        }

        public async Task UpdateMediaDocument(string userId, int mediaId, string content, string name)
        {
            var updatedAt = _dateTimeService.NowUtc;
            WipMedia media = new WipMedia
            {
                Id = mediaId,
                UpdatedAt = updatedAt
            };
            if (content != null && content != "") media.Path = content;
            if (name != null && name != "") media.Name = name;

            await _wipMediaRepository.UpdateAsync(media);
            await SaveChanges();
        }

        public async Task<PlaygroundMediaResponse> AddMediaDocument(string userId, string content, string name)
        {

            var createdAt = _dateTimeService.NowUtc;
            WipMedia media = new()
            {
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                Path = content,
                Name = name,
                PKeyUser = userId,
                FkeyMediaType = (int)MediaTypes.DOCUMENT
            };
            WipMedia newMedia = await _wipMediaRepository.AddAsync(media);
            await SaveChanges();
            PlaygroundMediaResponse result = _mapper.Map<PlaygroundMediaResponse>(newMedia);
            return result;
        }

        public async Task<PlaygroundMediaResponse> AddMedia(string userId, string path, string name)
        {
            var createdAt = _dateTimeService.NowUtc;
            WipMedia media = new()
            {
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                Path = path,
                Name = name,
                PKeyUser = userId
            };
            string fileType;
            try
            {
                var mimeType = MimeTypesMap.GetMimeType(path);
                var index = mimeType.IndexOf('/');
                fileType = mimeType.Substring(0, index);
                if (fileType == "audio")
                    media.FkeyMediaType = (int) MediaTypes.AUDIO;
                else if (fileType == "image")
                    media.FkeyMediaType = (int) MediaTypes.IMAGE;
                else if (fileType == "video")
                    media.FkeyMediaType = (int) MediaTypes.VIDEO;
                else
                    throw new Exception();
                var dotIndex = path.LastIndexOf(".");
                media.Extension = path.Substring(dotIndex+1);
            }
            catch
            {
                media.FkeyMediaType = (int)MediaTypes.IFFRAME;
            }
            WipMedia newMedia =  await _wipMediaRepository.AddAsync(media);
            await SaveChanges();
            PlaygroundMediaResponse result = _mapper.Map<PlaygroundMediaResponse>(newMedia);
            return result;
        }

        public async Task DeleteArtwork(string userId, int artworkId)
        {
            await _wipArtworkRepository.DeleteByIdAsync(userId, artworkId);
            await SaveChanges();
        }

        private Task<WipNode> AddStartingNode(DateTime createdAt, string userId, int artworkId, int mediaId)
        {
            WipNode startingPoint = new WipNode
            {
                Id = 1,
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                PKeyWipArtwork = artworkId,
                PKeyUser = userId,
                FkeyWipMedia = mediaId,
                Name = "Start",
                Description = "Twój początkowy slajd",
                X = 0,
                Y = 0
            };
            return _wipNodeRepository.AddAsync(startingPoint);
        }

        public async Task<int> AddArtwork(string userId, string title, string description)
        {
            var createdAt = _dateTimeService.NowUtc;
            WipArtwork artwork = new()
            {
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                PKeyUser = userId,
                Title = title,
                Description = description
            };

            var newArtwork = await _wipArtworkRepository.AddAsync(artwork);
            _applicationDbContext.SaveChanges(); //ToDo: ???
            await AddStartingNode(createdAt, userId, newArtwork.Id, 1);
            await SaveChanges();
            return newArtwork.Id;
        }

        public async Task UpdateArtworkByTitle(string userId, int artworkId, string title)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipArtworkRepository.UpdateByIdAsync(userId, artworkId,
            (WipArtwork wipArtwork) =>
            {
                wipArtwork.Title = title;
                wipArtwork.UpdatedAt = updatedAt;
            });
            await SaveChanges();

        }

        public async Task UpdateArtworkByDescription(string userId, int artworkId, string description)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipArtworkRepository.UpdateByIdAsync(userId, artworkId,
            (WipArtwork wipArtwork) =>
            {
                wipArtwork.Description = description;
                wipArtwork.UpdatedAt = updatedAt;
            });
            await SaveChanges();
        }

        public async Task DeleteConnection(string userId, int artworkId, int connectionId)
        {
            await _wipConnectionRepository.DeleteByIdAsync(userId, artworkId, connectionId);
            await SaveChanges();
        }

        public async Task<int> AddConnection(string userId, int artworkId, int nodeStarting, int nodeEnding, bool isDefault, string shortName, string longName)
        {
            WipConnection connection = new()
            {
                PKeyUser = userId,
                PKeyWipArtwork = artworkId,
                FKeyWipNodeStarting = nodeStarting,
                FKeyWipNodeEnding = nodeEnding,
                IsDefault = isDefault,
                ShortName = shortName,
                LongName = longName
            };

            var newConnection = await _wipConnectionRepository.AddAsync(connection);
            await SaveChanges();
            return newConnection.Id;
        }

        public async Task UpdateConnectionByIsDefault(string userId, int artworkId, int connectionId, bool isDefault)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipConnectionRepository.UpdateByIdAsync(userId, artworkId, connectionId,
            (WipConnection wipConnection) =>
            {
                wipConnection.IsDefault = isDefault;
            });
            await SaveChanges();
        }

        public async Task UpdateConnectionByShortName(string userId, int artworkId, int connectionId, string shortName)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipConnectionRepository.UpdateByIdAsync(userId, artworkId, connectionId,
            (WipConnection wipConnection) =>
            {
                wipConnection.ShortName = shortName;
            });
            await SaveChanges();
        }

        public async Task UpdateConnectionByLongName(string userId, int artworkId, int connectionId, string longName)
        {
            var updatedAt = _dateTimeService.NowUtc;
            await _wipConnectionRepository.UpdateByIdAsync(userId, artworkId, connectionId,
            (WipConnection wipConnection) =>
            {
                wipConnection.LongName = longName;
            });
            await SaveChanges();
        }
    }


}