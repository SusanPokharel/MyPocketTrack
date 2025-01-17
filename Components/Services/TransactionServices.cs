// using MyPocketTrack.Components.Models;
// using MyPocketTrack.Components.Storage;
//
// namespace MyPocketTrack.Components.Services
// {
//     public class TransactionService
//     {
//         private const string TransactionsFileName = "transactions.json";
//         private const string TagsFileName = "tags.json";
//
//         private List<Transaction> _transactions = new();
//         private List<string> _tags = new();
//
//         public IEnumerable<string> PredefinedTags => _tags;
//
//         // Load transactions and tags from files on initialization
//         public async Task InitializeAsync()
//         {
//             _transactions = await FileHandler<Transaction>.ReadFromFile(TransactionsFileName);
//             _tags = await FileHandler<string>.ReadFromFile(TagsFileName);
//
//             if (!_tags.Any())
//             {
//                 // Default tags if file is empty
//                 _tags = new List<string>
//                 {
//                     "Income", "Housing", "Food", "Savings", "Shopping", "Bills", "Entertainment"
//                 };
//                 await SaveTagsAsync();
//             }
//         }
//
//         // Save transactions to file
//         private async Task SaveTransactionsAsync() =>
//             await FileHandler<Transaction>.SaveToFile(TransactionsFileName, _transactions);
//
//         // Save tags to file
//         private async Task SaveTagsAsync() =>
//             await FileHandler<string>.SaveToFile(TagsFileName, _tags);
//
//         // Get all transactions
//         public IEnumerable<Transaction> GetTransactions() => _transactions;
//
//         // Add a transaction and save to file
//         public async Task AddTransaction(Transaction transaction)
//         {
//             _transactions.Add(transaction);
//             await SaveTransactionsAsync();
//         }
//
//         // Add a new tag and save to file
//         public async Task AddTag(string tag)
//         {
//             if (!_tags.Contains(tag))
//             {
//                 _tags.Add(tag);
//                 await SaveTagsAsync();
//             }
//         }
//
//         // Get top highest transactions
//         public IEnumerable<Transaction> GetTopHighestTransactions(int count) =>
//             _transactions.OrderByDescending(t => t.Amount).Take(count);
//
//         // Get top lowest transactions
//         public IEnumerable<Transaction> GetTopLowestTransactions(int count) =>
//             _transactions.OrderBy(t => t.Amount).Take(count);
//
//         // Filter transactions by type
//         public IEnumerable<Transaction> FilterTransactionsByType(TransactionsStatus type) =>
//             _transactions.Where(t => t.Type == type);
//     }
// }


using MyPocketTrack.Components.Models;
using MyPocketTrack.Components.Storage;

namespace MyPocketTrack.Components.Services
{
    public class TransactionService
    {
        private const string TransactionsFileName = "transactions.json";
        private const string TagsFileName = "tags.json";

        private List<Transaction> _transactions = new();
        private List<string> _tags = new();

        public IEnumerable<string> PredefinedTags => _tags;

        // Load transactions and tags from files on initialization
        public async Task InitializeAsync()
        {
            _transactions = await FileHandler<Transaction>.ReadFromFile(TransactionsFileName);
            _tags = await FileHandler<string>.ReadFromFile(TagsFileName);

            if (!_tags.Any())
            {
                // Default tags if file is empty
                _tags = new List<string>
                {
                    "Income", "Housing", "Food", "Savings", "Shopping", "Bills", "Entertainment"
                };
                await SaveTagsAsync();
            }
        }

        // Save transactions to file
        private async Task SaveTransactionsAsync() =>
            await FileHandler<Transaction>.SaveToFile(TransactionsFileName, _transactions);

        // Save tags to file
        private async Task SaveTagsAsync() =>
            await FileHandler<string>.SaveToFile(TagsFileName, _tags);

        // Get all transactions
        public IEnumerable<Transaction> GetTransactions() => _transactions;

        // Add a transaction and save to file
        public async Task AddTransaction(Transaction transaction)
        {
            if (transaction.Type == TransactionsStatus.Debt && transaction.DebtsStatus == null)
            {
                transaction.DebtsStatus = DebtsStatus.Pending; 
            }
            _transactions.Add(transaction);
            await SaveTransactionsAsync();
        }

        // Add a new tag and save to file
        public async Task AddTag(string tag)
        {
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
                await SaveTagsAsync();
            }
        }
        

        // Get top highest transactions
        public IEnumerable<Transaction> GetTopHighestTransactions(int count) =>
            _transactions.OrderByDescending(t => t.Amount).Take(count);

        // Get top lowest transactions
        public IEnumerable<Transaction> GetTopLowestTransactions(int count) =>
            _transactions.OrderBy(t => t.Amount).Take(count);

        // Filter transactions by type
        public IEnumerable<Transaction> FilterTransactionsByType(TransactionsStatus type) =>
            _transactions.Where(t => t.Type == type);

        // Get transactions by debt status
        public IEnumerable<Transaction> GetDebtsByStatus(DebtsStatus status) =>
            _transactions.Where(t => t.Type == TransactionsStatus.Debt && t.DebtsStatus == status);

// Calculated Properties
        public decimal TotalInflow => _transactions.Where(t => t.Type == TransactionsStatus.Inflow).Sum(t => t.Amount);
        public int TotalInflowCount => _transactions.Count(t => t.Type == TransactionsStatus.Inflow);

        public decimal TotalOutflow => _transactions.Where(t => t.Type == TransactionsStatus.Outflow).Sum(t => t.Amount);
        public int TotalOutflowCount => _transactions.Count(t => t.Type == TransactionsStatus.Outflow);

        public decimal TotalDebt => _transactions.Where(t => t.Type == TransactionsStatus.Debt).Sum(t => t.Amount);
        public int TotalDebtCount => _transactions.Count(t => t.Type == TransactionsStatus.Debt);

        public decimal RemainingDebt => TotalDebt - _transactions
            .Where(t => t.Type == TransactionsStatus.Debt && t.DebtsStatus == DebtsStatus.Paid)
            .Sum(t => t.Amount);

        public decimal ClearedDebt => _transactions
            .Where(t => t.Type == TransactionsStatus.Debt && t.DebtsStatus == DebtsStatus.Paid)
            .Sum(t => t.Amount);

        public decimal RemainingBalance =>
            Math.Max(TotalInflow - (TotalOutflow + ClearedDebt), 0);
        
// Total transaction count
        public int TotalTransactionCount => _transactions.Count();
        
        
        // Get total overdue debt
        public decimal GetTotalOverdueDebt() =>
            _transactions.Where(t => t.Type == TransactionsStatus.Debt && t.DebtsStatus == DebtsStatus.Overdue).Sum(t => t.Amount);

        
        private IEnumerable<Transaction> FilteredPendingDebts => 
            GetDebtsByStatus(DebtsStatus.Pending).Where(t => t.DebtsStatus == DebtsStatus.Pending);
        
        public async Task<string> ClearDebt(Transaction debtTransaction, decimal remainingAmount)
        {
            if (debtTransaction.Type != TransactionsStatus.Debt || debtTransaction.DebtsStatus != DebtsStatus.Pending)
            {
                return "This transaction is not a pending debt.";
            }
            if (remainingAmount < debtTransaction.Amount)
            {
                return "Insufficient remaining balance to clear the debt.";
            }
            // Mark the debt as paid
            debtTransaction.DebtsStatus = DebtsStatus.Paid;
            debtTransaction.DebtPaidDate = DateTime.Now;

            await SaveTransactionsAsync();
            return "Debt cleared successfully.";
        }
        
        
        // Mark a debt as paid
        public async Task<string> MarkDebtAsPaid(Transaction debtTransaction)
        {
            if (debtTransaction.Type != TransactionsStatus.Debt || debtTransaction.DebtsStatus != DebtsStatus.Pending)
            {
                return "Transaction is not a pending debt.";
            }

            debtTransaction.DebtsStatus = DebtsStatus.Paid;
            debtTransaction.DebtPaidDate = DateTime.Now;

            await SaveTransactionsAsync();
            return "Debt marked as paid.";
        }

        // Mark debts as overdue based on due dates
        public async Task CheckAndUpdateOverdueDebts()
        {
            var overdueDebts = _transactions
                .Where(t => t.Type == TransactionsStatus.Debt && t.DebtsStatus == DebtsStatus.Pending && t.DebtDueDate.HasValue && t.DebtDueDate < DateTime.Now);

            foreach (var debt in overdueDebts)
            {
                debt.DebtsStatus = DebtsStatus.Overdue;
            }

            await SaveTransactionsAsync();
        }
    }
}
