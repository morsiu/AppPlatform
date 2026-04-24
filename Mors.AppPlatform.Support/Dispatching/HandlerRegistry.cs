using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Mors.AppPlatform.Support.Dispatching;

public sealed class HandlerRegistry : IHandlerRegistry
{
    private readonly Dictionary<object, Func<object, object>> _handlers;

    public HandlerRegistry()
    {
        _handlers = new Dictionary<object, Func<object, object>>();
    }

    public void Set(object key, Func<object, object> handler)
    {
        _handlers[key] = handler;
    }

    public bool Retrieve(object key, [NotNullWhen(true)] out Func<object, object>? handler)
    {
        return _handlers.TryGetValue(key, out handler);
    }
}