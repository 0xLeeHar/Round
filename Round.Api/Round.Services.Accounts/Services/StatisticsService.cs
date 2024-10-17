using Microsoft.EntityFrameworkCore;
using Round.Common;
using Round.Common.Domain;
using Round.Services.Accounts.Domain;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Services;

public interface IStatisticsService
{
    Task<AccountStatistic> GetRunwayAsync(Guid tenantId, Guid? accountId);
    Task<AccountStatistic> GetSpendAsync(Guid tenantId, Guid? accountId);
    Task<AccountStatistic> GetIncomeAsync(Guid tenantId, Guid? accountId);
}

public class StatisticsService : IStatisticsService
{
    // TODO: This only works in GBP, for now. Will need updating for prod code!
    const string Currency = "GBP";
    
    private readonly AccountsContext _context;
    private readonly IAccountsStore _accountsStore;
    private readonly ITimeProvider _timeProvider;

    public StatisticsService(AccountsContext context, IAccountsStore accountsStore, ITimeProvider timeProvider)
    {
        _context = context;
        _accountsStore = accountsStore;
        _timeProvider = timeProvider;
    }
    
    public async Task<AccountStatistic> GetRunwayAsync(Guid tenantId, Guid? accountId)
    {
        var spend = await GetSpendAsync(tenantId, accountId);
        var income = await GetIncomeAsync(tenantId, accountId);

        var avgSpend = spend.Points.Average(s => s.Value);
        var avgIncome = income.Points.Average(s => s.Value);
        var avgMonthlyDelta = (int)Math.Round(avgIncome - avgSpend);

        var accounts = await _accountsStore.GetAccountsAsync(tenantId);
        var totalBalance = accounts
            .Where(w => w.Currency == Currency
                && (accountId == null || w.AccountId == accountId)) //TODO: Support multi-currency
            .Sum(s => s.Balance?.AmountInMinorUnits ?? 0);

        var points = new Dictionary<DateOnly, int>();

        points.Add(DateOnly.FromDateTime(_timeProvider.GetUtcNow()), totalBalance - avgMonthlyDelta);
        
        for (int i = 0; i < 6; i++)
        {
            var lastPoint = points.LastOrDefault();
            
            points.Add(lastPoint.Key.AddMonths(1), lastPoint.Value - avgMonthlyDelta);
        }

        return new AccountStatistic
        {
            TenantId = tenantId,
            AccountId = accountId,
            Currency = Currency,
            Points = points.ToDictionary(k => k.Key.ToString("yyyy-MM"), b => b.Value)
        };
    }
    
    public async Task<AccountStatistic> GetSpendAsync(Guid tenantId, Guid? accountId)
    {
        return await GetMonthlyAccountStatisticAsync(tenantId, BalanceType.Debit, accountId);
    }
    
    public async Task<AccountStatistic> GetIncomeAsync(Guid tenantId, Guid? accountId)
    {
        return await GetMonthlyAccountStatisticAsync(tenantId, BalanceType.Credit, accountId);
    }

    private async Task<AccountStatistic> GetMonthlyAccountStatisticAsync(Guid tenantId, BalanceType balanceType, Guid? accountId)
    {
        var dailySummaries = await GetAccountSummariesAsync(tenantId, balanceType, Currency, 6, accountId);

        var monthlyPoints = dailySummaries
            .GroupBy(g => g.Date.ToString("yyyy-MM"))
            .ToDictionary(k => k.Key, v => v.Sum(x => x.AmountInMinorUnits));
        
        return new AccountStatistic
        {
            TenantId = tenantId,
            AccountId = accountId,
            Currency = Currency,
            Points = monthlyPoints
        };
    }

    private async Task<List<AccountSummary>> GetAccountSummariesAsync(Guid tenantId, BalanceType balanceType, string currency, int historicalMonths, Guid? accountId)
    {
        var firstDate = DateOnly.FromDateTime(_timeProvider.GetUtcNow().AddMonths(historicalMonths * -1));
        
        return await _context.Accounts
            .Where(w => w.OwnerId == tenantId 
                        && w.Currency == currency
                        && (accountId == null || w.AccountId == accountId))
            .Join(_context.AccountSummary, account => account.AccountId, accSum => accSum.AccountId, (account, accSum) => accSum)
            .Where(w => w.BalanceType == balanceType
                && w.Date > firstDate)
            .ToListAsync();
    }
}