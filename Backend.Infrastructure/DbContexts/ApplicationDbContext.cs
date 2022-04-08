using Backend.Application.Interfaces.Shared;
using Backend.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCore;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private readonly IDateTimeService _dateTime;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime) :base (options)
        {
            _dateTime = dateTime;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseSnakeCaseNamingConvention();

        public DbSet<Artwork> Artwork { get; set; }
        public DbSet<WipArtwork> WipArtwork { get; set; }
        public DbSet<WipConnection> WipConnection { get; set; }
        public DbSet<WipMedia> WipMedia { get; set; }
        public DbSet<WipNode> WipNode { get; set; }
        public DbSet<Hashtag> Hashtag { get; set; }
        public DbSet<ViewPermissionForUser> ViewPermissionForUser { get; set; }
        public DbSet<ViewPermissionForGuest> ViewPermissionGuest { get; set; }
        public DbSet<MediaType> MediaType { get; set; }


        public IDbConnection Connection => Database.GetDbConnection();

        public bool HasChanges => ChangeTracker.HasChanges();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("public");
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRole");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaim");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogin");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaim");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserToken");
            });
            builder.Entity<Artwork>()
                .HasKey(entity => new {entity.Id});
            builder.Entity<ViewPermissionForGuest>()
                .HasKey(entity => new { entity.Id, entity.PKeyArtwork, entity.PKeyUserOwner});
            builder.Entity<ViewPermissionForUser>()
                .HasKey(entity => new { entity.PKeyUser, entity.PKeyArtwork, entity.PKeyUserOwner});
            builder.Entity<WipArtwork>()
                .HasKey(entity => new { entity.Id, entity.PKeyUser });
            builder.Entity<WipConnection>()
                .HasKey(entity => new { entity.Id, entity.PKeyWipArtwork, entity.PKeyUser });
            builder.Entity<WipMedia>()
                .HasKey(entity => new { entity.Id, entity.PKeyUser });
            builder.Entity<WipNode>()
                .HasKey(entity => new { entity.Id, entity.PKeyWipArtwork, entity.PKeyUser });

            builder.Entity<Artwork>()
                .HasOne(a => a.WipArtwork)
                .WithMany(wa => wa.Artworks)
                .HasForeignKey(k => new { k.FKeyWipArtwork, k.FKeyUser })
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Artwork>()
                .HasOne(a => a.User)
                .WithMany(u => u.Artworks)
                .HasForeignKey(k => new { k.FKeyUser})
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Artwork>()
                .HasMany(a => a.Hashtags)
                .WithMany(h => h.Artworks);
            builder.Entity<ViewPermissionForGuest>()
                .HasOne(vp => vp.Artwork)
                .WithMany(a => a.ViewPermissionForGuests)
                .HasForeignKey(k => new { k.PKeyArtwork})
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ViewPermissionForUser>()
                .HasOne(vp => vp.Artwork)
                .WithMany(a => a.ViewPermissionForUsers)
                .HasForeignKey(k => new { k.PKeyArtwork })
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ViewPermissionForUser>()
                .HasOne(vp => vp.User)
                .WithMany(u => u.ViewPermissionForUsers)
                .HasForeignKey(k => new { k.PKeyUser})
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<WipArtwork>()
                .HasOne(wa => wa.User)
                .WithMany(u => u.WipArtworks)
                .HasForeignKey(k => new { k.PKeyUser })
                .OnDelete(DeleteBehavior.Cascade); ;
            builder.Entity<WipConnection>()
                .HasOne(wc => wc.WipArtwork)
                .WithMany(wn => wn.WipConnections)
                .HasForeignKey(k => new { k.PKeyWipArtwork, k.PKeyUser})
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<WipConnection>()
                .HasOne(wc => wc.WipNodeStarting)
                .WithMany(wn => wn.WipStartingConnections)
                .HasForeignKey(k => new { k.FKeyWipNodeStarting, k.PKeyWipArtwork, k.PKeyUser })
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<WipConnection>()
                .HasOne(wc => wc.WipNodeEnding)
                .WithMany(wn => wn.WipEndingConnections)
                .HasForeignKey(k => new { k.FKeyWipNodeEnding, k.PKeyWipArtwork, k.PKeyUser })
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<WipNode>()
                .HasOne(wn => wn.WipArtwork)
                .WithMany(wa => wa.WipNodes)
                .HasForeignKey(k => new { k.PKeyWipArtwork, k.PKeyUser })
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<WipNode>()
                .HasOne(wn => wn.WipMedia)
                .WithMany(wa => wa.WipNodes)
                .HasForeignKey(k => new { k.FkeyWipMedia, k.PKeyUser })
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<WipMedia>()
                .HasOne(wm => wm.User)
                .WithMany(u => u.WipMedias)
                .HasForeignKey(k => new { k.PKeyUser })
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<WipMedia>()
                .HasOne(wm => wm.MediaType)
                .WithMany(mt => mt.WipMedias)
                .HasForeignKey(k => new { k.FkeyMediaType })
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MediaType>().HasData(
                new MediaType { Id=1, Type = "IFFRAME"},
                new MediaType { Id = 2, Type = "AUDIO" },
                new MediaType { Id = 3, Type = "IMAGE" },
                new MediaType { Id = 4, Type = "VIDEO" },
                new MediaType { Id = 5, Type = "DOCUMENT" }
            );
            base.OnModelCreating(builder);
        }
    }
}