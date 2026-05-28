using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Services.EventHandlers;

public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedEvent message)
    {
        _logger.LogInformation(
            "Sale {SaleNumber} was created. SaleId: {SaleId}",
            message.SaleNumber,
            message.SaleId);

        return Task.CompletedTask;
    }
}
