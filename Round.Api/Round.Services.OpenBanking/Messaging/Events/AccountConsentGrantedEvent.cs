namespace Round.Services.OpenBanking.Messaging.Events;

public record AccountConsentGrantedEvent(Guid RequestId, string ConsentToken);