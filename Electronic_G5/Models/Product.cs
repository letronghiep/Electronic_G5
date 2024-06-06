namespace Electronic_G5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Carts = new HashSet<Cart>();
            Images = new HashSet<Image>();
            Order_Items = new HashSet<Order_Items>();
            ProductOptions = new HashSet<ProductOption>();
        }

        [Key]
        public int product_id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
        [StringLength(100)]
        [DisplayName("Tên sản phẩm")]
        public string product_name { get; set; }

        [Required(ErrorMessage = "SKU không được bỏ trống")]
        [StringLength(100)]
        public string SKU { get; set; }

        [Column(TypeName = "text")]
        [DisplayName("Ảnh")]
        public string image_url { get; set; }

        [StringLength(100)]
        [DisplayName("Mô tả")]
        [AllowHtml]
        public string description { get; set; }
        [DisplayName("Số lượng")]

        public int stock { get; set; }

        public int category_id { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayName("Ngày tạo mới")]

        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayName("Ngày cập nhật")]

        public DateTime? updated_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual Category Category { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Items> Order_Items { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOption> ProductOptions { get; set; }
    }
}
