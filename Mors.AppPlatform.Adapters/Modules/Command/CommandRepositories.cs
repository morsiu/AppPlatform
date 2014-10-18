using Mors.AppPlatform.Common.Transactions;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Modules.Command
{
    public sealed class CommandRepositories : IRepositories
    {
        private readonly Mors.AppPlatform.Support.Repositories.IRepositories _repositories;

        public CommandRepositories(Mors.AppPlatform.Support.Repositories.IRepositories repositories)
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

        public ITransactional<IRepositories> Lift()
        {
            return new CommandTransactedRepositories(_repositories);
        }
    }
}
