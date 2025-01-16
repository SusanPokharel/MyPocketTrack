using MyPocketTrack.Components.Models;
using MyPocketTrack.Components.Storage;

namespace MyPocketTrack.Components.Services
{
    public class TransactionService
    {
        private const string TransactionsFileName = "transactions.json";

        public List<string> PredefinedTags { get; } = new()
        {
            "Income",
            "Housing",
            "Food",
            "Savings",
            "Shopping",
            "Bills",
            "Entertainment"
        };

        private List<Transaction> _transactions = new();

        // Load transactions from file on initialization
        public async Task InitializeAsync()
        {
            _transactions = await FileHandler<Transaction>.ReadFromFile(TransactionsFileName);
        }

        // Get all transactions
        public IEnumerable<Transaction> GetTransactions() => _transactions;

        // Add a transaction and save to file
        public async Task AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
            await FileHandler<Transaction>.SaveToFile(TransactionsFileName, _transactions);
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

        // Save all transactions to file (useful for bulk operations)
        public async Task SaveAllTransactionsAsync()
        {
            await FileHandler<Transaction>.SaveToFile(TransactionsFileName, _transactions);
        }

        // Load transactions from file (can be triggered manually)
        public async Task LoadTransactionsFromFile()
        {
            _transactions = await FileHandler<Transaction>.ReadFromFile(TransactionsFileName);
        }
    }
}
