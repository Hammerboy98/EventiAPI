using EventiAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventiAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Artista> Artisti { get; set; }
        public DbSet<Evento> Eventi { get; set; }
        public DbSet<Biglietto> Biglietti { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Biglietto>()
                .HasOne(b => b.User)
                .WithMany(u => u.Biglietti)
                .HasForeignKey(b => b.UserId);

            builder.Entity<Evento>()
                .HasOne(e => e.Artista)
                .WithMany(a => a.Eventi)
                .HasForeignKey(e => e.ArtistaId);
        }
    }
}
