using ApplicationSystem.DbModels.ATMSystem;

namespace ApplicationSystem.Repository.ATMSystemRepository
{
    public class ATMRepository : IATMRepository
    {
        private static readonly ATM _atm = new ATM(); // In-memory ATM object
        private static int _transactionIdCounter = 1; // Static counter for transaction IDs
        private static int _transactionDenomIdCounter = 1; // Static counter for transaction denomination IDs

        public void AddCash(Dictionary<int, int> denominations)
        {
            // Update the ATM's denominations
            foreach (var denomination in denominations)
            {
                var existingDenomination = _atm.Denominations.FirstOrDefault(d => d.DenominationValue == denomination.Key);
                if (existingDenomination != null)
                {
                    // Update count if the denomination exists
                    existingDenomination.Count += denomination.Value;
                }
                else
                {
                    // Add new denomination if it doesn't exist
                    _atm.Denominations.Add(new Denomination
                    {
                        DenominationValue = denomination.Key,
                        Count = denomination.Value
                    });
                }
            }

            // Add a transaction for tracking
            var transaction = new Transaction
            {
                TransactionId = _transactionIdCounter++, // Assign a unique transaction ID
                ATMId = 1, // Assuming a single ATM instance
                Timestamp = DateTime.Now,
                Type = "ADD",
                Amount = denominations.Sum(d => d.Key * d.Value),
                TransactionDenominations = denominations.Select(d => new TransactionDenomination
                {
                    TransactionDenomId = _transactionDenomIdCounter++, // Assign unique denomination ID
                    TransactionId = _transactionIdCounter - 1, // Use the same transaction ID
                    DenominationValue = d.Key,
                    Count = d.Value
                }).ToList()
            };

            _atm.Transactions.Add(transaction);
        }


        public Dictionary<int, int> WithdrawCash(int amount)
        {
            var atm = _atm;

            if (amount > ATM.MaxTransactionLimit)
            {
                throw new ArgumentException("Entered amount exceeds maximum transaction limit.");
            }

            if (!IsValidAmount(amount, atm.Denominations))
            {
                throw new ArgumentException("Invalid amount entered.");
            }

            var dispenseResult = DispenseCash(amount, atm.Denominations);
            if (dispenseResult == null)
            {
                throw new InvalidOperationException("Entered denomination not available or insufficient funds.");
            }

            // Add transaction
            atm.Transactions.Add(new Transaction
            {
                TransactionId = _transactionIdCounter++,
                ATMId = 1,
                Timestamp = DateTime.Now,
                Type = "WITHDRAW",
                Amount = amount,
                TransactionDenominations = dispenseResult.Select(r => new TransactionDenomination
                {
                    TransactionDenomId = _transactionDenomIdCounter++, // Assign a unique denomination ID
                    TransactionId = _transactionIdCounter - 1, // Use the same transaction ID
                    DenominationValue = r.Key,
                    Count = r.Value
                }).ToList()
            });

            return dispenseResult;
        }


        private Dictionary<int, int> DispenseCash(int amount, ICollection<Denomination> denominations)
        {
            var result = new Dictionary<int, int>();
            var remainingAmount = amount;

            // Use a temporary copy of the denominations to avoid modifying the original until the withdrawal is successful
            var tempDenominations = denominations
                .Select(d => new { d.DenominationValue, Count = d.Count })
                .OrderByDescending(d => d.DenominationValue)
                .ToList();

            foreach (var denomination in tempDenominations)
            {
                var count = Math.Min(remainingAmount / denomination.DenominationValue, denomination.Count);
                if (count > 0)
                {
                    result[denomination.DenominationValue] = count;
                    remainingAmount -= denomination.DenominationValue * count;
                }
            }

            // If the amount cannot be fully dispensed, return null
            if (remainingAmount != 0)
            {
                return null;
            }

            // Deduct from the actual denominations only if the withdrawal is successful
            foreach (var kvp in result)
            {
                var denomination = denominations.First(d => d.DenominationValue == kvp.Key);
                denomination.Count -= kvp.Value;
            }

            return result;
        }


        public Dictionary<int, int> GetNodesSummary()
        {
            // Fetch the current state of denominations in the ATM
            return _atm.Denominations.ToDictionary(d => d.DenominationValue, d => d.Count);
        }

        public List<Transaction?> GetTransactions()
        {
            // Fetch all transactions and map them to a response format
            return _atm.Transactions.ToList();
        }



        private bool IsValidAmount(int amount, ICollection<Denomination> denominations)
        {
            return denominations.Any(d => amount % d.DenominationValue == 0);
        }
    }
}
