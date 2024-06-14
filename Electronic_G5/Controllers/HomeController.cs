using Electronic_G5.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult Index()
        {
            //lấy all
            var allproducts = db.Products.Select(p => new ProductViewModel
            {
                Product = p,
                Images = db.Images.Where(i => i.product_id == p.product_id).ToList()
            }).ToList();
            //new pro
            var latestProduct = db.Products.OrderByDescending(p => p.created_at).Select(p => new ProductViewModel
            {
                Product = p,
                Images = db.Images.Where(i => i.product_id == p.product_id).ToList()
            }).ToList();
            // Loại bỏ sản phẩm mới nhất ra khỏi danh sách tất cả sản phẩm
            var filteredAllProducts = allproducts.Except(latestProduct).ToList();
            //best seller


            //hiển thị lên view
            var viewModel = new HomeViewModel
            {
                AllProducts = filteredAllProducts,
                LatestProducts = latestProduct,

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
            return View();
        }

        [Route("BYID1/{category_id?}")]
        public ActionResult BYID(int category_id)
        {
            var pro = db.Products.Where(p => p.category_id == category_id).ToList();
            return View(pro);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}