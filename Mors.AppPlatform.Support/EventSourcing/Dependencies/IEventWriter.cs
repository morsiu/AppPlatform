namespace Mors.AppPlatform.Support.EventSourcing.Dependencies
{
    public interface IEventWriter
    {
        void Write<TEvent>(TEvent @event);
    }
}
