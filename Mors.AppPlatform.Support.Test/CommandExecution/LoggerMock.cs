using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mors.AppPlatform.Support.Test.CommandExecution
{
    internal sealed class LoggerMock
    {
        private readonly List<object> _logEntries = new List<object>();

        public void LogEntry(object logEntry)
        {
            _logEntries.Add(logEntry);
        }

        public void AssertHasOneLogEntry()
        {
            Assert.AreEqual(1, _logEntries.Count);
        }

        public void AssertHasOneLogEntry(Action<dynamic> logEntryAssert)
        {
            AssertHasOneLogEntry();
            logEntryAssert(_logEntries[0]);
        }
    }
}
