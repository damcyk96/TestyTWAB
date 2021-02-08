using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestyDB
{
    public class OrderDetailsCongifuration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.ToTable("order_details");
            builder.HasKey(x => new
            {
                x.OrderId,
                x.ProductId
            });
            builder
                .Property(x => x.OrderId)
                .HasColumnName("order_id");
            builder
                .Property(x => x.ProductId)
                .HasColumnName("product_id");
            builder
                .Property(x => x.UnitPrice)
                .HasColumnName("unit_price");
            builder
                .Property(x => x.Quantity)
                .HasColumnName("quantity");
            builder
                .Property(x => x.Discount)
                .HasColumnName("discount");
        }
    }
}