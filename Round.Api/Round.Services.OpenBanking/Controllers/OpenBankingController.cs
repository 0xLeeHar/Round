using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Round.Common;
using Round.Services.OpenBanking.Domain;
using Round.Services.OpenBanking.Messaging.Commands;
using Round.Services.OpenBanking.Messaging.Events;

namespace Round.Services.OpenBanking.Controllers;

[ApiController]
[Route("api/open-banking")]
public class OpenBankingController : BaseController
{
    private readonly IBus _bus;

    public OpenBankingController(IBus bus)
    {
        _bus = bus;
    }
    
    [HttpPost("connect")]
    public async Task<RedirectUrl> ConnectToAccount(Guid bankId)
    {
        // Note: There should be some work here to call the OB partner AOT and configure the request.
        
        var tenantId = GetTenantId();
        var requestId = Guid.NewGuid(); // This is some long lived API request id / idempotency key
        
        await _bus.Send(new InitiateBackAccountConnectionCommand(requestId, tenantId, bankId));
        
        // Return redirect URL to selected back
        // TODO: This should be a HTTP 30x redirect, using 200 for testing.
        return new RedirectUrl(Url: $"http://localhost:3000/ob/consent?requestId={requestId}");
    }
    
    // Note: This is the callback from the OB provider, once consent is granted
    [HttpPost("callback")]
    public async Task<IActionResult> CallbackAsync(Guid requestId, string consentToken)
    {
        await _bus.Send(new AccountConsentGrantedEvent(requestId, consentToken));

        return Ok(); // This is responding to the OB webhook
    }
}