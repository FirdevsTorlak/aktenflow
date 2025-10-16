using AktenFlow.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AktenFlow.Api.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CaseFile> CaseFiles => Set<CaseFile>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<CaseItem> CaseItems => Set<CaseItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaseFile>().HasMany(cf => cf.Documents)
                                           .WithOne(d => d.CaseFile!)
                                           .HasForeignKey(d => d.CaseFileId);
            modelBuilder.Entity<Document>().HasIndex(d => new { d.CaseFileId, d.IsLatest });
        }
    }
}
