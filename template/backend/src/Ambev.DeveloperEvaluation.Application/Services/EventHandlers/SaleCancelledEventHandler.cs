using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Services.EventHandlers;

public class SaleCancelledEventHandler : IHandleMessages<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCancelledEvent message)
    {
        _logger.LogInformation(
            "Sale {SaleNumber} was cancelled. SaleId: {SaleId}",
            message.SaleNumber,
            message.SaleId);

        return Task.CompletedTask;
    }
}
