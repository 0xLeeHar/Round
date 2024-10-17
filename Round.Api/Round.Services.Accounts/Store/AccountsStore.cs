using Microsoft.EntityFrameworkCore;
using Round.Common.Domain;
using Round.Services.Accounts.Domain;

namespace Round.Services.Accounts.Store;

public interface IAccountsStore
{
    Task<List<BankAccount>> GetAccountsAsync(Guid tenantId);

    Task<List<BankAccountTransaction>> GetAccountTransactions(Guid tenantId, Guid? accountId);
}

// NOTE: This is a half-baked approach. Normally this would be some form of CQRS, 3-tier, or command based pattern
public class AccountsStore : IAccountsStore
{
    private readonly AccountsContext _context;

    public AccountsStore(AccountsContext context)
    {
        _context = context;
    }

    public async Task<List<BankAccount>> GetAccountsAsync(Guid tenantId)
    {
        return await _context.Accounts
            .Where(w => w.OwnerId == tenantId)
            .ToListAsync();
    }

    public async Task<List<BankAccountTransaction>> GetAccountTransactions(Guid tenantId, Guid? accountId)
    {
        return await _context.Accounts
            .Where(w => w.OwnerId == tenantId 
                        && (accountId == null || w.AccountId == accountId))
            .Join(_context.Transactions, account => account.AccountId, transaction => transaction.AccountId, (account, transaction) => transaction)
            .ToListAsync();
    }
}