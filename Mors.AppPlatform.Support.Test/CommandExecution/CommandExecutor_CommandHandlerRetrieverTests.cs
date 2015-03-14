using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mors.AppPlatform.Support.CommandExecution;

namespace Mors.AppPlatform.Support.Test.CommandExecution
{
    [TestClass]
    public class CommandExecutor_CommandHandlerRetrieverTests
    {
        [TestMethod]
        public void ShouldExecuteCommandHandlerReturnedFromRetriever()
        {
            var handlerWasExecuted = false;
            var executor = new CommandExecutor(_ => (commandDefinition, commandHandlerEnvironment) => { handlerWasExecuted = true; return null; }, CreateEnvironment, logEntry => { });

            executor.Execute(new object());

            Assert.IsTrue(handlerWasExecuted);
        }

        [TestMethod]
        public void ShouldRetrieveCommandHandlerByCallingRetrieverWithCommandAsArgument()
        {
            var retrieverWasExecuted = false;
            var passedCommandDefinition = default(object);
            var executor = new CommandExecutor(
                aCommandDefinition =>
                {
                    retrieverWasExecuted = true;
                    passedCommandDefinition = aCommandDefinition;
                    return (_, commandHandlerEnvironment) => null;
                },
                CreateEnvironment,
                logEntry => { });

            var commandDefinition = new object();
            executor.Execute(commandDefinition);

            Assert.IsTrue(retrieverWasExecuted);
            Assert.AreEqual(commandDefinition, passedCommandDefinition);
        }

        private CommandHandlerEnvironment CreateEnvironment()
        {
            return CommandHandlerEnvironmentFactory.Create();
        }
    }
}
