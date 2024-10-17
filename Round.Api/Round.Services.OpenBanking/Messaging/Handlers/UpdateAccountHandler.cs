using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Round.Common.Messaging.Accounts;
using Round.Services.OpenBanking.Domain;
using Round.Services.OpenBanking.Messaging.Commands;
using Round.Services.OpenBanking.Messaging.Services;
using Round.Services.OpenBanking.Store;

namespace Round.Services.OpenBanking.Messaging.Handlers;

public class UpdateAccountHandler : 
    IHandleMessages<RequestAccountBalanceCommand>,
    IHandleMessages<RequestAccountTransactionsCommand>
{
    private const int AccountUpdatedMinutes = 70;
    
    private readonly IBus _bus;
    private readonly OpenBankingContext _context;
    private readonly IOpenBankingProvider _openBankingProvider;
    private readonly ILogger<UpdateAccountHandler> _logger;

    public UpdateAccountHandler(IBus bus, OpenBankingContext context, IOpenBankingProvider openBankingProvider, ILogger<UpdateAccountHandler> logger)
    {
        _bus = bus;
        _context = context;
        _openBankingProvider = openBankingProvider;
        _logger = logger;
    }
    
    public async Task Handle(RequestAccountBalanceCommand message)
    {
        var connection = await GetConnectionAsync(message.AccountId);

        if (connection is null)
        {
            // Handle failure here
            _logger.LogError("Account connection not found");
            return;
        }
        
        var newBalance = await _openBankingProvider.GetAccountBalanceAsync(connection.ConsentToken);

        // Updated the accounts service
        await _bus.Send(new UpdateAccountBalanceCommand(message.AccountId, newBalance));
        
        // Update the account every `n` minutes
        await _bus.Defer(TimeSpan.FromMinutes(AccountUpdatedMinutes), message);
    }

    public async Task Handle(RequestAccountTransactionsCommand message)
    {
        var connection = await GetConnectionAsync(message.AccountId);

        if (connection is null)
        {
            // Handle failure here
            _logger.LogError("Account connection not found");
            return;
        }
        
        var newTransactions = await _openBankingProvider.GetAccountTransactionsAsync(message.AccountId, connection.ConsentToken);
        
        // Updated the accounts service
        await _bus.Send(new UpdatedAccountTransactionsCommand(message.AccountId, newTransactions));
        
        // Update the account every `n` minutes
        await _bus.Defer(TimeSpan.FromMinutes(AccountUpdatedMinutes), message);
    }

    private async Task<BankAccountConnection?> GetConnectionAsync(Guid accountId)
    {
        return await _context.AccountConnections
            .FindAsync(accountId);
    }
}