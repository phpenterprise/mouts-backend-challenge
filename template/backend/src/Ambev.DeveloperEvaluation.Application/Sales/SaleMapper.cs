using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public static class SaleMapper
{
    public static SaleResult ToResult(Sale sale)
    {
        return new SaleResult(
            sale.Id,
            sale.SaleNumber,
            sale.SaleDate,
            sale.CustomerId,
            sale.CustomerName,
            sale.BranchId,
            sale.BranchName,
            sale.TotalAmount,
            sale.IsCancelled,
            sale.CreatedAt,
            sale.UpdatedAt,
            sale.Items
                .OrderBy(item => item.ProductName)
                .Select(item => new SaleItemDto(
                    item.Id,
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice,
                    item.Discount,
                    item.TotalAmount,
                    item.IsCancelled))
                .ToList());
    }

    public static IReadOnlyCollection<SaleItem> ToItems(IEnumerable<SaleInputItem> items)
    {
        return items
            .Select(item => new SaleItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice))
            .ToList();
    }
}
