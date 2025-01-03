using System.ComponentModel.DataAnnotations;

namespace ApplicationSystem.DbModels.ATMSystem
{
    // Represents the specific denominations used in a transaction.
    public class TransactionDenomination
    {
        [Key]
        public int TransactionDenomId { get; set; } // Primary Key

        // Foreign key linking to the Transaction.
        public int TransactionId { get; set; } // Foreign Key

        // Value of the denomination, e.g., 100, 500.
        public int DenominationValue { get; set; }

        // Number of notes of this denomination used in the transaction.
        public int Count { get; set; }

        // Navigation property for relationship with the Transaction entity.
        public virtual Transaction Transaction { get; set; }
    }
}
