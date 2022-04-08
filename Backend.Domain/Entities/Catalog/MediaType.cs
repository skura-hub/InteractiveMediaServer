using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class MediaType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public List<WipMedia> WipMedias { get; set; }

        [Required]
        public string Type { get; set; }

        //public virtual ICollection<WipMedia> WipMedias { get; set; }
    }
}

