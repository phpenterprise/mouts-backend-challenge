using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = [];

    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public ExternalIdentity Customer { get; private set; } = null!;
    public ExternalIdentity Branch { get; private set; } = null!;
    public Guid CustomerId => Customer.Id;
    public string CustomerName => Customer.Description;
    public Guid BranchId => Branch.Id;
    public string BranchName => Branch.Description;
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private Sale()
    {
    }

    public Sale(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName,
        IEnumerable<SaleItem> items)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;

        SetHeader(saleNumber, saleDate, customerId, customerName, branchId, branchName);
        ReplaceItems(items);
    }

    public void Update(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName,
        IEnumerable<SaleItem> items)
    {
        EnsureActive();
        SetHeader(saleNumber, saleDate, customerId, customerName, branchId, branchName);
        ReplaceItems(items);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        EnsureActive();
        IsCancelled = true;

        foreach (var item in _items.Where(item => !item.IsCancelled))
            item.Cancel();

        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    public void CancelItem(Guid itemId)
    {
        EnsureActive();

        var item = _items.FirstOrDefault(item => item.Id == itemId)
            ?? throw new DomainException("Sale item not found");

        item.Cancel();
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(error => (ValidationErrorDetail)error)
        };
    }

    private void SetHeader(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName)
    {
        SaleNumber = saleNumber.Trim();
        SaleDate = saleDate.Kind == DateTimeKind.Utc
            ? saleDate
            : saleDate.ToUniversalTime();
        Customer = new ExternalIdentity(customerId, customerName);
        Branch = new ExternalIdentity(branchId, branchName);
    }

    private void ReplaceItems(IEnumerable<SaleItem> items)
    {
        var requestedItems = items.ToList();
        var duplicatedProduct = requestedItems
            .GroupBy(item => item.ProductId)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicatedProduct is not null)
            throw new DomainException("Sale cannot contain duplicated products");

        _items.RemoveAll(item => requestedItems.All(requested => requested.ProductId != item.ProductId));

        foreach (var requestedItem in requestedItems)
        {
            var currentItem = _items.FirstOrDefault(item => item.ProductId == requestedItem.ProductId);

            if (currentItem is null)
            {
                _items.Add(requestedItem);
                continue;
            }

            currentItem.Update(
                requestedItem.ProductId,
                requestedItem.ProductName,
                requestedItem.Quantity,
                requestedItem.UnitPrice);
        }

        var validation = Validate();
        if (!validation.IsValid)
            throw new DomainException(string.Join("; ", validation.Errors.Select(error => error.Detail)));

        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Where(item => !item.IsCancelled).Sum(item => item.TotalAmount);
    }

    private void EnsureActive()
    {
        if (IsCancelled)
            throw new DomainException("Cancelled sales cannot be changed");
    }
}
