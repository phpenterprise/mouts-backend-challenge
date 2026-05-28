namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public interface ISaleRequest
{
    string SaleNumber { get; }
    DateTime SaleDate { get; }
    Guid CustomerId { get; }
    string CustomerName { get; }
    Guid BranchId { get; }
    string BranchName { get; }
    IReadOnlyCollection<SaleItemRequest> Items { get; }
}
