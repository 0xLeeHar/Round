using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Round.Common;
using Round.Common.Messaging.Accounts;
using Round.Services.Accounts.Extensions;
using Round.Services.Accounts.Messaging.Commands;
using Round.Services.OpenBanking.Extensions;
using Round.Services.OpenBanking.Messaging.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add our 'microservices' here, for now.
builder.Services.AddTransient<ITimeProvider, TimeProvider>(); // This is now in .NET 8
builder.Services.AddAccountsService(builder.Configuration);
builder.Services.AddOpenBankingService(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string queueName = "round-queue";

builder.Services.AddRebus(config =>
    config
        .Transport(t => t.UseInMemoryTransport(new InMemNetwork(true), queueName))
        .Logging(l => l.ColoredConsole())
        .Sagas(s => s.StoreInMemory())
        .Timeouts(t => t.StoreInMemory())
        .Routing(cf =>
        {
            cf.TypeBased()
                .MapAssemblyOf<UpdateAccountStatisticsCommand>(queueName)
                .MapAssemblyOf<CreateBankAccountCommand>(queueName)
                .MapAssemblyOf<InitiateBackAccountConnectionCommand>(queueName);
        })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();