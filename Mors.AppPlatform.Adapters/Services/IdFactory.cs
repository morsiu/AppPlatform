using Mors.AppPlatform.Common.Services;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class IdFactory : IIdFactory
    {
        private readonly GuidIdFactory _idFactory;

        public IdFactory(GuidIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public object Create()
        {
            return _idFactory.Create();
        }
    }
}
