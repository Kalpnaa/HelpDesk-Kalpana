using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {


    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Description)
                .HasMaxLength(500);

            entity.Property(c => c.IsActive)
                .HasDefaultValue(true);

            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.TicketNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(t => t.TicketNumber)
                .IsUnique();

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(t => t.Priority)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // One Category can have many Tickets.
            entity.HasOne(t => t.Category)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // One User can create many Tickets.
            entity.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // One Agent can be assigned many Tickets.
            entity.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

