using Microsoft.EntityFrameworkCore;
using Round.Common;
using Round.Common.Domain;
using Round.Common.Sql;
using Round.Services.Accounts.Domain;

namespace Round.Services.Accounts.Store;

public class AccountsContext : DbContext
{
    public DbSet<BankAccount> Accounts { get; set; } = default!;
    public DbSet<BankAccountTransaction> Transactions { get; set; } = default!;
    public DbSet<AccountSummary> AccountSummary { get; set; } = default!;

    public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("accounts");
        CreateDbSchema(modelBuilder);
    }

    private void CreateDbSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.ToTable(name: "Accounts");
            entity.HasKey(e => e.AccountId);

            entity.Property(e => e.AccountId);
            entity.Property(e => e.BankId);
            entity.Property(e => e.OwnerId);

            entity.Property(e => e.AccountRoutingName)
                .HasMaxLength(500);
            
            entity.Property(e => e.AccountType)
                .HasDefaultValue(BankAccountType.Unknown);

            entity.Property(e => e.Country)
                .HasConversion(PropertyConverters.CreateNameConverter<Country>())
                .HasDefaultValue(Country.GB)
                .IsFixedLength()
                .HasMaxLength(2)
                .IsRequired();
            
            entity.Property(e => e.Currency)
                .IsFixedLength()
                .HasMaxLength(3)
                .IsRequired();

            entity.OwnsMany(p => p.Addresses, ai =>
            {
                ai.ToTable("Accounts_Addresses");
                ai.WithOwner().HasForeignKey("AccountId");
                ai.HasKey("AccountId", "Value");
                
                ai.Property(p => p.Value)
                    .HasMaxLength(500);
            });

            entity.OwnsOne(p => p.Balance, ai =>
            {
                ai.Property(p => p.AmountInMinorUnits)
                    .HasColumnName("Balance_Amount")
                    .HasDefaultValue(0);
                
                ai.Property(p => p.LastUpdatedDate)
                    .HasColumnName("Balance_LastUpdatedDate")
                    .HasDefaultValue(null);
            });
        });

        modelBuilder.Entity<BankAccountTransaction>(entity =>
        {
            entity.ToTable(name: "Transaction");
            entity.HasKey(e => e.TransactionId);
            
            entity.Property(e => e.AccountId);
            entity.Property(e => e.SettlementDate);
            
            entity.Property(e => e.Currency)
                .IsFixedLength()
                .HasMaxLength(3)
                .IsRequired();
            
            entity.Property(e => e.AmountInMinorUnits);
            
            entity.Property(e => e.Description)
                .IsFixedLength()
                .HasMaxLength(500);
            
            entity.Property(e => e.Reference)
                .IsFixedLength()
                .HasMaxLength(500);
            
            entity.Property(e => e.Status)
                .HasDefaultValue(TransactionStatus.Unknown);
            
            entity.Property(e => e.TransactionCode)
                .HasConversion(PropertyConverters.CreateNameConverter<IsoBankTransactionCode>())
                .IsFixedLength()
                .HasMaxLength(4);
        });

        modelBuilder.Entity<BankAccount>()
            .HasMany<BankAccountTransaction>()
            .WithOne()
            .HasForeignKey(f => f.AccountId)
            .IsRequired();

        modelBuilder.Entity<AccountSummary>(entity =>
        {
            entity.ToTable(name: "AccountSummary");
            entity.HasKey(e => new
            {
                e.AccountId, 
                e.Date,
                e.BalanceType
            });

            entity.Property(e => e.AccountId);
            entity.Property(e => e.Date);
            entity.Property(e => e.AmountInMinorUnits);
            entity.Property(e => e.BalanceType);
        });
        
        modelBuilder.Entity<BankAccount>()
            .HasMany<AccountSummary>()
            .WithOne()
            .HasForeignKey(f => f.AccountId)
            .IsRequired();
    }
}