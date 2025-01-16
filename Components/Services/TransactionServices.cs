using System.Collections.Generic;
using System.Linq;
using MyPocketTrack.Components.Models;

namespace MyPocketTrack.Components.Services
{
    public class TransactionService
    {
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
        
        private readonly List<Transaction> _transactions = new();

        public IEnumerable<Transaction> GetTransactions() => _transactions;

        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public IEnumerable<Transaction> GetTopHighestTransactions(int count) =>
            _transactions.OrderByDescending(t => t.Amount).Take(count);

        public IEnumerable<Transaction> GetTopLowestTransactions(int count) =>
            _transactions.OrderBy(t => t.Amount).Take(count);
        
        public IEnumerable<Transaction> FilterTransactionsByType(TransactionType type) =>
            _transactions.Where(t => t.Type == type);

    
        public IEnumerable<Transaction> GetDebtTransactions() =>
            _transactions.Where(t => t.IsDebt);
    }
}