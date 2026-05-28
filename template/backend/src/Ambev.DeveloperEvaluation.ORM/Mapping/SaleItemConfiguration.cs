using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnType("uuid");
        builder.Property(item => item.SaleId).HasColumnType("uuid").IsRequired();
        builder.Ignore(item => item.ProductId);
        builder.Ignore(item => item.ProductName);
        builder.OwnsOne(item => item.Product, product =>
        {
            product.Property(value => value.Id)
                .HasColumnName("ProductId")
                .HasColumnType("uuid")
                .IsRequired();

            product.Property(value => value.Description)
                .HasColumnName("ProductName")
                .HasMaxLength(100)
                .IsRequired();
        });
        builder.Navigation(item => item.Product).IsRequired();
        builder.Property(item => item.Quantity).IsRequired();
        builder.Property(item => item.UnitPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(item => item.Discount).HasPrecision(18, 2).IsRequired();
        builder.Property(item => item.TotalAmount).HasPrecision(18, 2).IsRequired();
        builder.Property(item => item.IsCancelled).IsRequired();
    }
}
