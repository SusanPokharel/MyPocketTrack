//
// namespace MyPocketTrack.Components.Models;
// public class Transaction
// {
//     public decimal Amount { get; set; }
//     public string Source { get; set; } = string.Empty;
//     public string Description { get; set; } = string.Empty;
//     public TransactionType Type { get; set; }
//     public string? Notes { get; set; }
//     public List<string> Tags { get; set; } = new();
//     public DateTime Date { get; set; }
//     public bool IsDebt { get; set; }
//     public DebtStatus? DebtStatus { get; set; }
//     public DateTime? DebtTakenDate { get; set; }
//     public DateTime? DebtDueDate { get; set; }
//     public DateTime? DebtPaidDate { get; set; }
//     
// }
//
// public enum TransactionType
// {
//     Inflow,
//     Outflow,
//     Debt
// }
//
// public enum DebtStatus
// {
//     Pending,
//     Paid,
//     Overdue
// }


namespace MyPocketTrack.Components.Models;

public class Transaction
{
    public decimal Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TransactionsStatus Type { get; set; }
    public string? Notes { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime Date { get; set; }

    private bool _isDebt;
    public bool IsDebt
    {
        get => _isDebt;
        set
        {
            _isDebt = value;
            if (_isDebt)
            {
                DebtStatus = DebtsStatus.Pending;
            }
        }
    }

    public DebtsStatus? DebtStatus { get; set; }
    public DateTime? DebtTakenDate { get; set; }
    public DateTime? DebtDueDate { get; set; }
    public DateTime? DebtPaidDate { get; set; }

    public Transaction()
    {
        Date = DateTime.Today;
    }
}