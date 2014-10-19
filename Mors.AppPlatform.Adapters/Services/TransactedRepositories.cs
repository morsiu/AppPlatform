using Mors.AppPlatform.Common;
using Mors.AppPlatform.Common.Transactions;

namespace Mors.AppPlatform.Adapters.Services
{
    internal sealed class TransactedRepositories : Common.Services.IRepositories, ITransactional<Common.Services.IRepositories>
    {
        private readonly ITransactional<Support.Repositories.IRepositories> _repositories;

        public TransactedRepositories(Support.Repositories.IRepositories repositories)
        {
            _repositories = repositories.Lift();
        }

        public TEntity Get<TEntity>(object id) where TEntity : IHasId
        {
            return _repositories.Object.Get<TEntity>(id);
        }

        public void Store<TEntity>(TEntity entity) where TEntity : IHasId
        {
            _repositories.Object.Store(entity.Id, entity);
        }

        public ITransactional<Common.Services.IRepositories> Lift()
        {
            return this;
        }

        public Common.Services.IRepositories Object
        {
            get { return this; }
        }

        public void Abort()
        {
            _repositories.Abort();
        }

        public void Commit()
        {
            _repositories.Commit();
        }
    }
}
