using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(sale => sale.Id);
        builder.Property(sale => sale.Id).HasColumnType("uuid");
        builder.Ignore(sale => sale.CustomerId);
        builder.Ignore(sale => sale.CustomerName);
        builder.Ignore(sale => sale.BranchId);
        builder.Ignore(sale => sale.BranchName);
        builder.Property(sale => sale.SaleNumber).IsRequired().HasMaxLength(50);
        builder.Property(sale => sale.SaleDate).IsRequired();
        builder.OwnsOne(sale => sale.Customer, customer =>
        {
            customer.Property(value => value.Id)
                .HasColumnName("CustomerId")
                .HasColumnType("uuid")
                .IsRequired();

            customer.Property(value => value.Description)
                .HasColumnName("CustomerName")
                .HasMaxLength(100)
                .IsRequired();
        });
        builder.Navigation(sale => sale.Customer).IsRequired();

        builder.OwnsOne(sale => sale.Branch, branch =>
        {
            branch.Property(value => value.Id)
                .HasColumnName("BranchId")
                .HasColumnType("uuid")
                .IsRequired();

            branch.Property(value => value.Description)
                .HasColumnName("BranchName")
                .HasMaxLength(100)
                .IsRequired();
        });
        builder.Navigation(sale => sale.Branch).IsRequired();
        builder.Property(sale => sale.TotalAmount).HasPrecision(18, 2);
        builder.Property(sale => sale.IsCancelled).IsRequired();
        builder.Property(sale => sale.CreatedAt).IsRequired();
        builder.Property(sale => sale.UpdatedAt);

        builder.HasIndex(sale => sale.SaleNumber).IsUnique();

        builder
            .HasMany(sale => sale.Items)
            .WithOne(item => item.Sale)
            .HasForeignKey(item => item.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(sale => sale.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
