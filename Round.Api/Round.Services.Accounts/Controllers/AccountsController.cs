using Microsoft.AspNetCore.Mvc;
using Round.Common;
using Round.Common.Domain;
using Round.Services.Accounts.Domain;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Controllers;

//TODO: Add API versioning here
[ApiController]
[Route("api/accounts")]
public class AccountsController : BaseController
{
    private readonly IAccountsStore _accountsStore;
    
    public AccountsController(IAccountsStore accountsStore)
    {
        _accountsStore = accountsStore;
    }
    
    [HttpGet]
    public async Task<IEnumerable<BankAccount>> GetAccountsAsync()
    {
        var tenantId = GetTenantId();
        return await _accountsStore.GetAccountsAsync(tenantId);
    }
    
    //NOTE: AccountId is optional here, not following REST routing conventions.
    [HttpGet("transactions")]
    public async Task<IEnumerable<BankAccountTransaction>> GetAccountTransactionsAsync(Guid? accountId)
    {
        var tenantId = GetTenantId();
        return await _accountsStore.GetAccountTransactions(tenantId, accountId);
    }
}