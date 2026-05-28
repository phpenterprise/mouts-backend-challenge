using Ambev.DeveloperEvaluation.Application.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleCommand(Guid Id) : IRequest<SaleResult>;
