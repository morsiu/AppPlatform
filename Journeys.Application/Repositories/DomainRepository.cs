﻿using Journeys.Domain.Infrastructure;
using Journeys.Domain.Journeys.Data;
using Journeys.Transactions;
using System;

namespace Journeys.Application.Repositories
{
    internal class DomainRepository<TEntity> : IDomainRepository<TEntity>, IProvideTransacted<IDomainRepository<TEntity>>
        where TEntity : IHasId
    {
        private readonly InMemoryRepository<Id, TEntity> _repository = new InMemoryRepository<Id, TEntity>();

        public TEntity Get(Id id)
        {
            return _repository.Get(id);
        }

        public void Store(TEntity entity)
        {
            _repository.Store(entity.Id, entity);
        }

        public ITransacted<IDomainRepository<TEntity>> Lift()
        {
            return new TransactedDomainRepository<TEntity>(this);
        }
    }
}
