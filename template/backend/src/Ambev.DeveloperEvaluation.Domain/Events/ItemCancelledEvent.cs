namespace Ambev.DeveloperEvaluation.Domain.Events;

public record ItemCancelledEvent(Guid SaleId, Guid ItemId, DateTime OccurredAt);
