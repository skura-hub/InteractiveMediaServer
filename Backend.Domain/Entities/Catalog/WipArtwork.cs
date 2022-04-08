using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class WipArtwork : TimeLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PKeyUser { get; set; }
        public User User { get; set; }

        public List<Artwork> Artworks { get; set; }
        public List<WipConnection> WipConnections { get; set; }
        public List<WipNode> WipNodes { get; set; }

        [Required, MinLength(2), MaxLength(36)]
        public string Title { get; set; }
        [MaxLength(240)]
        public string? Description { get; set; }

        public string? File { get; set; }
        
    }
}
