using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class Artwork : TimeLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string FKeyUser { get; set; }
        public User User { get; set; }

        [Required]
        public int FKeyWipArtwork { get; set; }
        public WipArtwork WipArtwork { get; set; }

        public List<Hashtag> Hashtags { get; set; }
        public List<ViewPermissionForGuest> ViewPermissionForGuests { get; set; }
        public List<ViewPermissionForUser> ViewPermissionForUsers { get; set; }

        [Required]
        public bool isPrivate { get; set; }

        [Required, MinLength(2), MaxLength(36)]
        public string Title { get; set; }
        [MaxLength(240)]
        public string? Description { get; set; }
        [Required]
        public string File { get; set; }
    }
}
