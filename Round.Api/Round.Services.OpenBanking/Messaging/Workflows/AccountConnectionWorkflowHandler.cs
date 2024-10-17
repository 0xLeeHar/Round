using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Round.Common;
using Round.Common.Domain;
using Round.Common.Messaging.Accounts;
using Round.Services.OpenBanking.Messaging.Commands;
using Round.Services.OpenBanking.Messaging.Events;

namespace Round.Services.OpenBanking.Messaging.Workflows;

public class AccountConnectionWorkflowData : ISagaData
{
    public Guid Id { get; set; }
    public int Revision { get; set; }
    
    public Guid RequestId { get; set; }
    
    public Guid? AccountId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? BankId { get; set; }
}

public class AccountConnectionWorkflowHandler : 
    Saga<AccountConnectionWorkflowData>, 
    IAmInitiatedBy<InitiateBackAccountConnectionCommand>,
    IHandleMessages<AccountConsentGrantedEvent>,
    IHandleMessages<BankAccountLinkedEvent>
{
    private readonly IBus _bus;
    private readonly ILogger<AccountConnectionWorkflowHandler> _logger;

    public AccountConnectionWorkflowHandler(IBus bus, ILogger<AccountConnectionWorkflowHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }
    
    protected override void CorrelateMessages(ICorrelationConfig<AccountConnectionWorkflowData> config)
    {
        config.Correlate<InitiateBackAccountConnectionCommand>(m => m.RequestId, d => d.RequestId);
        config.Correlate<AccountConsentGrantedEvent>(m => m.RequestId, d => d.RequestId);
        config.Correlate<BankAccountLinkedEvent>(m => m.RequestId, d => d.RequestId);
    }

    public Task Handle(InitiateBackAccountConnectionCommand message)
    {
        _logger.LogInformation("Initiating account connection request.");
        
        Data.RequestId = message.RequestId;
        Data.TenantId = message.TenantId;
        Data.BankId = message.BankId;
        
        return Task.CompletedTask;
    }

    public async Task Handle(AccountConsentGrantedEvent message)
    {   
        _logger.LogInformation("Account consent granted.");

        // Note: Hard code this for now!
        var accountId = Guid.NewGuid();
        var accountRoutingName = "MR J Jarvis";
        var accountType = BankAccountType.Current;
        var country = Country.GB;
        var currency = "GBP";
        var addresses = new[]
        {
            new BankAccountAddress
            {
                Value = "55566678@112233.payuk"
            }
        };

        Data.AccountId = accountId;

        await _bus.Send(new CreateBankAccountCommand
        (
            accountId,
            Data.BankId!.Value,
            Data.TenantId!.Value,
            accountRoutingName,
            accountType,
            country,
            currency,
            addresses
        ));
        
        await _bus.Send(new LinkObAccountCommand(Data.RequestId, accountId, message.ConsentToken, Data.BankId!.Value));
    }

    public async Task Handle(BankAccountLinkedEvent message)
    {
        await _bus.Send(new RequestAccountBalanceCommand(message.AccountId));
        await _bus.Send(new RequestAccountTransactionsCommand(message.AccountId));
        
        MarkAsComplete();
    }
}