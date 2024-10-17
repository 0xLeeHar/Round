namespace Round.Common.Domain;

public sealed class BankAccountTransaction
{
    public required Guid TransactionId { get; set; } 
    
    public required Guid AccountId { get; set; }
    
    public required DateTime SettlementDate { get; set; }
    
    public required string Currency { get; set; }
    
    public required int AmountInMinorUnits { get; set; }
    
    public string? Description { get; set; }
    public string? Reference { get; set; }
    
    public required TransactionStatus Status { get; set; }
    
    // This code determines if the value is credit/debit 
    public required IsoBankTransactionCode TransactionCode { get; set; }
}

public enum TransactionStatus
{ 
    Unknown = 0,
    Pending = 1,
    Booked = 2
}