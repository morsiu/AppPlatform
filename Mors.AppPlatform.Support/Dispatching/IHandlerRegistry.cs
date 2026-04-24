using System;
using System.Diagnostics.CodeAnalysis;

namespace Mors.AppPlatform.Support.Dispatching;

public interface IHandlerRegistry
{
    bool Retrieve(object key, [NotNullWhen(true)] out Func<object, object>? handler);

    void Set(object key, Func<object, object> handler);
}