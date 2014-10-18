using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters.Modules.Command
{
    internal sealed class CommandTransactedRepositories : IRepositories, ITransactional<IRepositories>
    {
        private readonly ITransactional<IRepositories> _repositories;

        public CommandTransactedRepositories(Mors.AppPlatform.Support.Repositories.IRepositories repositories)
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

        public ITransactional<IRepositories> Lift()
        {
            return this;
        }

        public IRepositories Object
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
