using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(sale => sale.Items)
            .FirstOrDefaultAsync(sale => sale.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Sale>> ListAsync(
        int page,
        int size,
        string? saleNumber = null,
        Guid? customerId = null,
        Guid? branchId = null,
        bool? isCancelled = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySorting(
            ApplyFilters(_context.Sales.AsNoTracking(), saleNumber, customerId, branchId, isCancelled),
            orderBy);

        return await query
            .AsNoTracking()
            .Include(sale => sale.Items)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        string? saleNumber = null,
        Guid? customerId = null,
        Guid? branchId = null,
        bool? isCancelled = null,
        CancellationToken cancellationToken = default)
    {
        return await ApplyFilters(_context.Sales, saleNumber, customerId, branchId, isCancelled)
            .CountAsync(cancellationToken);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale is null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsBySaleNumberAsync(
        string saleNumber,
        Guid? ignoredSaleId = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AnyAsync(
            sale => sale.SaleNumber == saleNumber && (!ignoredSaleId.HasValue || sale.Id != ignoredSaleId.Value),
            cancellationToken);
    }

    private static IQueryable<Sale> ApplyFilters(
        IQueryable<Sale> query,
        string? saleNumber,
        Guid? customerId,
        Guid? branchId,
        bool? isCancelled)
    {
        if (!string.IsNullOrWhiteSpace(saleNumber))
            query = query.Where(sale => sale.SaleNumber.Contains(saleNumber));

        if (customerId.HasValue)
            query = query.Where(sale => sale.Customer.Id == customerId.Value);

        if (branchId.HasValue)
            query = query.Where(sale => sale.Branch.Id == branchId.Value);

        if (isCancelled.HasValue)
            query = query.Where(sale => sale.IsCancelled == isCancelled.Value);

        return query;
    }

    private static IQueryable<Sale> ApplySorting(IQueryable<Sale> query, string? orderBy)
    {
        return orderBy?.Trim().ToLowerInvariant() switch
        {
            "salenumber" => query.OrderBy(sale => sale.SaleNumber),
            "-salenumber" => query.OrderByDescending(sale => sale.SaleNumber),
            "totalamount" => query.OrderBy(sale => sale.TotalAmount),
            "-totalamount" => query.OrderByDescending(sale => sale.TotalAmount),
            "saledate" => query.OrderBy(sale => sale.SaleDate),
            "-saledate" => query.OrderByDescending(sale => sale.SaleDate).ThenBy(sale => sale.SaleNumber),
            _ => query.OrderByDescending(sale => sale.SaleDate).ThenBy(sale => sale.SaleNumber)
        };
    }
}
