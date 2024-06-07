namespace Electronic_G5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
            Shipments = new HashSet<Shipment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [DisplayName("Họ tên")]

        public string full_name { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [StringLength(100)]
        [DisplayName("Email")]

        public string email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(100)]
        [DisplayName("Mật khẩu")]

        public string password { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [StringLength(100)]
        [DisplayName("Địa chỉ")]

        public string address { get; set; }
        [DisplayName("Sô điện thoại")]

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [StringLength(100)]
        public string phone_number { get; set; }
        [DisplayName("Hình ảnh")]

        [Column(TypeName = "text")]
        public string image { get; set; }

      

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }
        [DisplayName("Quyền")]
        public int role_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
