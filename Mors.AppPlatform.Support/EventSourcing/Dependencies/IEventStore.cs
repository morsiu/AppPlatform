using System.Collections.Generic;

namespace Mors.AppPlatform.Support.EventSourcing.Dependencies;

public interface IEventStore
{
    IEnumerable<object> GetReader();

    IEventWriter GetWriter();
}