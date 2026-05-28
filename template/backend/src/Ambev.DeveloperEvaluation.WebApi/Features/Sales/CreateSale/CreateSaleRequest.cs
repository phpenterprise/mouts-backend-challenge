using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record CreateSaleRequest(
    string SaleNumber,
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    IReadOnlyCollection<SaleItemRequest> Items) : ISaleRequest;
