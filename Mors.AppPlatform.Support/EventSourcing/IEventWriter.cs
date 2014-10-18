namespace Mors.AppPlatform.Support.EventSourcing
{
    internal interface IEventWriter
    {
        void Write<TEvent>(TEvent @event);
    }
}
