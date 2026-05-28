using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class RebusSaleEventPublisher : ISaleEventPublisher
{
    private readonly IBus? _bus;
    private readonly ILogger<RebusSaleEventPublisher> _logger;

    public RebusSaleEventPublisher(ILogger<RebusSaleEventPublisher> logger, IBus? bus = null)
    {
        _logger = logger;
        _bus = bus;
    }

    public Task PublishSaleCreatedAsync(Guid saleId, string saleNumber, CancellationToken cancellationToken = default)
    {
        return PublishAsync(new SaleCreatedEvent(saleId, saleNumber, DateTime.UtcNow), cancellationToken);
    }

    public Task PublishSaleModifiedAsync(Guid saleId, string saleNumber, CancellationToken cancellationToken = default)
    {
        return PublishAsync(new SaleModifiedEvent(saleId, saleNumber, DateTime.UtcNow), cancellationToken);
    }

    public Task PublishSaleCancelledAsync(Guid saleId, string saleNumber, CancellationToken cancellationToken = default)
    {
        return PublishAsync(new SaleCancelledEvent(saleId, saleNumber, DateTime.UtcNow), cancellationToken);
    }

    public Task PublishItemCancelledAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default)
    {
        return PublishAsync(new ItemCancelledEvent(saleId, itemId, DateTime.UtcNow), cancellationToken);
    }

    private async Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken)
        where TEvent : notnull
    {
        if (_bus is not null)
        {
            await _bus.Publish(message);
            return;
        }

        _logger.LogInformation("{EventName}: {@Event}", typeof(TEvent).Name, message);
    }
}
