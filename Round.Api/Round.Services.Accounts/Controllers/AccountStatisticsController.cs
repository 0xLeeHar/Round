using Microsoft.AspNetCore.Mvc;
using Round.Common;
using Round.Services.Accounts.Domain;
using Round.Services.Accounts.Services;

namespace Round.Services.Accounts.Controllers;

[ApiController]
[Route("api/accounts/statistics")]
public class AccountStatisticsController : BaseController
{
    private readonly IStatisticsService _statisticsService;

    public AccountStatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    //NOTE: AccountId is optional here, null will return all accounts.
    [HttpGet("runway")]
    public async Task<AccountStatistic> GetRunway(Guid? accountId)
    {
        var tenantId = GetTenantId();
        return await _statisticsService.GetRunwayAsync(tenantId, accountId);
    }
    
    [HttpGet("spend")]
    public async Task<AccountStatistic> GetSpend(Guid? accountId)
    {
        var tenantId = GetTenantId();
        return await _statisticsService.GetSpendAsync(tenantId, accountId);
    }
    
    [HttpGet("income")]
    public async Task<AccountStatistic> GetIncome(Guid? accountId)
    {
        var tenantId = GetTenantId();
        return await _statisticsService.GetIncomeAsync(tenantId, accountId);
    }
}