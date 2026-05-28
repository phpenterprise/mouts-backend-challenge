namespace Ambev.DeveloperEvaluation.Application.Services;

public interface ISaleEventPublisher
{
    Task PublishSaleCreatedAsync(Guid saleId, string saleNumber, CancellationToken cancellationToken = default);
    Task PublishSaleModifiedAsync(Guid saleId, string saleNumber, CancellationToken cancellationToken = default);
    Task PublishSaleCancelledAsync(Guid saleId, string saleNumber, CancellationToken cancellationToken = default);
    Task PublishItemCancelledAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default);
}
