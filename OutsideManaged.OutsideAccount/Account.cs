using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace OutsideManaged.OutsideAccount
{
    public class Account : IAccount
    {
        private readonly IServiceProvider _serviceProvider;

        public Account(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<OutsideAccountView>> GetAccountListView()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ModelContext modelContext = scope.ServiceProvider.GetRequiredService<ModelContext>();

                var result = from a in modelContext.Accountvos
                             select new OutsideAccountView()
                             {
                                 UserAccount = a.Userid,
                                 Title = "軟體架構師",
                                 ContactName = "Gelis"
                             };

                return result;
            }

            //return await Task.FromResult<IEnumerable<OutsideAccountView>>(
            //    new List<OutsideAccountView>(new OutsideAccountView[1].AsEnumerable())); // 回應一個空值
        }

        public async Task<bool> CheckAccountIsValid(string userAccount, DateTime? createDate)
        {
            return await Task.FromResult<bool>(true);
        }
    }
}