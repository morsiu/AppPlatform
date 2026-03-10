using Mors.Journeys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Adapters.Journeys
{
    public static class SerializableTypes
    {
        public static IReadOnlySet<Type> Value { get; } =
            typeof(IQuery<>).Assembly.GetTypes()
                .Where(x => x.GetCustomAttribute<DataContractAttribute>() != null)
                .Concat(
                    typeof(IQuery<>).Assembly.GetTypes()
                        .SelectMany(
                            x => x.GetInterfaces()
                                .Where(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IQuery<>))
                                .Select(x => x.GetGenericArguments()[0])))
                .Select(
                    x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                        ? typeof(List<>).MakeGenericType(x.GetGenericArguments()[0])
                        : x)
                .ToHashSet();
    }
}
