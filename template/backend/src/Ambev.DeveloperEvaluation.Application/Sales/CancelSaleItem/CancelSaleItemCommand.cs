using Ambev.DeveloperEvaluation.Application.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest<SaleResult>;
