namespace Round.Services.OpenBanking.Messaging.Commands;

public record InitiateBackAccountConnectionCommand(Guid RequestId, Guid TenantId, Guid BankId);