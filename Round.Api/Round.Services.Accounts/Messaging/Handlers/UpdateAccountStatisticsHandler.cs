using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Round.Services.Accounts.Domain;
using Round.Services.Accounts.Messaging.Commands;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Messaging.Handlers;

public class UpdateAccountStatisticsHandler : IHandleMessages<UpdateAccountStatisticsCommand>
{
    private readonly AccountsContext _context;

    public UpdateAccountStatisticsHandler(AccountsContext context)
    {
        _context = context;
    }
    
    public async Task Handle(UpdateAccountStatisticsCommand message)
    {
        var transactions = await _context.Transactions
            .Where(w => w.AccountId == message.AccountId)
            .ToListAsync();

        var summaries = transactions
            .GroupBy(g => new { g.Currency, g.SettlementDate.Date, g.TransactionCode.BalanceType })
            .Select(s => new AccountSummary
            {
                AccountId = message.AccountId,
                AmountInMinorUnits = s.Sum(t => t.AmountInMinorUnits),
                BalanceType = s.Key.BalanceType,
                Date = DateOnly.FromDateTime(s.Key.Date)
            });

        _context.RemoveRange(transactions);
        
        await _context.AccountSummary.AddRangeAsync(summaries);
        await _context.SaveChangesAsync();
    }
}