using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mors.AppPlatform.Support.Dispatching
{
    public interface IHandlerRegistry
    {
        bool Retrieve(object key, out Func<object, object> handler);

        void Set(object key, Func<object, object> handler);
    }
}
