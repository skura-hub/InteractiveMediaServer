using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class WipMedia : TimeLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PKeyUser { get; set; }
        public User User { get; set; }

        [Required]
        public int FkeyMediaType { get; set; }
        public MediaType MediaType { get; set; }

        public List<WipNode>WipNodes { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public string? Extension { get; set; }





        
    }
}
