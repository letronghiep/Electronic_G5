using Electronic_G5.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Electronic_G5.Controllers
{
    public class HomeController : Controller
    {
        private ElectronicDb db = new ElectronicDb();



        [AllowAnonymous]

        public ActionResult Index(string filter)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            switch (filter)
            {
                case "latest":
                    // Hiển thị danh sách sản phẩm mới nhất
                    products = db.Products.OrderByDescending(p => p.created_at).Select(p => new ProductViewModel
                    {
                        Product = p,
                        Images = db.Images.Where(i => i.product_id == p.product_id).ToList()
                    }).ToList();
                    break;

                case "bestselling":
                    // Hiển thị danh sách sản phẩm bán chạy nhất
                    var topSalesProducts = db.Order_Items.GroupBy(o => o.product_id).Select(g => new
                    {
                        product_id = g.Key,
                        order_quantity = g.Sum(oi => oi.quantity),
                        product = g.FirstOrDefault().Product
                    }).OrderByDescending(g => g.order_quantity).ToList();

                    products = topSalesProducts.Select(g => new ProductViewModel
                    {
                        Product = g.product,
                        Images = db.Images.Where(i => i.product_id == g.product.product_id).ToList()
                    }).ToList();
                    break;
                    //sort
                    

                default:
                    // Hiển thị tất cả sản phẩm
                    products = db.Products.Select(p => new ProductViewModel
                    {
                        Product = p,
                        Images = db.Images.Where(i => i.product_id == p.product_id).ToList()
                    }).ToList();
                    break;
            }

            //hiển thị lên view
            var viewModel = new HomeViewModel
            {
                Products = products,

            };
            return View(viewModel);

        }

        public ActionResult ChiTietSP(int? product_id)
        {
            if (product_id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            var product = db.Products.FirstOrDefault(
                p => p.product_id == product_id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        public ActionResult Cart()
        {
            // kiểm tra xem đăng nhập chưa
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            int userId = int.Parse(Session["UserID"].ToString());
            // danh sách sp trong giỏ
            var cartItems = db.Carts.Where(item => item.user_id == userId).ToList();

            return View(cartItems);
        }

        [Route("BYID1/{category_id?}")]
        public ActionResult BYID(int category_id)
        {
            var pro = db.Products.Where(p => p.category_id == category_id).ToList();
            return View(pro);
        }
       
    }
}