using Round.Common;
using Round.Common.Domain;

namespace Round.Services.OpenBanking.Messaging.Services;

// NOTE: This is the interface out to the open banking provider!
// TODO: This is stubbed for now
public interface IOpenBankingProvider
{
    Task<MoneyValue> GetAccountBalanceAsync(string consentToken);
    
    Task<IEnumerable<BankAccountTransaction>> GetAccountTransactionsAsync(Guid accountId, string consentToken);
}

public class OpenBankingProvider : IOpenBankingProvider
{
    public Task<MoneyValue> GetAccountBalanceAsync(string consentToken)
    {
        return Task.FromResult(new MoneyValue("GBP", 38_992_83));
    }

    public async Task<IEnumerable<BankAccountTransaction>> GetAccountTransactionsAsync(Guid accountId, string consentToken)
    {
        var newTransactions = new []
        {
            new BankAccountTransaction
            {
                AccountId = accountId,
                AmountInMinorUnits = 1_930_83,
                Currency = "GBP",
                Description = "MR J James",
                Reference = "99fj-sdl",
                SettlementDate = DateTime.UtcNow,
                Status = TransactionStatus.Booked,
                TransactionCode = IsoBankTransactionCode.DomesticCreditDeposit,
                TransactionId = Guid.NewGuid()
            },
            new BankAccountTransaction
            {
                AccountId = accountId,
                AmountInMinorUnits = 30_13,
                Currency = "GBP",
                Description = "Sainsbury",
                Reference = "839030",
                SettlementDate = DateTime.UtcNow,
                Status = TransactionStatus.Booked,
                TransactionCode = IsoBankTransactionCode.CashDeposit,
                TransactionId = Guid.NewGuid()
            },
            new BankAccountTransaction
            {
                AccountId = accountId,
                AmountInMinorUnits = 2934_00,
                Currency = "GBP",
                Description = "MISS A Another",
                Reference = "LFIJKNSIF",
                SettlementDate = DateTime.UtcNow,
                Status = TransactionStatus.Booked,
                TransactionCode = IsoBankTransactionCode.DomesticCreditTransfer,
                TransactionId = Guid.NewGuid()
            }
        };
        
        return await Task.FromResult(newTransactions);
    }
}