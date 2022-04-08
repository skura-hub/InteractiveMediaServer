using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class WipConnection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PKeyWipArtwork { get; set; }
        public string PKeyUser { get; set; }
        public WipArtwork WipArtwork { get; set; }

        [Required]
        public int FKeyWipNodeStarting { get; set; }
        public WipNode WipNodeStarting { get; set; }

        [Required]
        public int FKeyWipNodeEnding { get; set; }
        public WipNode WipNodeEnding { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required, MinLength(1), MaxLength(36)]
        public string ShortName { get; set; }

        [MaxLength(240)]
        public string? LongName { get; set; }

        
        
    }
}
