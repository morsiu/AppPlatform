using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Mors.AppPlatform.Support.Dispatching;

public sealed class AsyncHandlerDispatcher
{
    private readonly IHandlerSource _source;

    public AsyncHandlerDispatcher(
        IHandlerSource source)
    {
        _source = source;
    }

    [DoesNotReturn]
    public void Run()
    {
        while (true)
        {
            var handler = _source.Dequeue();
            Task.Run(handler);
        }
    }
}