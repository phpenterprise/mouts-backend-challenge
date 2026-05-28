using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly ISaleRepository _saleRepository;

    public ListSalesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ListSalesResult> Handle(ListSalesCommand command, CancellationToken cancellationToken)
    {
        if (command.Page < 1)
            throw new ValidationException("Page must be greater than zero");

        if (command.Size is < 1 or > 100)
            throw new ValidationException("Size must be between 1 and 100");

        var totalCount = await _saleRepository.CountAsync(
            command.SaleNumber,
            command.CustomerId,
            command.BranchId,
            command.IsCancelled,
            cancellationToken);

        var sales = await _saleRepository.ListAsync(
            command.Page,
            command.Size,
            command.SaleNumber,
            command.CustomerId,
            command.BranchId,
            command.IsCancelled,
            command.OrderBy,
            cancellationToken);
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)command.Size);

        return new ListSalesResult(
            sales.Select(SaleMapper.ToResult).ToList(),
            command.Page,
            totalPages,
            totalCount);
    }
}
