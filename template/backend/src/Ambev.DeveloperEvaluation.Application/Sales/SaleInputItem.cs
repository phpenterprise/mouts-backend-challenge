namespace Ambev.DeveloperEvaluation.Application.Sales;

public record SaleInputItem(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
