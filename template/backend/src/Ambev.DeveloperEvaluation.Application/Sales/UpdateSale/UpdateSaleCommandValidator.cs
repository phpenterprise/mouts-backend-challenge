using Ambev.DeveloperEvaluation.Application.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.SaleNumber).NotEmpty().MaximumLength(50);
        RuleFor(command => command.SaleDate).NotEmpty();
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(command => command.BranchId).NotEmpty();
        RuleFor(command => command.BranchName).NotEmpty().MaximumLength(100);
        RuleFor(command => command.Items).NotEmpty();
        RuleForEach(command => command.Items).SetValidator(new SaleInputItemValidator());
    }
}
