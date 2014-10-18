using System;

namespace Mors.AppPlatform.Support.Repositories
{
    public sealed class GuidIdFactory
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }

        public Type IdImplementationType
        {
            get { return typeof(Guid); }
        }
    }
}
