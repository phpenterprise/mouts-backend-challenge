using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new SaleRequestValidator<CreateSaleRequest>().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var result = await _mediator.Send(new CreateSaleCommand(
            request.SaleNumber,
            request.SaleDate,
            request.CustomerId,
            request.CustomerName,
            request.BranchId,
            request.BranchName,
            ToInputItems(request.Items)), cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<SaleResult>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = result
        });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleCommand(id), cancellationToken);

        return Ok(new ApiResponseWithData<SaleResult>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = result
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<SaleResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListSales(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? saleNumber = null,
        [FromQuery] Guid? customerId = null,
        [FromQuery] Guid? branchId = null,
        [FromQuery] bool? isCancelled = null,
        [FromQuery] string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new ListSalesCommand(page, size, saleNumber, customerId, branchId, isCancelled, orderBy),
            cancellationToken);

        return Ok(new PaginatedResponse<SaleResult>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = result.Data,
            CurrentPage = result.CurrentPage,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount
        });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale(
        [FromRoute] Guid id,
        [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await new SaleRequestValidator<UpdateSaleRequest>().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var result = await _mediator.Send(new UpdateSaleCommand(
            id,
            request.SaleNumber,
            request.SaleDate,
            request.CustomerId,
            request.CustomerName,
            request.BranchId,
            request.BranchName,
            ToInputItems(request.Items)), cancellationToken);

        return Ok(new ApiResponseWithData<SaleResult>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = result
        });
    }

    [HttpPatch("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelSaleCommand(id), cancellationToken);

        return Ok(new ApiResponseWithData<SaleResult>
        {
            Success = true,
            Message = "Sale cancelled successfully",
            Data = result
        });
    }

    [HttpPatch("{id}/items/{itemId}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSaleItem(
        [FromRoute] Guid id,
        [FromRoute] Guid itemId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelSaleItemCommand(id, itemId), cancellationToken);

        return Ok(new ApiResponseWithData<SaleResult>
        {
            Success = true,
            Message = "Sale item cancelled successfully",
            Data = result
        });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSaleCommand(id), cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Sale deleted successfully"
        });
    }

    private static IReadOnlyCollection<SaleInputItem> ToInputItems(IEnumerable<SaleItemRequest> items)
    {
        return items
            .Select(item => new SaleInputItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice))
            .ToList();
    }
}
