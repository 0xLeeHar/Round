namespace Round.Services.OpenBanking.Messaging.Events;

public record BankAccountLinkedEvent(Guid RequestId, Guid AccountId);