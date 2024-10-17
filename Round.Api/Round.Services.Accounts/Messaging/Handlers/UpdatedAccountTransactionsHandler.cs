using Microsoft.EntityFrameworkCore;
using Rebus.Bus;
using Rebus.Handlers;
using Round.Common.Messaging.Accounts;
using Round.Services.Accounts.Messaging.Commands;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Messaging.Handlers;

public class UpdatedAccountTransactionsHandler : IHandleMessages<UpdatedAccountTransactionsCommand>
{
    private readonly AccountsContext _context;
    private readonly IBus _bus;

    public UpdatedAccountTransactionsHandler(AccountsContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }
    
    public async Task Handle(UpdatedAccountTransactionsCommand message)
    {
        foreach (var transaction in message.Transactions)
        {
            var exists = await _context.Transactions.AnyAsync(a => a.TransactionId == transaction.TransactionId);

            if (!exists)
            {
                await _context.Transactions.AddAsync(transaction);        
            }
        }
        
        await _context.SaveChangesAsync();

        await _bus.Send(new UpdateAccountStatisticsCommand(message.AccountId));
    }
}