namespace Round.Services.Banks.Domain;

// TODO: This should also include country, etc.
public record Bank(Guid BankId, string Name, string ImageUrl);