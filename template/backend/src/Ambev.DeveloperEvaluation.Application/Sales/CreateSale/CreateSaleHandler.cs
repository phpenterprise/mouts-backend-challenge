using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEventPublisher _eventPublisher;

    public CreateSaleHandler(ISaleRepository saleRepository, ISaleEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateSaleCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (await _saleRepository.ExistsBySaleNumberAsync(command.SaleNumber, cancellationToken: cancellationToken))
            throw new InvalidOperationException($"Sale number {command.SaleNumber} already exists");

        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName,
            SaleMapper.ToItems(command.Items));

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        await _eventPublisher.PublishSaleCreatedAsync(createdSale.Id, createdSale.SaleNumber, cancellationToken);

        return SaleMapper.ToResult(createdSale);
    }
}
