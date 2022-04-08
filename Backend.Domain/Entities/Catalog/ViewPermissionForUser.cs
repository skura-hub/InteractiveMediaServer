using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entities.Catalog
{
    public class ViewPermissionForUser : TimeLog
    {
        public string PKeyUser { get; set; }
        public User User { get; set; }

        public Guid PKeyArtwork { get; set; }
        public string PKeyUserOwner { get; set; }
        public Artwork Artwork { get; set; }

        public List<User> Users { get; set; }
    }
}
