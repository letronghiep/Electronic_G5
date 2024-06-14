using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electronic_G5.Models
{
    public class HomeViewModel
    {
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
        public List<ProductViewModel> LatestProducts { get; set; } = new List<ProductViewModel>();
     //   public List<BestSelling> TopSalesProducts { get; set; } = new List<BestSelling> ();

    }
}