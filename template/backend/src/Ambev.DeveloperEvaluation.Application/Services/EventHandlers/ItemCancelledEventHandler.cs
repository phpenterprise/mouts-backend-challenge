using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Services.EventHandlers;

public class ItemCancelledEventHandler : IHandleMessages<ItemCancelledEvent>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;

    public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ItemCancelledEvent message)
    {
        _logger.LogInformation(
            "Sale item {ItemId} was cancelled. SaleId: {SaleId}",
            message.ItemId,
            message.SaleId);

        return Task.CompletedTask;
    }
}
