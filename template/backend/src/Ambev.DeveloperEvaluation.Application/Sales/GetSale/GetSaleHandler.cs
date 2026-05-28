using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<SaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
            throw new ValidationException("Sale id is required");

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        return SaleMapper.ToResult(sale);
    }
}
