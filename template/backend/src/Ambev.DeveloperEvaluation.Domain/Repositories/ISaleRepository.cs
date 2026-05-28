using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Sale>> ListAsync(
        int page,
        int size,
        string? saleNumber = null,
        Guid? customerId = null,
        Guid? branchId = null,
        bool? isCancelled = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        string? saleNumber = null,
        Guid? customerId = null,
        Guid? branchId = null,
        bool? isCancelled = null,
        CancellationToken cancellationToken = default);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySaleNumberAsync(string saleNumber, Guid? ignoredSaleId = null, CancellationToken cancellationToken = default);
}
