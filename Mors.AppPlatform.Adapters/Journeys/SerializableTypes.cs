using Mors.Journeys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Adapters.Journeys;

public static class SerializableTypes
{
    public static IReadOnlySet<Type> Value { get; } =
        typeof(IQuery<>).Assembly.GetTypes()
            .Where(x => x.GetCustomAttribute<DataContractAttribute>() != null)
            .Concat(
                typeof(IQuery<>).Assembly.GetTypes()
                    .Where(x => x.GetCustomAttribute<DataContractAttribute>() != null)
                    .SelectMany(
                        x => x.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Select(y => y.PropertyType)
                            .Where(y => y.IsGenericType
                                        && y.GetGenericTypeDefinition() is { } z
                                        && (z == typeof(IEnumerable<>) || z == typeof(IReadOnlyList<>)))))
            .Concat(
                typeof(IQuery<>).Assembly.GetTypes()
                    .SelectMany(
                        x => x.GetInterfaces()
                            .Where(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IQuery<>))
                            .Select(y => y.GetGenericArguments()[0])))
            .Select(
                x => x.IsGenericType
                     && x.GetGenericTypeDefinition() is { } z
                     && (z == typeof(IEnumerable<>) || z == typeof(IReadOnlyList<>))
                    ? typeof(List<>).MakeGenericType(x.GetGenericArguments()[0])
                    : x)
            .ToHashSet();
}