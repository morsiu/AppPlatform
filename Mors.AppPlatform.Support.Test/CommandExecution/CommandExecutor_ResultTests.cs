using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mors.AppPlatform.Support.CommandExecution;

namespace Mors.AppPlatform.Support.Test.CommandExecution
{
    [TestClass]
    public class CommandExecutor_ResultTests
    {
        [TestMethod]
        public void ShouldReportCommandFailureWhenCommandHandlerIsMissing()
        {
            var executor = new CommandExecutor(commandDefinition => null, CreateEnvironment, logEntry => { });

            var result = executor.Execute(new object());

            Assert.AreEqual(false, result.CommandComplete);
            Assert.IsNull(result.CommandResult);
        }

        [TestMethod]
        public void ShouldReportCommandFailureWhenCommandHandlerThrowsException()
        {
            var executor = new CommandExecutor(_ => (commandDefinition, commandHandlerEnvironment) => { throw new Exception(); }, CreateEnvironment, logEntry => { });

            var result = executor.Execute(new object());

            Assert.AreEqual(false, result.CommandComplete);
            Assert.IsNull(result.CommandResult);
        }

        [TestMethod]
        public void ShouldReportCommandFailureWhenCommandHandlerEnvironmentRetrieverThrowsException()
        {
            var executor = new CommandExecutor(_ => (commandDefinition, commandHandlerEnvironment) => null, () => { throw new Exception(); }, logEntry => { });

            var result = executor.Execute(new object());

            Assert.AreEqual(false, result.CommandComplete);
            Assert.IsNull(result.CommandResult);
        }

        [TestMethod]
        public void ShouldReportCommandSuccessWhenCommandHandlerRunsToCompletion()
        {
            var commandResult = new object();
            var executor = new CommandExecutor(_ => (commandDefinition, commandHandlerEnvironment) => commandResult, CreateEnvironment, logEntry => { });

            var result = executor.Execute(new object());

            Assert.AreEqual(true, result.CommandComplete);
            Assert.AreSame(commandResult, result.CommandResult);
        }

        [TestMethod]
        public void ShouldReportApplicationStateCorruptionWhenAggregateStorerThrowsException()
        {
            var executor = new CommandExecutor(
                _ => (commandDefinition, commandHandlerEnvironment) => { commandHandlerEnvironment.StoreAggregate(typeof(object), 1, new object()); return null; },
                () => CommandHandlerEnvironmentFactory.Create(aggregateStorer: (aggregateType, aggregateId, aggregate) => { throw new Exception(); }),
                logEntry => { });

            var result = executor.Execute(new object());

            Assert.AreEqual(true, result.ApplicationStateCorrupted);
        }

        [TestMethod]
        public void ShouldReportApplicationStateCorruptionWhenEventDispatcherThrowsException()
        {
            var executor = new CommandExecutor(
                _ => (commandDefinition, commandHandlerEnvironment) => { commandHandlerEnvironment.QueueEvent(new object()); return null; },
                () => CommandHandlerEnvironmentFactory.Create(eventDispatcher: @event => { throw new Exception(); }),
                logEntry => { });

            var result = executor.Execute(new object());

            Assert.AreEqual(true, result.ApplicationStateCorrupted);
        }

        private CommandHandlerEnvironment CreateEnvironment()
        {
            return CommandHandlerEnvironmentFactory.Create();
        }
    }
}
