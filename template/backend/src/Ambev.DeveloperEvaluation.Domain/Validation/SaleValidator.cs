using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber).NotEmpty().MaximumLength(50);
        RuleFor(sale => sale.SaleDate).NotEmpty();
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(sale => sale.BranchId).NotEmpty();
        RuleFor(sale => sale.BranchName).NotEmpty().MaximumLength(100);
        RuleFor(sale => sale.Items).NotEmpty();
    }
}
