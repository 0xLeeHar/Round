using Rebus.Handlers;
using Round.Common;
using Round.Common.Messaging.Accounts;
using Round.Services.Accounts.Domain;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Messaging.Handlers;

public class UpdateAccountBalanceHandler : IHandleMessages<UpdateAccountBalanceCommand>
{
    private readonly AccountsContext _context;
    private readonly ITimeProvider _timeProvider;

    public UpdateAccountBalanceHandler(AccountsContext context, ITimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }
    
    public async Task Handle(UpdateAccountBalanceCommand message)
    {
        var account = await _context.Accounts.FindAsync(message.AccountId);

        if (account is null)
        {
            return;
        }

        account.Balance = new AccountBalance
        {
            AmountInMinorUnits = message.Balance.AmountInMinorUnits,
            LastUpdatedDate = _timeProvider.GetUtcNow()
        };

        await _context.SaveChangesAsync();
    }
}