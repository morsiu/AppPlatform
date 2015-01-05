using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mors.AppPlatform.Support.EventSourcing.Dependencies
{
    public interface IEventStore
    {
        IEnumerable<object> GetReader();

        IEventWriter GetWriter();
    }
}
