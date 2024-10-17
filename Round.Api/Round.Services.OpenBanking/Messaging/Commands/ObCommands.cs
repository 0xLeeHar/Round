namespace Round.Services.OpenBanking.Messaging.Commands;

public record LinkObAccountCommand(Guid RequestId, Guid AccountId, string ConsentToken, Guid BankId);

public record RequestAccountBalanceCommand(Guid AccountId);

public record RequestAccountTransactionsCommand(Guid AccountId);