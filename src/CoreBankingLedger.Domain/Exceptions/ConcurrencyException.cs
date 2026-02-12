namespace CoreBankingLedger.Domain.Exceptions;

public class ConcurrencyException : Exception
{
    public Guid EntityId { get; }

    public ConcurrencyException(Guid entityId)
        : base($"Concurrency conflict detected for entity {entityId}. The record has been modified by another transaction.")
    {
        EntityId = entityId;
    }
}