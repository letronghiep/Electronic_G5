using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electronic_G5.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = new Product();
        public List<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
        public List<Image> Images { get; set; } = new List<Image>();
    }
}