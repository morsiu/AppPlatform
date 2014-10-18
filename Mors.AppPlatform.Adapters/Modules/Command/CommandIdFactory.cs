using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Modules.Command
{
    public sealed class CommandIdFactory : IIdFactory
    {
        private readonly GuidIdFactory _idFactory;

        public CommandIdFactory(GuidIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public object Create()
        {
            return _idFactory.Create();
        }
    }
}
