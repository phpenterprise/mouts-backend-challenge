using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Services.EventHandlers;

public class SaleModifiedEventHandler : IHandleMessages<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedEvent message)
    {
        _logger.LogInformation(
            "Sale {SaleNumber} was modified. SaleId: {SaleId}",
            message.SaleNumber,
            message.SaleId);

        return Task.CompletedTask;
    }
}
