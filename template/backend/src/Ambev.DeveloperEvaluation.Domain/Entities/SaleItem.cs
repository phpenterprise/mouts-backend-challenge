using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public ExternalIdentity Product { get; private set; } = null!;
    public Guid ProductId => Product.Id;
    public string ProductName => Product.Description;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    public Sale Sale { get; private set; } = null!;

    private SaleItem()
    {
    }

    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        Product = new ExternalIdentity(productId, productName);
        Quantity = quantity;
        UnitPrice = unitPrice;

        Recalculate();
    }

    public void Update(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        EnsureActive();

        Product = new ExternalIdentity(productId, productName);
        Quantity = quantity;
        UnitPrice = unitPrice;

        Recalculate();
    }

    public void Cancel()
    {
        EnsureActive();
        IsCancelled = true;
        Discount = 0;
        TotalAmount = 0;
    }

    private void Recalculate()
    {
        if (Quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        if (Quantity > 20)
            throw new DomainException("It is not possible to sell more than 20 identical items");

        if (UnitPrice <= 0)
            throw new DomainException("Unit price must be greater than zero");

        var grossAmount = Quantity * UnitPrice;
        var discountPercentage = Quantity >= 10 ? 0.20m : Quantity >= 4 ? 0.10m : 0m;

        Discount = decimal.Round(grossAmount * discountPercentage, 2);
        TotalAmount = decimal.Round(grossAmount - Discount, 2);
    }

    private void EnsureActive()
    {
        if (IsCancelled)
            throw new DomainException("Cancelled sale items cannot be changed");
    }
}
