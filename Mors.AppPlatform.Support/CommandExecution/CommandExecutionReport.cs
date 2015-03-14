namespace Mors.AppPlatform.Support.CommandExecution
{
    public sealed class CommandExecutionReport
    {
        private readonly object _commandResult;
        private readonly bool _commandComplete;
        private readonly bool _applicationStateCorrupted;

        public static CommandExecutionReport CreateCompletionReport(object commandResult)
        {
            return new CommandExecutionReport(true, commandResult, false);
        }

        public static CommandExecutionReport CreateFailureReport()
        {
            return new CommandExecutionReport(false, null, false);
        }

        public static CommandExecutionReport CreateFailureAndApplicationStateCorruptionReport()
        {
            return new CommandExecutionReport(false, null, true);
        }

        private CommandExecutionReport(
            bool commandComplete,
            object commandResult,
            bool applicationStateCorrupted)
        {
            _commandComplete = commandComplete;
            _commandResult = commandResult;
            _applicationStateCorrupted = applicationStateCorrupted;
        }

        public bool CommandComplete { get { return _commandComplete; } }

        public object CommandResult { get { return _commandResult; } }

        public bool ApplicationStateCorrupted { get { return _applicationStateCorrupted; } }
    }
}
