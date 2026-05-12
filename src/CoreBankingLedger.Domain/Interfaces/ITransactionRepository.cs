namespace CoreBankingLedger.Domain.Interfaces;

public interface ITransactionRepository
{
    void Add(Entities.Transaction transaction);
    Task<Entities.Transaction?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default);
}