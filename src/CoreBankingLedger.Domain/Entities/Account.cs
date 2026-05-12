using CoreBankingLedger.Domain.Entities.Base;
using CoreBankingLedger.Domain.Enums;
using CoreBankingLedger.Domain.Exceptions;

namespace CoreBankingLedger.Domain.Entities;

public class Account : BaseEntity
{
    public Guid UserId { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public Currency Currency { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();
    public bool IsActive { get; private set; }
    private readonly List<LedgerEntry> _ledgerEntries = new();
    public IReadOnlyCollection<LedgerEntry> LedgerEntries => _ledgerEntries.AsReadOnly();

    private Account() { }

    public static Account Create(Guid userId, string accountNumber, Currency currency)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number is required.", nameof(accountNumber));

        return new Account
        {
            UserId = userId,
            AccountNumber = accountNumber,
            Currency = currency,
            CurrentBalance = 0m,
            IsActive = true
        };
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Credit amount must be positive.", nameof(amount));

        if (!IsActive)
            throw new InvalidOperationException($"Account {AccountNumber} is not active.");

        CurrentBalance += amount;
        MarkAsModified();
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Debit amount must be positive.", nameof(amount));

        if (!IsActive)
            throw new InvalidOperationException($"Account {AccountNumber} is not active.");

        if (CurrentBalance < amount)
            throw new InsufficientFundsException(Id, amount, CurrentBalance);

        CurrentBalance -= amount;
        MarkAsModified();
    }

    internal void AddLedgerEntry(LedgerEntry entry)
    {
        _ledgerEntries.Add(entry);
    }

    public void Deactivate()
    {
        if (CurrentBalance != 0)
            throw new InvalidOperationException("Cannot deactivate account with non-zero balance.");

        IsActive = false;
        MarkAsModified();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsModified();
    }
}