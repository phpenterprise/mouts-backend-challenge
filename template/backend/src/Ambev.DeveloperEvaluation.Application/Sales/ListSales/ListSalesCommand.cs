using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public record ListSalesCommand(
    int Page = 1,
    int Size = 10,
    string? SaleNumber = null,
    Guid? CustomerId = null,
    Guid? BranchId = null,
    bool? IsCancelled = null,
    string? OrderBy = null) : IRequest<ListSalesResult>;
