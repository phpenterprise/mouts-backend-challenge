namespace Ambev.DeveloperEvaluation.Application.Sales;

public record SaleItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalAmount,
    bool IsCancelled);
