using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Round.Common;
using Round.Common.Domain;
using Round.Services.Accounts.Domain;

namespace Round.Services.Accounts.Store;

// NOTE: This is NOT the way to seed a database in production code!!!
public class DatabaseSeedStartupCommand : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public DatabaseSeedStartupCommand(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountsContext>();

        await AddBankAccounts(context);
        await AddTransactions(context);
        await AddAccountSummaries(context);
        
        await context.SaveChangesAsync(stoppingToken);
    }

    private async Task AddBankAccounts(AccountsContext context)
    {
        var faker = new Faker();
        
        await context.Accounts.AddRangeAsync(new[]
        {
            new BankAccount
            {
                AccountId = new Guid("a27ddfd1-2270-4882-b692-bd759f43d8e5"),
                BankId = new Guid("dfec4417-a85c-47b6-9f33-4a565f7584a3"),
                OwnerId = new Guid("930607ed-d7a4-45fd-9f3f-2d669dfdf369"),
                AccountRoutingName = "MR LC Harman",
                AccountType = BankAccountType.Current,
                Country = Country.GB,
                Currency = "GBP",
                Addresses = new[]
                {
                    new BankAccountAddress { Value = "12345678@209912.payuk" }
                },
                Balance = new AccountBalance
                {
                    AmountInMinorUnits = faker.Random.Number(48933, 52_000_00),
                    LastUpdatedDate = faker.Date.Past()
                }
            },
            new BankAccount
            {
                AccountId = new Guid("b80ce261-05a5-4e64-8158-04bc38419afb"),
                BankId = new Guid("79e9b4dc-6481-4716-ac73-be59d9c6d2c3"),
                OwnerId = new Guid("930607ed-d7a4-45fd-9f3f-2d669dfdf369"),
                AccountRoutingName = "MR LC Harman",
                AccountType = BankAccountType.Current,
                Country = Country.GB,
                Currency = "GBP",
                Addresses = new[]
                {
                    new BankAccountAddress { Value = "55566678@201211.payuk" }
                },
                Balance = new AccountBalance
                {
                    AmountInMinorUnits = faker.Random.Number(48933, 52_000_00),
                    LastUpdatedDate = faker.Date.Past()
                }
            }
        });
        
        await context.SaveChangesAsync();
    }

    private async Task AddTransactions(AccountsContext context)
    {
        var rand = new Random();
        
        foreach (var account in context.Accounts)
        {
            for (int i = 0; i < 50; i++)
            {
                var faker = new Faker();
                
                await context.Transactions.AddAsync(new BankAccountTransaction
                {
                    TransactionId = new Guid(),
                    AccountId = account.AccountId,
                    AmountInMinorUnits = rand.Next(100, 50_000_00),
                    Currency = "GBP",
                    Description = faker.Person.FullName,
                    Reference = faker.Random.String2(1, 18),
                    SettlementDate = faker.Date.Past(),
                    Status = TransactionStatus.Booked,
                    TransactionCode = IsoBankTransactionCode.FromValue(faker.Random.Number(1, 6))
                });
            }
        }
    }

    private async Task AddAccountSummaries(AccountsContext context)
    {
        var rand = new Random();
        
        foreach (var account in context.Accounts)
        {
            var debits = Enumerable
                .Range(1, 180)
                .Select(s => new AccountSummary
                {
                    AccountId = account.AccountId,
                    AmountInMinorUnits = rand.Next(0, 1200_00),
                    Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-s)),
                    BalanceType = BalanceType.Debit
                });

            await context.AccountSummary.AddRangeAsync(debits);
                
            var credits = Enumerable
                .Range(1, 180)
                .Select(s => new AccountSummary
                {
                    AccountId = account.AccountId,
                    AmountInMinorUnits = rand.Next(0, 1000_00),
                    Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-s)),
                    BalanceType = BalanceType.Credit
                });
            
            await context.AccountSummary.AddRangeAsync(credits);
            
        }
    }
    
}