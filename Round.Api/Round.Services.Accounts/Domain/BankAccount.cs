using Round.Common;
using Round.Common.Domain;

namespace Round.Services.Accounts.Domain;

//TODO: Id, BankId, OwnerId, and Currency should be encapsulated in their own types
//Note: Currency here is 1-2-1 with account, some neo-banks, like Revolut, support multi-currency with the same address. Ignore this for now! 
public sealed class BankAccount
{
    public required Guid AccountId { get; set; } 
    public required Guid BankId { get; set; }
    public required Guid OwnerId { get; set; }
    public required string AccountRoutingName { get; set; } 
    public required BankAccountType AccountType { get; set; }
    public required Country Country { get; set; }
    public required string Currency { get; set; }
    public required IEnumerable<BankAccountAddress> Addresses { get; set; }
    
    public AccountBalance? Balance { get; set; }
}

public record AccountBalance
{
    public required int AmountInMinorUnits { get; set; }
    public required DateTime LastUpdatedDate { get; set; }
}