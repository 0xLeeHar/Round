using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using Round.Common.Messaging.Accounts;
using Round.Services.Accounts.Domain;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Messaging.Handlers;

public class CreateBankAccountHandler : IHandleMessages<CreateBankAccountCommand>
{
    private readonly AccountsContext _context;
    private readonly ILogger<CreateBankAccountHandler> _logger;

    public CreateBankAccountHandler(AccountsContext context, ILogger<CreateBankAccountHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task Handle(CreateBankAccountCommand message)
    {
        _logger.LogInformation("Creating bank account");

        await _context.Accounts.AddAsync(new BankAccount
        {
            AccountId = message.AccountId,
            BankId = message.BankId,
            OwnerId = message.OwnerId,
            AccountRoutingName = message.AccountRoutingName,
            AccountType = message.AccountType,
            Country = message.Country,
            Currency = message.Currency,
            Addresses = message.Addresses
        });
        
        await _context.SaveChangesAsync();
    }
}