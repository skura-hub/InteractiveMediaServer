using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class WipNode : TimeLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PKeyWipArtwork { get; set; }
        public string PKeyUser { get; set; }
        public WipArtwork WipArtwork { get; set; }

        public int? FkeyWipMedia { get; set; }
        public WipMedia? WipMedia { get; set; }

        public List<WipConnection> WipStartingConnections { get; set; }
        public List<WipConnection> WipEndingConnections { get; set; }

        [MaxLength(36)]
        public string? Name { get; set; }
        [MaxLength(240)]
        public string? Description { get; set; }

        [Required]
		public int X {get; set;}
		[Required]
		public int Y {get; set;}
    }
}
