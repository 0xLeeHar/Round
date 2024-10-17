using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Round.Services.Accounts.Messaging.Handlers;
using Round.Services.Accounts.Services;
using Round.Services.Accounts.Store;

namespace Round.Services.Accounts.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAccountsService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAccountsStore, AccountsStore>();
        services.AddTransient<IStatisticsService, StatisticsService>();
        
        services.AddDbContext<AccountsContext>(opts =>
        {
            //TODO: This is an in-memory DB and should be hosted in the real world.
            opts.UseInMemoryDatabase("RoundDB-Acc");
        });

        //services.AddHostedService<DatabaseSeedStartupCommand>();

        // Handlers
        services.AddTransient<CreateBankAccountHandler>();
        services.AddTransient<UpdateAccountBalanceHandler>();
        services.AddTransient<UpdatedAccountTransactionsHandler>();
        services.AddTransient<UpdateAccountStatisticsHandler>();
        
        services.AddRebusHandler<CreateBankAccountHandler>();
        services.AddRebusHandler<UpdateAccountBalanceHandler>();
        services.AddRebusHandler<UpdatedAccountTransactionsHandler>();
        services.AddRebusHandler<UpdateAccountStatisticsHandler>();
    }   
}