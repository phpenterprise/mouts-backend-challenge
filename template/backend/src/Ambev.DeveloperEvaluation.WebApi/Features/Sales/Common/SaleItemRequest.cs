namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public record SaleItemRequest(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
