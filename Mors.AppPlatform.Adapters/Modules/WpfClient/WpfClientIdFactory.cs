﻿using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Modules.WpfClient
{
    public sealed class WpfClientIdFactory : Application.Client.Wpf.IIdFactory
    {
        private readonly GuidIdFactory _idFactory;

        public WpfClientIdFactory(GuidIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public object Create()
        {
            return _idFactory.Create();
        }
    }
}
