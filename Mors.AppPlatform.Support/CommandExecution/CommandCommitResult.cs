namespace Mors.AppPlatform.Support.CommandExecution
{
    public enum CommandCommitResult
    {
        CommitComplete,
        EventsStoreFailed,
        AggregatesStoreFailed,
        CommitCompleteButSomeEventsPublishFailed
    }
}
