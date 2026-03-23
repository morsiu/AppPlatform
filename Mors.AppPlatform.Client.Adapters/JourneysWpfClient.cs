using Mors.AppPlatform.Adapters.Journeys;
using Mors.AppPlatform.Client.Adapters.Journeys;
using Mors.Journeys.Application.Client.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using Mors.AppPlatform.Client.Support;

namespace Mors.AppPlatform.Client.Adapters;

public class JourneysWpfClient : IApplication
{
    private static Bootstrapper _bootstrapper;

    public JourneysWpfClient(
        Service.Client.RequestFactory requestFactory,
        AppPlatform.Support.Events.IEventBus eventBus,
        AppPlatform.Support.Dispatching.HandlerDispatcher handlerDispatcher,
        AppPlatform.Support.Dispatching.IHandlerRegistry handlerRegistry,
        AppPlatform.Support.Repositories.GuidIdFactory idFactory)
    {
        _bootstrapper =
            new Bootstrapper(
                new ClientWpfEventBus(eventBus),
                new ClientWpfCommandDispatcher(requestFactory, handlerDispatcher),
                new ClientWpfCommandHandlerRegistry(handlerRegistry),
                new ClientWpfQueryDispatcher(requestFactory, handlerDispatcher),
                new ClientWpfQueryHandlerRegistry(handlerRegistry),
                new ClientWpfIdFactory(idFactory));
    }

    public static IReadOnlySet<Type> GetSerializableTypes()
    {
        return SerializableTypes.Value;
    }

    public UIElement CreateUiForInteractionWithSelf()
    {
        return _bootstrapper.Bootstrap();
    }

    public string DescribeSelfForSelectionUi()
    {
        return "Journeys";
    }

    public string DescribeSelfForTitleBarOfMainWindow()
    {
        return "Journeys";
    }
}