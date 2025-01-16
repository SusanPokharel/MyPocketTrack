
namespace MyPocketTrack.Components.Models;
public class Transaction
{
    public decimal Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public string? Notes { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime Date { get; set; }
    public bool IsDebt { get; set; }
    public DebtStatus? DebtStatus { get; set; }
    public DateTime? DebtTakenDate { get; set; }
    public DateTime? DebtDueDate { get; set; }
    public DateTime? DebtPaidDate { get; set; }
}

public enum TransactionType
{
    Inflow,
    Outflow
}

public enum DebtStatus
{
    Pending,
    Paid,
    Overdue
}
