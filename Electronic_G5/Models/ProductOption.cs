namespace Electronic_G5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOption
    {
        [Key]
        public int product_option_id { get; set; }

        public int product_id { get; set; }

        [StringLength(255)]
        public string product_option_name { get; set; }

        [StringLength(255)]
        public string product_option_value { get; set; }

        public decimal product_option_price { get; set; }

        public virtual Product Product { get; set; }
    }
}
