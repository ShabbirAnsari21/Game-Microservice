using System.ComponentModel.DataAnnotations;

namespace ApplicationSystem.DbModels.ATMSystem
{
    // Represents a transaction (either adding or withdrawing cash) at an ATM.
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; } // Primary Key

        // Foreign key linking this transaction to a specific ATM.
        public int ATMId { get; set; } // Foreign Key

        // Timestamp of when the transaction occurred.
        public DateTime Timestamp { get; set; }

        // Type of transaction, either "ADD" or "WITHDRAW".
        public string Type { get; set; }

        // Total amount of cash involved in the transaction.
        public int Amount { get; set; }

        // Collection of denominations used in this transaction.
        public virtual ICollection<TransactionDenomination> TransactionDenominations { get; set; }

        // Navigation property for relationship with the ATM entity.
        public virtual ATM ATM { get; set; }
    }
}
