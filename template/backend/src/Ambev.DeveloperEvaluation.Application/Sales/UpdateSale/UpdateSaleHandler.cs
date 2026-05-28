using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEventPublisher _eventPublisher;

    public UpdateSaleHandler(ISaleRepository saleRepository, ISaleEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateSaleCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (await _saleRepository.ExistsBySaleNumberAsync(command.SaleNumber, command.Id, cancellationToken))
            throw new InvalidOperationException($"Sale number {command.SaleNumber} already exists");

        sale.Update(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName,
            SaleMapper.ToItems(command.Items));

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        await _eventPublisher.PublishSaleModifiedAsync(updatedSale.Id, updatedSale.SaleNumber, cancellationToken);

        return SaleMapper.ToResult(updatedSale);
    }
}
