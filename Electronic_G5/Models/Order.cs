namespace Electronic_G5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Items = new HashSet<Order_Items>();
        }

        [Key]
        [DisplayName("Mã hóa đơn")]
        public int order_id { get; set; }

        [DisplayName("Ngày đặt hàng")]

        public DateTime order_date { get; set; }
        [DisplayName("Tổng")]

        public decimal total_price { get; set; }
        [DisplayName("Khách hàng")]

        public int user_id { get; set; }

        [DisplayName("Trạng thái đơn hàng")]

        public string order_status { get; set; }
        [DisplayName("Trạng thái giao hàng")]

        public string fulfillment_status { get; set; }
        [DisplayName("Mã thanh toán")]
        public int payment_id { get; set; }
        [DisplayName("Mã giao hàng")]
     
        public int shipment_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Items> Order_Items { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual Shipment Shipment { get; set; }

        public virtual User User { get; set; }
    }
}
