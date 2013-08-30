﻿using System;
using Journeys.Adapters.Messages;
using Journeys.Dispatching;
using Journeys.Dispatching.Exceptions;
using Journeys.Queries;

namespace Journeys.Adapters
{
    public class Query<TResult>
    {
        private IQuery<TResult> _query;

        public Query(IQuery<TResult> query)
        {
            _query = query;
        }

        public TResult Execute(HandlerDispatcher dispatcher)
        {
            var queryKey = QueryKey.From(_query);
            try
            {
                return (TResult)dispatcher.Dispatch(queryKey, _query);
            }
            catch (HandlerNotFoundException)
            {
                throw new InvalidOperationException(string.Format(FailureMessages.NoHandlerRegisteredForQueryOfType, queryKey));
            }
        }
    }
}