namespace OutsideManaged.OutsideAccount
{
    public interface IAccount
    {
        Task<bool> CheckAccountIsValid(string userAccount, DateTime? createDate);
        Task<IEnumerable<OutsideAccountView>> GetAccountListView();
    }
}