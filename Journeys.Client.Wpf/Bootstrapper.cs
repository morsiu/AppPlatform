﻿using Journeys.Client.Wpf.Adapters;
using Journeys.Repositories;

namespace Journeys.Client.Wpf
{
    internal class Bootstrapper
    {
        public MainWindow Bootstrap()
        {
            var eventBus = new Event.EventBus();
            var idFactory = new IdFactory();

            var queryBootstrapper = new Query.Bootstrapper(eventBus);
            queryBootstrapper.Bootstrap();

            var repositories = new Repositories.Repositories();
            var commandBootstrapper = new Command.Bootstrapper(eventBus, new CommandRepositories(repositories), new CommandIdFactory(idFactory));
            commandBootstrapper.Bootstrap(queryBootstrapper.QueryDispatcher);
            
            var eventSourcingBootstrapper = new EventSourcing.Bootstrapper(eventBus, new EventSourcingRepositories(repositories), idFactory.IdImplementationType, "Events.txt");
            eventSourcingBootstrapper.Bootstrap();
            
            var viewEventBus = new Infrastructure.EventBus();
            return new MainWindow(commandBootstrapper.CommandDispatcher, queryBootstrapper.QueryDispatcher, viewEventBus, idFactory);
        }
    }
}
