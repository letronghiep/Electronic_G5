using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Electronic_G5.Models;

namespace Electronic_G5.Controllers
{
    public class CartsController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Product).Include(c => c.User);
            return View(carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name");
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cart_id,user_id,quantity,product_id")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", cart.product_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name", cart.user_id);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", cart.product_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name", cart.user_id);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cart_id,user_id,quantity,product_id")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", cart.product_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name", cart.user_id);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AddToCart(int productId, int _quantity)
        {
            //lấy user_id từ session đã lưu
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login","Users");
            }
            int userId = int.Parse(Session["UserID"].ToString());
          
            //kiểm tra sản phẩm có trong giỏ chưa
            try
            {
                var existingItem = db.Carts.FirstOrDefault(item => item.product_id == productId && item.user_id == userId);
                if (existingItem != null)
                {
                    existingItem.quantity += _quantity;
                }
                else
                {
                    //tạo sp mới trong giỏ hàng
                    Cart newItem = new Cart();
                    newItem.quantity = _quantity;
                    newItem.user_id = userId;
                    newItem.product_id = productId;
                    //var newItem = new Cart
                    //{
                    //    product_id = productId,
                    //    quantity = _quantity,
                    //    user_id = userId
                    //};
                    db.Carts.Add(newItem);
                }
               
                System.Diagnostics.Debug.WriteLine(productId);
                System.Diagnostics.Debug.WriteLine(_quantity);
                System.Diagnostics.Debug.WriteLine(userId);

                db.SaveChanges();
            } 
            catch (Exception ex)
            {

               ViewBag.error = "loi" + ex.Message;
            }
            return RedirectToAction("Cart","Home");
        }
      
         

    }
    
}
