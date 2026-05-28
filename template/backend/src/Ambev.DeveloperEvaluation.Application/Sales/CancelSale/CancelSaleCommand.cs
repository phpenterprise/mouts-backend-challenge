using Ambev.DeveloperEvaluation.Application.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public record CancelSaleCommand(Guid Id) : IRequest<SaleResult>;
