using AutoMapper;
using Backend.Application.DTOs.ApplicationUser;
using Backend.Application.DTOs.Wip;
using Backend.Application.Enums;
using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Mappings
{
    internal class AllMappings : Profile
    {
        public AllMappings()
        {
            CreateMap<User, PublicUserResponse>();
            CreateMap<WipArtwork, PlaygroundArtworkResponse>();

            CreateMap<WipConnection, PlaygroundConnectionResponse>()
                .ForMember(p => p.NodeEndingId,
                    opts => opts.MapFrom(wip => wip.FKeyWipNodeEnding.ToString()))
                .ForMember(p => p.NodeStartingId,
                    opts => opts.MapFrom(wip => wip.FKeyWipNodeStarting.ToString()))
                .ForMember(p => p.Id,
                    opts => opts.MapFrom(wip => wip.Id.ToString()));
            CreateMap<WipMedia, PlaygroundMediaResponse>()
                .ForMember(playground => playground.Type,
                opts => opts.MapFrom(wip =>  ((MediaTypes)wip.FkeyMediaType) ));
            /*
            CreateMap<WipNode, PlaygroundNodeResponse>()
                .ForMember(p => p.MediaId,
                    opts => opts.MapFrom(wip => wip.FkeyWipMedia))
                .ForMember(p => p.MediaPath, //TODO Raczej nie zadziała
                    opts => opts.MapFrom(wip => wip.WipMedia.Path));
            */
        }
    }
}
