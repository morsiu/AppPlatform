﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journeys.Command;
using Journeys.Dispatching;

namespace Journeys.Adapters
{
    public class CommandHandlerRegistry : ICommandHandlerRegistry
    {
        private readonly CommandProcessor _commandProcessor;

        public CommandHandlerRegistry(CommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public void SetHandler<TCommand>(Command.CommandHandler<TCommand> handler)
        {
            _commandProcessor.SetHandler(new Dispatching.CommandHandler<TCommand>(handler));
        }
    }
}
