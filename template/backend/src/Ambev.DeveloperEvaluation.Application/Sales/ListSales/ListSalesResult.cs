using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public record ListSalesResult(
    IReadOnlyCollection<SaleResult> Data,
    int CurrentPage,
    int TotalPages,
    int TotalCount);
