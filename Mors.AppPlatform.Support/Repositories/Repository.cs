using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Support.Repositories
{
    internal sealed class Repository<TEntity> : IRepository<TEntity>
    {
        private readonly InMemoryRepository<object, TEntity> _repository = new InMemoryRepository<object, TEntity>();

        public TEntity Get(object id)
        {
            return _repository.Get(id);
        }

        public void Store(object id, TEntity entity)
        {
            _repository.Store(id, entity);
        }

        public ITransactional<IRepository<TEntity>> Lift()
        {
            return new TransactedRepository<TEntity>(this);
        }
    }
}
