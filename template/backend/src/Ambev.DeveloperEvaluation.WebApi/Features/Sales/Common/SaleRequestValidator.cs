using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public class SaleRequestValidator<T> : AbstractValidator<T>
    where T : ISaleRequest
{
    public SaleRequestValidator()
    {
        RuleFor(request => request.SaleNumber).NotEmpty().MaximumLength(50);
        RuleFor(request => request.SaleDate).NotEmpty();
        RuleFor(request => request.CustomerId).NotEmpty();
        RuleFor(request => request.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(request => request.BranchId).NotEmpty();
        RuleFor(request => request.BranchName).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Items).NotEmpty();
        RuleForEach(request => request.Items).ChildRules(item =>
        {
            item.RuleFor(value => value.ProductId).NotEmpty();
            item.RuleFor(value => value.ProductName).NotEmpty().MaximumLength(100);
            item.RuleFor(value => value.Quantity).InclusiveBetween(1, 20);
            item.RuleFor(value => value.UnitPrice).GreaterThan(0);
        });
    }
}
