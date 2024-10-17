using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Round.Services.Accounts.Store;

public class AccountsContextMigrationContextFactory : IDesignTimeDbContextFactory<AccountsContext>
{
    public AccountsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AccountsContext>();
        optionsBuilder.UseInMemoryDatabase("Round-InMem");

        return new AccountsContext(optionsBuilder.Options);
    }
}