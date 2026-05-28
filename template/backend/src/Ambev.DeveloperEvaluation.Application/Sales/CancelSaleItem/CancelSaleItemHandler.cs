using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEventPublisher _eventPublisher;

    public CancelSaleItemHandler(ISaleRepository saleRepository, ISaleEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        if (command.SaleId == Guid.Empty || command.ItemId == Guid.Empty)
            throw new ValidationException("Sale id and item id are required");

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        sale.CancelItem(command.ItemId);
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        await _eventPublisher.PublishItemCancelledAsync(updatedSale.Id, command.ItemId, cancellationToken);

        return SaleMapper.ToResult(updatedSale);
    }
}
