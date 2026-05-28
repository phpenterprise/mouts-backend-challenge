using Ambev.DeveloperEvaluation.Application.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleCommand(
    string SaleNumber,
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    IReadOnlyCollection<SaleInputItem> Items) : IRequest<SaleResult>;
