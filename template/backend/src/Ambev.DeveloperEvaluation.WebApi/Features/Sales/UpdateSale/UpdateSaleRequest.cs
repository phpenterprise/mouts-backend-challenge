using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public record UpdateSaleRequest(
    string SaleNumber,
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    IReadOnlyCollection<SaleItemRequest> Items) : ISaleRequest;
