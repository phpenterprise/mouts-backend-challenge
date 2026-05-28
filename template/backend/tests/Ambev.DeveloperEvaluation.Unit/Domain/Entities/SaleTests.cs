using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Theory(DisplayName = "Sale item calculates quantity discount according to business rules")]
    [InlineData(3, 10, 0, 30)]
    [InlineData(4, 10, 4, 36)]
    [InlineData(9, 10, 9, 81)]
    [InlineData(10, 10, 20, 80)]
    [InlineData(20, 10, 40, 160)]
    public void SaleItem_ShouldCalculateDiscountByQuantity(int quantity, decimal unitPrice, decimal discount, decimal total)
    {
        var item = new SaleItem(Guid.NewGuid(), "Product", quantity, unitPrice);

        item.Discount.Should().Be(discount);
        item.TotalAmount.Should().Be(total);
    }

    [Fact(DisplayName = "Sale item rejects quantities above twenty")]
    public void SaleItem_ShouldRejectQuantityAboveTwenty()
    {
        var act = () => new SaleItem(Guid.NewGuid(), "Product", 21, 10);

        act.Should().Throw<DomainException>()
            .WithMessage("It is not possible to sell more than 20 identical items");
    }

    [Fact(DisplayName = "Sale total ignores cancelled items")]
    public void Sale_ShouldIgnoreCancelledItemsInTotal()
    {
        var sale = CreateSale(
            new SaleItem(Guid.NewGuid(), "Product A", 4, 10),
            new SaleItem(Guid.NewGuid(), "Product B", 2, 15));
        var itemToCancel = sale.Items.First();

        sale.CancelItem(itemToCancel.Id);

        sale.TotalAmount.Should().Be(30);
        itemToCancel.IsCancelled.Should().BeTrue();
        itemToCancel.TotalAmount.Should().Be(0);
    }

    [Fact(DisplayName = "Sale cancellation cancels all items and zeroes total")]
    public void Sale_ShouldCancelAllItems()
    {
        var sale = CreateSale(
            new SaleItem(Guid.NewGuid(), "Product A", 4, 10),
            new SaleItem(Guid.NewGuid(), "Product B", 10, 5));

        sale.Cancel();

        sale.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(0);
        sale.Items.Should().OnlyContain(item => item.IsCancelled);
    }

    private static Sale CreateSale(params SaleItem[] items)
    {
        return new Sale(
            "SALE-001",
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Customer",
            Guid.NewGuid(),
            "Branch",
            items);
    }
}
