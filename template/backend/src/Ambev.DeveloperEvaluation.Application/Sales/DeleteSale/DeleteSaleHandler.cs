using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand>
{
    private readonly ISaleRepository _saleRepository;

    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
            throw new ValidationException("Sale id is required");

        var deleted = await _saleRepository.DeleteAsync(command.Id, cancellationToken);
        if (!deleted)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
    }
}
