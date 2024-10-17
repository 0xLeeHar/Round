using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Round.Services.OpenBanking.Messaging.Handlers;
using Round.Services.OpenBanking.Messaging.Services;
using Round.Services.OpenBanking.Messaging.Workflows;
using Round.Services.OpenBanking.Store;

namespace Round.Services.OpenBanking.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddOpenBankingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IOpenBankingProvider, OpenBankingProvider>();
        
        services.AddDbContext<OpenBankingContext>(opts =>
        {
            //TODO: This is an in-memory DB and should be hosted in the real world.
            opts.UseInMemoryDatabase("RoundDB-OB");
        });

        // Handlers
        services.AddTransient<AccountConnectionWorkflowHandler>();
        services.AddTransient<LinkObAccountHandler>();
        services.AddTransient<UpdateAccountHandler>();
        
        services.AddRebusHandler<AccountConnectionWorkflowHandler>();
        services.AddRebusHandler<LinkObAccountHandler>();
        services.AddRebusHandler<UpdateAccountHandler>();
    }   
}