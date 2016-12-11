using System;
using Command = System.Object;
using CommandResult = System.Object;
using LogEntry = System.Object;

namespace Mors.AppPlatform.Support.CommandExecution
{
    using CommandHandler = Func<Command, ICommandHandlerEnvironment, CommandResult>;

    public sealed class CommandExecutor
    {
        private readonly Func<Command, CommandHandler> _commandHandlerRetriever;
        private readonly Logger _logger;
        private readonly Func<CommandHandlerEnvironment> _commandHandlerEnvironmentRetriever;

        public CommandExecutor(
            Func<Command, CommandHandler> commandHandlerRetriever,
            Func<CommandHandlerEnvironment> commandHandlerEnvironmentRetriever,
            Action<LogEntry> logger)
        {
            _commandHandlerRetriever = commandHandlerRetriever;
            _commandHandlerEnvironmentRetriever = commandHandlerEnvironmentRetriever;
            _logger = new Logger(logger);
        }

        public CommandExecutionReport Execute(Command command)
        {
            CommandHandler commandHandler;
            if (!RetrieveCommandHandler(command, out commandHandler))
            {
                return CommandExecutionReport.CreateFailureReport();
            }

            CommandHandlerEnvironment commandHandlerEnvironment;
            if (!RetrieveCommandHandlerEnvironment(out commandHandlerEnvironment))
            {
                return CommandExecutionReport.CreateFailureReport();
            }

            CommandResult commandResult;
            if (!ExecuteCommandHandler(commandHandler, commandHandlerEnvironment, command, out commandResult))
            {
                return CommandExecutionReport.CreateFailureReport();
            }

            bool applicationStateCorrupted;
            if (!CommitCommandHandlerEnvironment(commandHandlerEnvironment, out applicationStateCorrupted))
            {
                return applicationStateCorrupted
                    ? CommandExecutionReport.CreateFailureAndApplicationStateCorruptionReport()
                    : CommandExecutionReport.CreateFailureReport();
            }

            return CommandExecutionReport.CreateCompletionReport(commandResult);
        }

        private bool CommitCommandHandlerEnvironment(CommandHandlerEnvironment commandHandlerEnvironment, out bool applicationStateCorrupted)
        {
            try
            {
                var commitResult = commandHandlerEnvironment.Commit();
                applicationStateCorrupted = commitResult == CommandCommitResult.AggregatesStoreFailed || commitResult == CommandCommitResult.CommitCompleteButSomeEventsPublishFailed;
                return commitResult == CommandCommitResult.CommitComplete;
            }
            catch (Exception exception)
            {
                _logger.LogExceptionFromCommandHandlerEnvironmentCommit(exception);
                applicationStateCorrupted = false;
                return false;
            }
        }

        private bool RetrieveCommandHandlerEnvironment(out CommandHandlerEnvironment commandHandlerEnvironment)
        {
            try
            {
                commandHandlerEnvironment = _commandHandlerEnvironmentRetriever();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogExceptionFromCommandHandlerEnvironmentRetriever(exception);
                commandHandlerEnvironment = null;
                return false;
            }
        }

        private bool ExecuteCommandHandler(
            CommandHandler commandHandler,
            CommandHandlerEnvironment commandHandlerEnvironment,
            CommandResult command,
            out CommandResult commandResult)
        {
            try
            {
                commandResult = commandHandler(command, commandHandlerEnvironment);
                _logger.LogCommandHandlerExecution(command, commandResult);
                return true;
            }
            catch (Exception exception)
            {
                commandResult = default(CommandResult);
                _logger.LogExceptionFromCommandHandler(exception, command);
                return false;
            }
        }

        private bool RetrieveCommandHandler(Command command, out CommandHandler commandHandler)
        {
            commandHandler = _commandHandlerRetriever(command);
            if (commandHandler == null)
            {
                _logger.LogMissingCommandHandler(command);
                return false;
            }
            return true;
        }

        private sealed class Logger
        {
            private readonly Action<LogEntry> _logger;

            public Logger(Action<LogEntry> logger)
            {
                _logger = logger;
            }

            public void LogExceptionFromCommandHandler(Exception exception, CommandResult command)
            {
                _logger(new { Origin = "CommandExecutor", Event = "Exception thrown from command handler", Command = command, Exception = exception });
            }

            public void LogCommandHandlerExecution(CommandResult command, CommandResult commandResult)
            {
                _logger(new { Origin = "CommandExecutor", Event = "Command handler ran to completion", Command = command, Result = commandResult });
            }

            public void LogMissingCommandHandler(Command command)
            {
                _logger(new { Origin = "CommandExecutor", Event = "Missing a command handler", Command = command });
            }

            internal void LogExceptionFromCommandHandlerEnvironmentRetriever(Exception exception)
            {
                _logger(new { Origin = "CommandExecutor", Event = "Exception thrown from command handler environment retrieve", Exception = exception });
            }

            internal void LogExceptionFromCommandHandlerEnvironmentCommit(Exception exception)
            {
                _logger(new { Origin = "CommandExecutor", Event = "Exception thrown from command handler environment commit", Exception = exception });
            }
        }
    }
}
