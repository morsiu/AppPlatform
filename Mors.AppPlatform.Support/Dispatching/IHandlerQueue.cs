using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mors.AppPlatform.Support.Dispatching
{
    public interface IHandlerQueue
    {
        WaitHandle NonEmptyEvent { get; }

        void Enqueue(Action handler);

        bool TryDequeue(out Action handler);
    }
}
