using AutoMapper;
using Backend.Application.DTOs.Interactive;
using Backend.Application.DTOs.Wip;
using Backend.Application.Enums;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Utils;
using Backend.Domain.Entities.Catalog;
using Backend.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class WipNodeRepository : GenericRepository<WipNode>, IWipNodeRepository
    {
        public WipNodeRepository(ApplicationDbContext repository) : base(repository) {
        }

        private IQueryable<WipNode> FindAll(string userId, int wipArtworkId)
        {
            var query =
                from wipNodes in Entities
                where wipNodes.PKeyUser == userId && wipNodes.PKeyWipArtwork == wipArtworkId
                select wipNodes;
            return query;
        }

        public Task<WipNode> FindByIdAsync(string userId, int wipArtworkId, int nodeId)
        {
            var query =
                from wipNode in Entities
                where wipNode.Id == nodeId &&
                   wipNode.PKeyUser == userId && wipNode.PKeyWipArtwork == wipArtworkId
                select wipNode;
            return query.SingleAsync<WipNode>();
        }

        public Task<PlaygroundNodeResponse> FindByIdWithMediaAsync(string userId, int wipArtworkId, int nodeId)
        {
            var query =
                from wipNodes in Entities
                where wipNodes.PKeyUser == userId && wipNodes.PKeyWipArtwork == wipArtworkId && wipNodes.Id == nodeId
                select new PlaygroundNodeResponse
                {
                    Id = wipNodes.Id.ToString(),
                    Data = new NodeDataResponse
                    {
                        Name = wipNodes.Name
                    },
                    Position = new PositionResponse
                    {
                        X = wipNodes.X,
                        Y = wipNodes.Y,
                    },
                    MediaType = wipNodes.WipMedia.MediaType.Type
                };
            return query.SingleAsync();
        }

        public Task<List<WipNode>> FindAllNodesAsync(string userId, int wipArtworkId)
        {
            return FindAll(userId, wipArtworkId).ToListAsync();
        }

        public Task<List<PlaygroundNodeResponse>> FindAllNodesWithMedia(string userId, int wipArtworkId){

            var query =
                from wipNodes in Entities
                where wipNodes.PKeyUser == userId && wipNodes.PKeyWipArtwork == wipArtworkId
                select new PlaygroundNodeResponse
                {
                    Id = wipNodes.Id.ToString(),
                    Data = new NodeDataResponse
                    {
                        Name = wipNodes.Name
                    },
                    Position = new PositionResponse
                    {
                        X = wipNodes.X,
                        Y = wipNodes.Y,
                    },
                    MediaType = wipNodes.WipMedia.MediaType.Type
                };
            return query.ToListAsync();
        }



        public async Task<List<Node>> FindAllNodesWithEverything(string userId, int wipArtworkId)
        {
            IndexMapper<int> indexMapper = new IndexMapper<int>(0);
            var query =
                from wipNodes in _dbContext.Set<WipNode>()
                where wipNodes.PKeyUser == userId && wipNodes.PKeyWipArtwork == wipArtworkId
                select new Node
                {
                    Id = wipNodes.Id,
                    Name = wipNodes.Name,
                    Media = new Media
                    {
                        Path = wipNodes.WipMedia.Path,
                        Type = wipNodes.WipMedia.MediaType.Type,
                        Title = wipNodes.WipMedia.MediaType.Id == (int)MediaTypes.DOCUMENT ?
                            wipNodes.WipMedia.Name : null
                    },
                    Edges = new List<Edge>(wipNodes.WipStartingConnections.Select(edge =>
                        new Edge
                        {
                            End = edge.FKeyWipNodeEnding,
                            IsDefault = edge.IsDefault,
                            Name = edge.ShortName
                        })
                    )
                };
            var result = await query.ToListAsync();
            List<Node> nodes = result.OrderBy(o => o.Id).ToList();
            for(int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Id = indexMapper.CreateAndGetNewIndex(nodes[i].Id);
            }
            for(int i = 0; i < nodes.Count; i++)
            {
                int totalConnections = nodes[i].Edges.Count;
                int totalDefaultConnections = 0;

                nodes[i].Edges.ForEach(e =>
                {
                    e.End = indexMapper.GetIndex(e.End);
                    if (e.IsDefault)
                    {
                        totalDefaultConnections++;
                    }
                });
                if(totalConnections == 1)
                {
                    nodes[i].Edges[0].IsDefault = true;
                }
                if (totalConnections != 1 && totalDefaultConnections != 1)
                {
                    nodes[i].HasDefaultConnection = false;
                    nodes[i].HasMultipleDefaultConnections = false;
                }
                else if (totalConnections == 1 || totalDefaultConnections == 1)
                {
                    nodes[i].HasDefaultConnection = true;
                    nodes[i].HasMultipleDefaultConnections = false;
                }
                else
                {
                    nodes[i].HasDefaultConnection = false;
                    nodes[i].HasMultipleDefaultConnections = true;
                }
            }
            return nodes;
        }

        public async Task UpdateByIdAsync(string userId, int wipArtworkId, int nodeId, System.Action<WipNode> updateAction)
        {
            WipNode wipNode = await FindByIdAsync(userId, wipArtworkId, nodeId);
            updateAction(wipNode);
            await UpdateAsync(wipNode);
        }

        public async Task DeleteByIdAsync(string userId, int wipArtworkId, int nodeId)
        {
            WipNode wipNode = await FindByIdAsync(userId, wipArtworkId, nodeId);
            await DeleteAsync(wipNode);
        }
    }
}