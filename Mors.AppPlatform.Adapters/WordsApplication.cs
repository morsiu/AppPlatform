﻿using Mors.AppPlatform.Adapters.Words;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.EventSourcing;
using Mors.AppPlatform.Support.Repositories;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters
{
    public static class WordsApplication
    {
        public static void Bootstrap(
            IHandlerRegistry handlerRegistry,
            IEventBus eventBus,
            Transaction transaction,
            EventSourcingModule eventSourcingModule,
            GuidIdFactory idFactory)
        {
            var bootstrapper = new Mors.Words.Bootstrapper();
            var wordsEventBus = new ApplicationEventBus(eventBus, eventSourcingModule);
            bootstrapper.BootstrapCommands(
                new ApplicationCommandHandlerRegistry(handlerRegistry, transaction, wordsEventBus, idFactory).Register);
            bootstrapper.BootstrapQueries(
                new ApplicationQueryHandlerRegistry(handlerRegistry).Register,
                wordsEventBus.RegisterListener);
        }
    }
}
