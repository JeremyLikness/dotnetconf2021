using MessierModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class MessierContext : DbContext
    {
        public MessierContext()
        {
        }

        public MessierContext(DbContextOptions<MessierContext> options)
            : base(options)
        {
        }

        public DbSet<Constellation> Constellations { get; set; }
        public DbSet<ObjectType> ObjectTypes { get; set; }
        public DbSet<MessierTarget> Targets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Constellation>()
                .HasMany(c => c.Targets)
                .WithOne(m => m.Constellation);

            modelBuilder.Entity<ObjectType>()
                .HasMany(o => o.Targets)
                .WithOne(m => m.Type);

            base.OnModelCreating(modelBuilder);
        }
    }
}