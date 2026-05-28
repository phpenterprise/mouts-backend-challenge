using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Application.Services.EventHandlers;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        var salesEventNetwork = new InMemNetwork();

        builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        builder.Services.AddScoped<ISaleEventPublisher, RebusSaleEventPublisher>();

        builder.Services.AddRebus(
            configure => configure.Transport(transport =>
                transport.UseInMemoryTransport(salesEventNetwork, "sales-events")),
            onCreated: async bus =>
            {
                await bus.Subscribe<SaleCreatedEvent>();
                await bus.Subscribe<SaleModifiedEvent>();
                await bus.Subscribe<SaleCancelledEvent>();
                await bus.Subscribe<ItemCancelledEvent>();
            });

        builder.Services.AddRebusHandler<SaleCreatedEventHandler>();
        builder.Services.AddRebusHandler<SaleModifiedEventHandler>();
        builder.Services.AddRebusHandler<SaleCancelledEventHandler>();
        builder.Services.AddRebusHandler<ItemCancelledEventHandler>();
    }
}
