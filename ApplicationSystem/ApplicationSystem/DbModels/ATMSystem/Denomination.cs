using System.ComponentModel.DataAnnotations;

namespace ApplicationSystem.DbModels.ATMSystem
{
    // This class represents the cash denominations (e.g., 100, 500) stored in the ATM.
    public class Denomination
    {
        [Key]
        public int DenominationId { get; set; } // Primary Key

        // Foreign key linking this denomination to a specific ATM.
        public int ATMId { get; set; } // Foreign Key

        // Value of the denomination, e.g., 100, 500.
        public int DenominationValue { get; set; }

        // Number of notes of this denomination currently in the ATM.
        public int Count { get; set; }

        // Navigation property for relationship with the ATM entity.
        public virtual ATM ATM { get; set; }
    }
}
