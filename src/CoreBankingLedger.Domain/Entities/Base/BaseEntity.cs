namespace CoreBankingLedger.Domain.Entities.Base;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAtUtc { get; protected set; }
    public DateTime? UpdatedAtUtc { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAtUtc = DateTime.UtcNow;
    }

    protected void MarkAsModified()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}