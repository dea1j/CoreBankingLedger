using CoreBankingLedger.Domain.Entities.Base;
using CoreBankingLedger.Domain.Enums;

namespace CoreBankingLedger.Domain.Entities;

public class Transaction : BaseEntity
{
    public string Reference { get; private set; } = string.Empty;
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    public string? Description { get; private set; }
    public string? Metadata { get; private set; }
    public DateTime ProcessedAtUtc { get; private set; }

    private readonly List<LedgerEntry> _ledgerEntries = new();
    public IReadOnlyCollection<LedgerEntry> LedgerEntries => _ledgerEntries.AsReadOnly();

    private Transaction() { }

    public static Transaction CreateDeposit(
        string reference,
        decimal amount,
        Currency currency,
        string? description = null,
        string? metadata = null)
    {
        ValidateTransactionCreation(reference, amount);

        return new Transaction
        {
            Reference = reference,
            Type = TransactionType.Deposit,
            Amount = amount,
            Currency = currency,
            Description = description,
            Metadata = metadata,
            ProcessedAtUtc = DateTime.UtcNow
        };
    }

    public static Transaction CreateWithdrawal(
        string reference,
        decimal amount,
        Currency currency,
        string? description = null,
        string? metadata = null)
    {
        ValidateTransactionCreation(reference, amount);

        return new Transaction
        {
            Reference = reference,
            Type = TransactionType.Withdrawal,
            Amount = amount,
            Currency = currency,
            Description = description,
            Metadata = metadata,
            ProcessedAtUtc = DateTime.UtcNow
        };
    }

    public static Transaction CreateTransfer(
        string reference,
        decimal amount,
        Currency currency,
        string? description = null,
        string? metadata = null)
    {
        ValidateTransactionCreation(reference, amount);

        return new Transaction
        {
            Reference = reference,
            Type = TransactionType.Transfer,
            Amount = amount,
            Currency = currency,
            Description = description,
            Metadata = metadata,
            ProcessedAtUtc = DateTime.UtcNow
        };
    }

    public void AddDebitEntry(Account account, decimal amount, decimal balanceAfter)
    {
        if (amount <= 0)
            throw new ArgumentException("Debit amount must be positive.", nameof(amount));

        // The minus sign (-) is applied here before sending to the LedgerEntry factory
        var entry = LedgerEntry.Create(this, account, -amount, balanceAfter);
        _ledgerEntries.Add(entry);
    }

    public void AddCreditEntry(Account account, decimal amount, decimal balanceAfter)
    {
        if (amount <= 0)
            throw new ArgumentException("Credit amount must be positive.", nameof(amount));

        var entry = LedgerEntry.Create(this, account, amount, balanceAfter);
        _ledgerEntries.Add(entry);
    }

    private static void ValidateTransactionCreation(string reference, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Transaction reference is required.", nameof(reference));

        if (amount <= 0)
            throw new ArgumentException("Transaction amount must be positive.", nameof(amount));
    }
}