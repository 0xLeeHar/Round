using System.Text.Json.Serialization;
using Ardalis.SmartEnum;

namespace Round.Common.Domain;

[JsonConverter(typeof(SmartEnumNameConverter<IsoBankTransactionCode, int>))]
public class IsoBankTransactionCode : SmartEnum<IsoBankTransactionCode>
{
    //TODO: This list is not exhaustive
    public static readonly IsoBankTransactionCode ChequeDeposit = new(1, "CHKD", "Cheque Deposit", BalanceType.Credit);
    public static readonly IsoBankTransactionCode CashDeposit = new(2, "CDPT", "Cash Deposit", BalanceType.Credit);
    public static readonly IsoBankTransactionCode DomesticCreditDeposit = new(3, "DMCD", "Domestic Credit Deposit", BalanceType.Credit);
    
    public static readonly IsoBankTransactionCode CashWithdrawal = new(4, "CWDL", "Cash Withdrawal", BalanceType.Debit);
    public static readonly IsoBankTransactionCode DomesticCreditTransfer = new(5, "DMCT", "Domestic Credit Transfer", BalanceType.Debit);
    public static readonly IsoBankTransactionCode CrossBorderCreditTransfer = new(6, "XBCT", "Cross-Border Credit Transfer", BalanceType.Debit);

    public IsoBankTransactionCode(int value, string code, string description, BalanceType balanceType) : base(code, value)
    {
        Description = description;
        BalanceType = balanceType;
    }

    public string Description { get; }
    public BalanceType BalanceType { get; }
}

public enum BalanceType
{
    Credit = 1,
    Debit = 2
}