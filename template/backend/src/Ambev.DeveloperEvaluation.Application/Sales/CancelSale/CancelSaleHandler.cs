using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEventPublisher _eventPublisher;

    public CancelSaleHandler(ISaleRepository saleRepository, ISaleEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
            throw new ValidationException("Sale id is required");

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        sale.Cancel();
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        await _eventPublisher.PublishSaleCancelledAsync(updatedSale.Id, updatedSale.SaleNumber, cancellationToken);

        return SaleMapper.ToResult(updatedSale);
    }
}
