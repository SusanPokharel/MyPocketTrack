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
                DebtsStatus = Models.DebtsStatus.Pending; // Default status when IsDebt is true
            }
        }
    }

    // Using the DebtsStatus enum
    public DebtsStatus? DebtsStatus { get; set; } // Nullable to allow non-debt transactions
    public DateTime? DebtTakenDate { get; set; }
    public DateTime? DebtDueDate { get; set; }
    public DateTime? DebtPaidDate { get; set; }

    // Default constructor to initialize the transaction date
    public Transaction()
    {
        Date = DateTime.Today;
    }
}