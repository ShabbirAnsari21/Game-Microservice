using ApplicationSystem.DbModels.ATMSystem;

namespace ApplicationSystem.Repository.ATMSystemRepository
{
    public interface IATMRepository
    {
        void AddCash(Dictionary<int, int> denominations);
        Dictionary<int, int> WithdrawCash(int amount);
        Dictionary<int, int> GetNodesSummary();
        List<Transaction> GetTransactions();
    }
}
