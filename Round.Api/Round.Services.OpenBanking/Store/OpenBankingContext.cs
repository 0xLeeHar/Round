using Microsoft.EntityFrameworkCore;
using Round.Services.OpenBanking.Domain;

namespace Round.Services.OpenBanking.Store;

public class OpenBankingContext : DbContext
{
    public DbSet<BankAccountConnection> AccountConnections { get; set; } = default!;
    
    public OpenBankingContext(DbContextOptions<OpenBankingContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("openbanking");

        modelBuilder.Entity<BankAccountConnection>(entity =>
        {
            entity.ToTable(name: "Connections");
            entity.HasKey(e => e.AccountId);

            entity.Property(e => e.AccountId);
            entity.Property(e => e.BankId);
            entity.Property(e => e.ConsentToken)
                .IsFixedLength()
                .HasMaxLength(500);
        });
    }
}