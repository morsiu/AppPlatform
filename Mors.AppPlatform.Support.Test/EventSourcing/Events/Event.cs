using System.Runtime.Serialization;

namespace Mors.AppPlatform.Support.Test.EventSourcing.Events
{
    [DataContract]
    internal sealed class Event
    {
        [DataMember]
        public string Field { get; private set; }

        public Event(string field)
        {
            Field = field;
        }
    }
}
