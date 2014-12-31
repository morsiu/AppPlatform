using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Support.Repositories
{
    internal interface IRepository<TEntity> : IProvideTransactional<IRepository<TEntity>>
    {
        TEntity Get(object id);

        void Store(object id, TEntity entity);
    }
}
