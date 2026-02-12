namespace CoreBankingLedger.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public Guid AccountId { get; }
    public decimal RequestedAmount { get; }
    public decimal AvailableBalance { get; }
    
    public InsufficientFundsException(Guid accountId, decimal requestedAmount, decimal availableBalance)
        : base($"Insufficient funds. Account {accountId} has {availableBalance:C} but {requestedAmount:C} was requested.")
    {
        AccountId = accountId;
        RequestedAmount = requestedAmount;
        AvailableBalance = availableBalance;
    }
}