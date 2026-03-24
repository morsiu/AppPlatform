using System;

namespace Mors.AppPlatform.Service.Adapters.Dispatching;

internal sealed class QueryKey : IEquatable<QueryKey>
{
    private readonly Type _queryType;

    public QueryKey(Type queryType)
    {
        _queryType = queryType;
    }

    public bool Equals(QueryKey other)
    {
        return other != null && other._queryType == _queryType;
    }

    public override bool Equals(object obj)
    {
        return obj is QueryKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _queryType.GetHashCode();
    }

    public override string ToString()
    {
        return _queryType.ToString();
    }
}