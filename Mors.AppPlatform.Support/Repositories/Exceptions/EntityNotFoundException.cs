using System;

namespace Mors.AppPlatform.Support.Repositories.Exceptions
{
    [Serializable]
    public sealed class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}
