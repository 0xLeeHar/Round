namespace Round.Services.Accounts.Domain;

// NOTE: The UI shows the delta between months
public sealed class AccountStatistic
{
    public required Guid TenantId { get; set; }
    public required Guid? AccountId { get; set; }

    public required string Currency { get; set; }
    
    /// <summary>
    /// This is a dict of date and agg amounts
    /// Key: Date formatted as `YYMM`
    /// Value: Amount in minor units 
    /// </summary>
    public required Dictionary<string, int> Points { get; set; }
}