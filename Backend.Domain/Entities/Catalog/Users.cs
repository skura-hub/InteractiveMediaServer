using Backend.Domain.Entities.Catalog;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class User : IdentityUser
    {
        public List<Artwork> Artworks { get; set; }
        public List<WipArtwork> WipArtworks { get; set; }
        public List<WipMedia> WipMedias { get; set; }
        public List <ViewPermissionForUser> ViewPermissionForUsers { get; set; }

        public string? Description { get; set; }
        public string? Status { get; set; }
        [Required]
        public bool IsActive { get; set; } = false;

    }
}