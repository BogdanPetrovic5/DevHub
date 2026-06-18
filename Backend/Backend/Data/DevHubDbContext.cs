using Backend.Models;
using Backend.Models.Commit;
using Backend.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class DevHubDbContext : DbContext
    {
        public DevHubDbContext(DbContextOptions<DevHubDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Repo> Repositories { get; set; }
        public DbSet<RepoFile> RepoFiles { get; set; }
        public DbSet<RepoCommit> RepoCommits { get; set; }
        public DbSet<RepoCommitFile> RepoCommitFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username);
         
            modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.Id);
            modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.Token).IsUnique();
            modelBuilder.Entity<RefreshToken>().HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Repo>().HasKey(r => r.Id);
            modelBuilder.Entity<Repo>().HasIndex(r => r.Name);
            modelBuilder.Entity<Repo>().HasIndex(r => new { r.UserId, r.Name }).IsUnique();
            modelBuilder.Entity<Repo>().HasOne(r => r.User)
                .WithMany(u => u.Repositories)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RepoFile>().HasKey(rf => rf.Id);
            modelBuilder.Entity<RepoFile>().HasIndex(rf => new { rf.RepositoryId, rf.Path }).IsUnique();
            modelBuilder.Entity<RepoFile>().HasOne(rf => rf.Repository)
                .WithMany(r => r.Files)
                .HasForeignKey(rf => rf.RepositoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RepoCommit>().HasKey(rc => rc.Id);
            modelBuilder.Entity<RepoCommit>().HasOne(rc => rc.Repository)
                .WithMany(r => r.RepoCommits)
                .HasForeignKey(rc => rc.RepositoryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RepoCommit>().HasOne(rc => rc.User)
                .WithMany(u => u.RepoCommits)
                .HasForeignKey(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RepoCommit>().HasIndex(rc => rc.UserId);

            modelBuilder.Entity<RepoCommitFile>().HasKey(rcf => rcf.Id);
            modelBuilder.Entity<RepoCommitFile>().HasOne(rcf => rcf.Commit)
                .WithMany(rc => rc.Files)
                .HasForeignKey(rcf => rcf.CommitId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RepoCommitFile>().HasIndex(rcf => rcf.CommitId);
           
        }
    }
}
