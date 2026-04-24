using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Mors.AppPlatform.Service.Infrastructure;

internal sealed class ContentTypeAwareSerializer
{
    private readonly DataContractSerializer _xmlSerializer;

    public ContentTypeAwareSerializer(IEnumerable<Type> knownTypes)
    {
        _xmlSerializer = new DataContractSerializer(typeof(object), knownTypes);
    }

    public object? Deserialize(Stream contentStream, string? contentType)
    {
        switch (MatchContentType(contentType))
        {
            case ContentType.Json:
                return DeserializeJson(contentStream);
            case ContentType.Xml:
                return _xmlSerializer.ReadObject(contentStream);
            default:
                throw new ArgumentException("Unsupported content type: {0}", contentType);
        }
    }

    public string Serialize(object? content, Stream contentStream, IEnumerable<Tuple<string?, decimal>> contentTypes)
    {
        var selectedContentType = SelectContentType(contentTypes);
        switch (selectedContentType?.Type)
        {
            case ContentType.Json:
                SerializeJson(content, contentStream);
                break;
            case ContentType.Xml:
                _xmlSerializer.WriteObject(contentStream, content);
                break;
            default:
                throw new ArgumentException(
                    $"Unsupported content types: {string.Join(", ", contentTypes.Select(ct => ct.Item1))}", nameof(contentTypes));
        }
        return selectedContentType.Value.Value;
    }

    private static void SerializeJson(object? content, Stream contentStream)
    {
        var serializedContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        var writer = new StreamWriter(contentStream);
        writer.Write(serializedContent);
        writer.Flush();
    }

    private static object? DeserializeJson(Stream contentStream)
    {
        var reader = new StreamReader(contentStream);
        var content = reader.ReadToEnd();
        return JsonConvert.DeserializeObject(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
    }

    private static (ContentType Type, string Value)? SelectContentType(IEnumerable<Tuple<string?, decimal>> contentTypes)
    {
        return contentTypes
            .Select(
                ct =>
                    MatchContentType(ct.Item1) is { } contentType && ct.Item1 is { } contentTypeName
                        ? ((ContentType, string)?)(contentType, contentTypeName)
                        : null)
            .FirstOrDefault(ct => ct != null);
    }

    private static ContentType? MatchContentType(string? contentType)
    {
        if (contentType == null)
        {
            return null;
        }
        if (contentType.Contains("xml"))
        {
            return ContentType.Xml;
        }
        if (contentType.Contains("json"))
        {
            return ContentType.Json;
        }
        return null;
    }

    private enum ContentType
    {
        Xml,
        Json,
    }
}