using Rebus.Bus;
using Rebus.Handlers;
using Round.Services.OpenBanking.Domain;
using Round.Services.OpenBanking.Messaging.Commands;
using Round.Services.OpenBanking.Messaging.Events;
using Round.Services.OpenBanking.Store;

namespace Round.Services.OpenBanking.Messaging.Handlers;

public class LinkObAccountHandler : IHandleMessages<LinkObAccountCommand>
{
    private readonly IBus _bus;
    private readonly OpenBankingContext _context;

    public LinkObAccountHandler(IBus bus, OpenBankingContext context)
    {
        _bus = bus;
        _context = context;
    }
    
    public async Task Handle(LinkObAccountCommand message)
    {
        _context.Add(new BankAccountConnection
        {
            AccountId = message.AccountId,
            BankId = message.BankId,
            ConsentToken = message.ConsentToken
        });

        await _context.SaveChangesAsync();
        await _bus.Send(new BankAccountLinkedEvent(message.RequestId, message.AccountId));
    }
}