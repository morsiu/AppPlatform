using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ApplicationQueryDispatcher : Mors.Journeys.Application.IQueryDispatcher
    {
        private readonly HandlerDispatcher _handlerDispatcher;

        public ApplicationQueryDispatcher(HandlerDispatcher handlerDispatcher)
        {
            _handlerDispatcher = handlerDispatcher;
        }

        public TResult Dispatch<TResult>(Mors.Journeys.Data.IQuery<TResult> querySpecification)
        {
            var query = new Query<TResult>(querySpecification);
            return query.Dispatch(_handlerDispatcher);
        }
    }
}
