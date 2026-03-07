using System;
using System.Collections.Generic;

namespace Mors.AppPlatform.Support.Serialization
{
    public sealed class KnownTypesSet
    {
        private readonly HashSet<Type> _knownTypes = new();

        public void AddType(Type type)
        {
            _knownTypes.Add(type);
        }

        public IReadOnlySet<Type> GetKnownTypes()
        {
            return _knownTypes;
        }
    }
}
