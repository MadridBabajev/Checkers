using Domain.Db;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class ApplicationDbContext: DbContext
{
    public DbSet<CheckersGame> CheckersGames { get; set; } = default!;
    public DbSet<GameState> GameStates { get; set; } = default!;
    public DbSet<CheckersOptions> CheckersOptions { get; set; } = default!;
    public DbSet<Player> Players { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CheckersGame>().HasOne(p => p.GamePlayer1)
            .WithMany(g => g.GamesAsPlayedP1).HasForeignKey(g => g.GamePlayer1Id);
        
        modelBuilder.Entity<CheckersGame>().HasOne(p => p.GamePlayer2)
            .WithMany(g => g.GamesAsPlayedP2).HasForeignKey(g => g.GamePlayer2Id);

        // modelBuilder.Entity<CheckersGame>().HasOne();
        
        modelBuilder.Entity<Player>().HasIndex(p => p.PlayerName)
            .IsUnique();
        
    }

}