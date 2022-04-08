using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class ViewPermissionForGuest : TimeLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid PKeyArtwork { get; set; }
        public string PKeyUserOwner { get; set; }
        public Artwork Artwork { get; set; }

        public DateTime ExpirationDate {get; set;}
        [Required]
        public string SecretKey { get; set; }

       
        
    }
}
