using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mors.AppPlatform.Support.CommandExecution;
using Command = System.Object;
using CommandResult = System.Object;

namespace Mors.AppPlatform.Support.Test.CommandExecution
{
    using CommandHandler = Func<Command, ICommandHandlerEnvironment, CommandResult>;

    [TestClass]
    public class CommandExecutor_LogBehaviorTests
    {
        [TestMethod]
        public void ShouldLogMissingCommandHandler()
        {
            var loggerMock = new LoggerMock();
            var executor = new CommandExecutor(commandDefinition => ResultToHandler(null), CreateEnvironment, loggerMock.LogEntry);

            executor.Execute(new object());

            loggerMock.AssertHasOneLogEntry();
        }

        [TestMethod]
        public void ShouldLogExceptionThrownFromCommandHandler()
        {
            var exception = new Exception();
            var loggerMock = new LoggerMock();
            var executor = new CommandExecutor(commandDefinition => ExceptionToHandler(exception), CreateEnvironment, loggerMock.LogEntry);

            executor.Execute(new object());

            loggerMock.AssertHasOneLogEntry(logEntry => Assert.AreEqual(exception, logEntry.Exception));
        }

        [TestMethod]
        public void ShouldLogCommandHandlerCompletion()
        {
            var loggerMock = new LoggerMock();
            var executor = new CommandExecutor(commandDefinition => ResultToHandler(null), CreateEnvironment, loggerMock.LogEntry);

            executor.Execute(new object());

            loggerMock.AssertHasOneLogEntry();
        }

        [TestMethod]
        public void ShouldLogExceptionThrownByCommandHandlerEnvironmentRetriever()
        {
            var loggerMock = new LoggerMock();
            var exception = new Exception();
            var executor = new CommandExecutor(commandDefinition => ResultToHandler(null), () => { throw exception; }, loggerMock.LogEntry);

            executor.Execute(new object());

            loggerMock.AssertHasOneLogEntry(logEntry => Assert.AreEqual(exception, logEntry.Exception));
        }

        private CommandHandler ResultToHandler(object result)
        {
            return (commandDefinition, commandHandlerEnvironment) => result;
        }

        private CommandHandler ExceptionToHandler(Exception exception)
        {
            return (commandDefinition, commandHandlerEnvironment) => { throw exception; };
        }

        private CommandHandlerEnvironment CreateEnvironment()
        {
            return CommandHandlerEnvironmentFactory.Create();
        }
    }
}
