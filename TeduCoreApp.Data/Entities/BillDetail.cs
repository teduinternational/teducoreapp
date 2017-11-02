using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("BillDetails")]
    public class BillDetail : DomainEntity<int>
    {
        public BillDetail() { }

        public BillDetail(int id, int billId, int productId, int quantity, decimal price, int colorId, int sizeId)
        {
            Id = id;
            BillId = billId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            ColorId = colorId;
            SizeId = sizeId;
        }

        public BillDetail(int billId, int productId, int quantity, decimal price, int colorId, int sizeId)
        {
            BillId = billId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            ColorId = colorId;
            SizeId = sizeId;
        }
        public int BillId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        [ForeignKey("BillId")]
        public virtual Bill Bill { set; get; }

        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [ForeignKey("ColorId")]
        public virtual Color Color { set; get; }

        [ForeignKey("SizeId")]
        public virtual Size Size { set; get; }
    }
}
