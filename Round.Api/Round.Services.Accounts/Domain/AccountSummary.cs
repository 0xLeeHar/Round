using Round.Common.Domain;

namespace Round.Services.Accounts.Domain;

public class AccountSummary
{
    public required Guid AccountId { get; set; }
    
    public required DateOnly Date { get; set; }
    
    public required int AmountInMinorUnits { get; set; }
    
    public required BalanceType BalanceType { get; set; }
}