using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ClientWpfIdFactory : Mors.Journeys.Application.Client.Wpf.IIdFactory
    {
        private readonly GuidIdFactory _idFactory;

        public ClientWpfIdFactory(GuidIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public object Create()
        {
            return _idFactory.Create();
        }
    }
}
