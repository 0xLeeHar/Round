namespace Round.Services.OpenBanking.Domain;

public sealed class BankAccountConnection
{
    public required Guid AccountId { get; set; }
    
    public required Guid BankId { get; set; }
    
    public required string ConsentToken { get; set; }
    
    // TODO: This should have status, provider, last used date, etc.
}