using Round.Common.Domain;

namespace Round.Common.Messaging.Accounts;

public record CreateBankAccountCommand(
    Guid AccountId,
    Guid BankId,
    Guid OwnerId,
    string AccountRoutingName,
    BankAccountType AccountType,
    Country Country,
    string Currency,
    IEnumerable<BankAccountAddress> Addresses);


public record UpdateAccountBalanceCommand(Guid AccountId, MoneyValue Balance);

public record UpdatedAccountTransactionsCommand(Guid AccountId, IEnumerable<BankAccountTransaction> Transactions);