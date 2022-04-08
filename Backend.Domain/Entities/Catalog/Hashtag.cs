using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class Hashtag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public List<Artwork> Artworks { get; set; }

        [Required, MaxLength(36)]
        public string TagName { get; set; }
        //[Required]
        //public Sensitiveness SensitivenessRating { get; set; }
    }
}
