using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;

namespace Mors.AppPlatform.Support.Logging
{
    public sealed class JsonFileLog
    {
        private readonly StreamWriter _writer;
        private readonly JsonSerializer _serializer;

        public JsonFileLog(string logFilePath)
        {
            var stream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(stream, Encoding.UTF8);
            _serializer = JsonSerializer.Create();
        }

        public void Log(object entry)
        {
            WriteEntryToLog(AddTimeStamp(entry));
        }

        private void WriteEntryToLog(object entry)
        {
            using (var scope = new TransactionScope())
            {
                _serializer.Serialize(_writer, entry);
                scope.Complete();
            }
        }

        private object AddTimeStamp(object entry)
        {
            return new { Timestamp = DateTime.Now, Entry = entry };
        }
    }
}
