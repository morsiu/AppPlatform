using Mors.AppPlatform.Support.Repositories;
using Mors.Journeys.Data;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ApplicationRepositories : Mors.Journeys.Application.IRepositories
    {
        private readonly IRepositories _repositories;

        public ApplicationRepositories(IRepositories repositories)
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
    }
}
