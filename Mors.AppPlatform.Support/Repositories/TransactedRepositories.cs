﻿using System;
using System.Collections.Generic;
using Mors.AppPlatform.Common.Transactions;

namespace Mors.AppPlatform.Support.Repositories
{
    using EntityType = Type;

    internal sealed class TransactedRepositories : IRepositories, ITransactional<IRepositories>
    {
        private readonly Dictionary<EntityType, ITransactional> _transactedRepositories = new Dictionary<EntityType, ITransactional>();
        private readonly Repositories _repositories;

        public TransactedRepositories(Repositories repositories)
        {
            _repositories = repositories;
        }

        public TEntity Get<TEntity>(object id)
        {
            var repository = GetRepository<TEntity>();
            return repository.Get(id);
        }

        public void Store<TEntity>(object id, TEntity entity)
        {
            var repository = GetRepository<TEntity>();
            repository.Store(id, entity);
        }

        private IRepository<TEntity> GetRepository<TEntity>()
        {
            var entityType = typeof(TEntity);
            if (_transactedRepositories.ContainsKey(entityType))
            {
                var repository = (IRepository<TEntity>)_transactedRepositories[entityType];
                return repository;
            }
            var newRepository = _repositories.GetRepository<TEntity>();
            var transactedRepository = new TransactedRepository<TEntity>(newRepository);
            _transactedRepositories[entityType] = transactedRepository;
            return newRepository;
        }

        public IRepositories Object
        {
            get { return this; }
        }

        public void Abort()
        {
            foreach (var repository in _transactedRepositories.Values)
            {
                repository.Abort();
            }
        }

        public void Commit()
        {
            foreach (var repository in _transactedRepositories.Values)
            {
                repository.Commit();
            }
        }

        public ITransactional<IRepositories> Lift()
        {
            return this;
        }
    }
}
