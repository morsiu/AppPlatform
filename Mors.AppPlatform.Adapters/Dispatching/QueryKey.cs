using System;
using Mors.Journeys.Data;

namespace Mors.AppPlatform.Adapters.Dispatching
{
    internal sealed class QueryKey
    {
        private readonly Type _queryType;

        public QueryKey(Type queryType)
        {
            _queryType = queryType;
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(obj, null)
                && obj is QueryKey
                && Equals((QueryKey)obj);
        }

        public override int GetHashCode()
        {
            return _queryType.GetHashCode();
        }

        public override string ToString()
        {
            return _queryType.ToString();
        }

        private bool Equals(QueryKey other)
        {
            return other._queryType.Equals(_queryType);
        }
    }
}
