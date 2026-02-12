using CoreBankingLedger.Domain.Entities.Base;

namespace CoreBankingLedger.Domain.Entities;

public class LedgerEntry : BaseEntity
{
    public Guid TransactionId { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Amount { get; private set; }
    public decimal BalanceAfter { get; private set; }
    public long SequenceNumber { get; private set; }
    public Transaction Transaction { get; private set; } = null!;
    public Account Account { get; private set; } = null!;
    
    private LedgerEntry() { }
    
    internal static LedgerEntry Create(
        Transaction transaction, 
        Account account, 
        decimal amount, 
        decimal balanceAfter)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
        
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        var entry = new LedgerEntry
        {
            TransactionId = transaction.Id,
            AccountId = account.Id,
            Amount = amount,
            BalanceAfter = balanceAfter,
            Transaction = transaction,
            Account = account
        };

        account.AddLedgerEntry(entry);

        return entry;
    }
}