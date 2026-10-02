using Microsoft.EntityFrameworkCore;
using TeamsApi.Models;

namespace TeamsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Начальные данные (Seed data)
        modelBuilder.Entity<Team>().HasData(
            new Team { Id = 1, Name = "Астана", City = "Астана", Description = "Столичный футбольный клуб" },
            new Team { Id = 2, Name = "Кайрат", City = "Алматы", Description = "Футбольный клуб из Алматы" },
            new Team { Id = 3, Name = "Тобол", City = "Костанай", Description = "Футбольный клуб из Костаная" }
        );
    }
}
