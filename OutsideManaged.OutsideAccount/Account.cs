namespace OutsideManaged.OutsideAccount
{
    public class Account
    {
        private readonly IServiceProvider _serviceProvider;

        public Account(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<OutsideAccountView>> GetAccountListView()
        {
            return await Task.FromResult<IEnumerable<OutsideAccountView>>(
                new List<OutsideAccountView>(new OutsideAccountView[1].AsEnumerable())); // 回應一個空值
        }

        public async Task<bool> CheckAccountIsValid(string userAccount, DateTime? createDate)
        {
            return await Task.FromResult<bool>(true);
        }
    }
}