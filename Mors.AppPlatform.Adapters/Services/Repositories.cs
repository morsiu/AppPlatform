using Mors.AppPlatform.Common;
using Mors.AppPlatform.Common.Transactions;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class Repositories : Common.Services.IRepositories
    {
        private readonly IRepositories _repositories;

        public Repositories(IRepositories repositories)
        {
            _repositories = repositories;
        }

        public TEntity Get<TEntity>(object id) where TEntity : IHasId
        {
            return _repositories.Get<TEntity>(id);
        }

        public void Store<TEntity>(TEntity entity) where TEntity : IHasId
        {
            _repositories.Store(entity.Id, entity);
        }

        public ITransactional<Common.Services.IRepositories> Lift()
        {
            return new TransactedRepositories(_repositories);
        }
    }
}
