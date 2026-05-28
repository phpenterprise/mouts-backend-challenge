namespace Ambev.DeveloperEvaluation.Application.Sales;

public record SaleResult(
    Guid Id,
    string SaleNumber,
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    decimal TotalAmount,
    bool IsCancelled,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyCollection<SaleItemDto> Items);
