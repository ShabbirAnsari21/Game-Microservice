using System.ComponentModel.DataAnnotations;

namespace ApplicationSystem.DbModels.ATMSystem
{
    public class ATM
    {
        // Primary table to represent the ATM entity.
        [Key]
        public int AtmId { get; set; } // Primary Key

        // Collection of denominations available in this ATM.
        public virtual ICollection<Denomination> Denominations { get; set; } = new List<Denomination>();

        // Collection of transactions that occurred at this ATM.
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        // Maximum transaction limit for withdrawal
        public const int MaxTransactionLimit = 15000;
    }
}
