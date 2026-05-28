using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public class SaleInputItemValidator : AbstractValidator<SaleInputItem>
{
    public SaleInputItemValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty().MaximumLength(100);
        RuleFor(item => item.Quantity).InclusiveBetween(1, 20);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
    }
}
