namespace CoreBankingLedger.Domain;

public interface IAccountRepository
{
    Task<Entities.Account> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.Account> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    void Update(Entities.Account account);
}