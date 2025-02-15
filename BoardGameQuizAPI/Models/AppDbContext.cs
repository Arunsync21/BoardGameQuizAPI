using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BoardGameQuizAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserResponse> UserResponses { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=boardgame.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys and configure them as identity columns
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<Role>()
                .HasKey(u => u.RoleId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<Set>()
                .HasKey(s => s.SetId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<Section>()
                .HasKey(s => s.SectionId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<Question>()
                .HasKey(q => q.QuestionId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<Option>()
                .HasKey(o => o.OptionId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<UserResponse>()
                .HasKey(ur => ur.ResponseId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<UserProgress>()
                .HasKey(up => up.ProgressId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity<Certificate>()
                .HasKey(c => c.CertificateId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // Define relationships and constraints

            // User to UserProgress (One-to-Many)
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set to UserProgress (One-to-Many)
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.Set)
                .WithMany()
                .HasForeignKey(up => up.SetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Section to UserProgress (One-to-Many)
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.Section)
                .WithMany()
                .HasForeignKey(up => up.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Section to Question (One-to-Many)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Role)
                .WithMany()
                .HasForeignKey(q => q.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set to Question (One-to-Many)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Set)
                .WithMany()
                .HasForeignKey(q => q.SetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Section to Question (One-to-Many)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Section)
                .WithMany()
                .HasForeignKey(q => q.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question to Option (One-to-Many)
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany()
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to UserResponse (One-to-Many)
            modelBuilder.Entity<UserResponse>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question to UserResponse (One-to-Many)
            modelBuilder.Entity<UserResponse>()
                .HasOne(ur => ur.Question)
                .WithMany()
                .HasForeignKey(ur => ur.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to Certificate (One-to-Many)
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set to Certificate (One-to-Many)
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Set)
                .WithMany()
                .HasForeignKey(c => c.SetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
