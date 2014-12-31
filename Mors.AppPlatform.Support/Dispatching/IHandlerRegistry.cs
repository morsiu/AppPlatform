using System;

namespace Mors.AppPlatform.Support.Dispatching
{
    public interface IHandlerRegistry
    {
        bool Retrieve(object key, out Func<object, object> handler);

        void Set(object key, Func<object, object> handler);
    }
}
