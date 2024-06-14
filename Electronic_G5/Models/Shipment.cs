namespace Electronic_G5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shipment()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int shipment_id { get; set; }
        [DisplayName("Ngày giao hàng")]
        public DateTime shipment_date { get; set; }

        [Required(ErrorMessage = "Nhập số nhà hoặc địa chỉ")]
        [StringLength(100)]
        [DisplayName("Số nhà(Địa chỉ cụ thể)")]

        public string address { get; set; }

        [Required(ErrorMessage = "Nhập tỉnh")]
        [StringLength(100)]
        [DisplayName("Tỉnh")]

        public string city { get; set; }

        [Required(ErrorMessage = "Nhập thành phố")]

        [StringLength(100)]
        [DisplayName("Thành phố")]

        public string state { get; set; }

        [Required(ErrorMessage = "Nhập nước")]
        [StringLength(100)]
        [DisplayName("Nước")]

        public string country { get; set; }

        [Required(ErrorMessage = "Mã vùng không được bỏ trống")]
        [StringLength(10)]
        [DisplayName("Mã vùng")]

        public string zip_code { get; set; }

        public int user_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual User User { get; set; }
    }
}
