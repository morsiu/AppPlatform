using System.IO;
using Mors.AppPlatform.Support.EventSourcing.Storage;
using Mors.AppPlatform.Support.Test.EventSourcing.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mors.AppPlatform.Support.Test.EventSourcing
{
    [TestClass]
    public sealed class XmlEventReaderWriterTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void ShouldWriteThenReadOneEvent()
        {
            var eventTypesToSupport = new[] { typeof(Event) };
            var stream = new MemoryStream();
            var writer = new XmlEventWriter(stream, eventTypesToSupport);
            var @event = new Event("Value");
            writer.Write(@event);

            stream.Seek(0, SeekOrigin.Begin);
            var reader = new XmlEventReader(stream, eventTypesToSupport);
            var readEvent = (Event)reader.Read();
            Assert.AreEqual(@event.Field, readEvent.Field);
        }
    }
}
