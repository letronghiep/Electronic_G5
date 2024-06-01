namespace Electronic_G5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Categories1 = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        [Key]
        public int category_id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được bỏ trống")]
        [StringLength(100)]
        [DisplayName("Tên danh mục")]
        public string category_name { get; set; }

        [DisplayName("Danh mục cha")]
        public int? parent_id { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayName("Ngày tạo")]

        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayName("Ngày cập nhật")]

        public DateTime? updated_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories1 { get; set; }

        public virtual Category Category1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
