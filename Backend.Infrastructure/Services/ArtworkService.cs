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
    public class ArtworkService : IArtworkService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArtworkRepository _artworkRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IViewPermissionForGuestRepository _viewPermissionForGuestRepository;

        public ArtworkService(
            IUnitOfWork unitOfWork,
            ApplicationDbContext applicationDbContext,
            IApplicationUserRepository applicationUserRepository,
            IArtworkRepository artworkRepository,
            IViewPermissionForGuestRepository viewPermissionForGuestRepository,
            IMapper mapper,
            UserManager<User> userManager,
            IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _unitOfWork = unitOfWork;
            _applicationDbContext = applicationDbContext;
            _artworkRepository = artworkRepository;
            _mapper = mapper;
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
            _viewPermissionForGuestRepository = viewPermissionForGuestRepository;
        }

        //TODO: Replace with UnitOfWork
        private async Task SaveChanges()
        {
            await _applicationDbContext.SaveChangesAsync();
        }

        private Guid CreateCryptographicallySecureGuid()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }

        private Task<ViewPermissionForGuest> GenerateGuestPermission(Artwork artwork)
        {
            var secretKey = CreateCryptographicallySecureGuid();
            ViewPermissionForGuest guestPermission = new ViewPermissionForGuest
            {
                PKeyArtwork = artwork.Id,
                PKeyUserOwner = artwork.FKeyUser,
                SecretKey = secretKey.ToString(),
                CreatedAt = artwork.UpdatedAt,
                UpdatedAt = artwork.UpdatedAt
            };
            return _viewPermissionForGuestRepository.AddAsync(guestPermission);
        }

        public async Task<Guid> PublishArtwork(Artwork artwork)
        {
            var newArtwork = await _artworkRepository.AddAsync(artwork);
            await SaveChanges();
            if (newArtwork.isPrivate)
            {
                var viewPermissionForGuest = await GenerateGuestPermission(newArtwork);
                await SaveChanges();
            }
            return newArtwork.Id;
        }

        public async Task<Artwork> GetArtwork(string path)
        {
            if (path.StartsWith('s')){
                var secret = path.Substring(1);
                var permission = await _viewPermissionForGuestRepository.FindByPath(secret);
                var artwork = await _artworkRepository.FindById(permission.PKeyArtwork);
                return artwork;
            }
            else if (path.StartsWith('p'))
            {
                var publicPath = path.Substring(1);
                var id = Guid.Parse(publicPath);
                var artwork = await _artworkRepository.FindById(id);
                if (artwork.isPrivate) throw new KeyNotFoundException();
                return artwork;
            }
            throw new Exception("Wrong path for artwork");
        }

        public async Task DeleteArtwork(string userId, string path)
        {
            var artwork = await GetArtwork(path);
            if (artwork.FKeyUser != userId)
                throw new KeyNotFoundException();
            await _artworkRepository.DeleteAsync(artwork);
            await SaveChanges();
        }
    }
}
