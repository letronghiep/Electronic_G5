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
        private const string CartSession = "CartSession";
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
        [HttpPost]
        public ActionResult AddToCart(int product_id, int quantity, decimal price)
        {
            //lấy user_id từ session đã lưu
            if (Session["UserID"] == null)
            {
                return Json(new { success = false, message = "Vui long dang nhap." });
            }
            int userId = int.Parse(Session["UserID"].ToString());

            //kiểm tra sản phẩm có trong giỏ chưa
            try
            {
                var product = db.Products.Include(x => x.ProductOptions).FirstOrDefault(x => x.product_id == product_id);
                var cart = Session[CartSession];
                if (cart != null)
                {
                    var list = (List<Cart>)cart;
                    if (list.Exists(x => x.product_id == product_id && x.user_id == userId))
                    {
                        foreach (var item in list)
                        {
                            if (item.product_id == product_id)
                            {
                                item.quantity += quantity;
                            }
                        }
                    }
                    else
                    {
                        var item = new Cart();
                        item.product_id = product_id;
                        item.Product = product;
                        item.quantity = quantity;
                        item.price = price;
                        item.user_id = userId;
                        list.Add(item);
                    }
                    Session[CartSession] = list;
                }
                else
                {
                    //tạo sp mới trong giỏ hàng
                    Cart newItem = new Cart();
                    newItem.quantity = quantity;
                    newItem.Product = product;
                    newItem.user_id = userId;
                    newItem.product_id = product_id;
                    newItem.price = price;
                    var list = new List<Cart>();
                    list.Add(newItem);
                    Session[CartSession] = list;

                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                ViewBag.error = "loi" + ex.Message;
                return Json(new { success = false });

            }
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
            //var cartItems = db.Carts.Where(item => item.user_id == userId).ToList();
            var cart = Session[CartSession];
            var cartItems = new List<Cart>();
            if (cart != null)
            {
                cartItems = (List<Cart>)cart;
            }
            return View(cartItems);
        }
        [HttpPost]
        public ActionResult UpdateCartItem(int product_id, int quantity)
        {
            // Check if user is logged in
            if (Session["UserID"] == null)
            {
                TempData["ReturnUrl"] = Request.UrlReferrer.ToString();
                return Json(new { success = false, message = "Vui lòng đăng nhập." });
            }

            // Parse user ID from session
            int userId = int.Parse(Session["UserID"].ToString());

            try
            {
                // Find the cart item for the current user and product
                var cart = Session[CartSession] as List<Cart>;

                if (cart != null)
                {
                    var cartItem = cart.FirstOrDefault(item => item.product_id == product_id);
                    if (cartItem != null)
                    {
                        // Update the quantity
                        cartItem.quantity = quantity;
                        Session[CartSession] = cart;
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });

                }
                else
                {
                    // Handle case where cart item is not found (optional)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions (log the error, return error message, etc.)
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public ActionResult Checkout()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                var userId = int.Parse(Session["UserID"].ToString());
                var user = db.Users.FirstOrDefault(u => u.user_id == userId);
                return View(user);

            }
        }
        [HttpPost]
        public ActionResult Checkout(User user, List<Cart> listCarts, Payment payment, Shipment shipment)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Adding carts to the database
                    if (listCarts != null && listCarts.Any())
                    {
                        db.Carts.AddRange(listCarts);
                        foreach (var cart in listCarts) {
                            var prod = db.Products.FirstOrDefault(p => p.product_id == cart.product_id);
                            if (prod != null) {
                                prod.stock -= cart.quantity;
                                prod.quantity_sold += cart.quantity;
                            }
                            
                        }
                        db.SaveChanges();
                    }

                    // Adding payment to the database
                    Payment payment1 = null;
                    if (payment != null)
                    {
                        payment1 = new Payment
                        {
                            payment_date = DateTime.Now,
                            payment_method = payment.payment_method,
                            amount = payment.amount,
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            user_id = user.user_id,
                        };
                        db.Payments.Add(payment1);
                        db.SaveChanges();
                    }

                    // Adding shipment to the database
                    Shipment shipment1 = null;
                    if (shipment != null)
                    {
                        shipment1 = new Shipment
                        {
                            shipment_date = DateTime.Now,
                            address = shipment.address,
                            city = shipment.city,
                            state = shipment.state,
                            country = shipment.country,
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            user_id = user.user_id,
                            zip_code = shipment.zip_code,
                        };
                        db.Shipments.Add(shipment1);
                        db.SaveChanges();
                    }

                    // Adding order to the database
                    var newOrder = new Order
                    {
                        order_date = DateTime.Now,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                        fulfillment_status = "Đang chờ xử lý",
                        order_status = "Đang xử lý",
                        total_price = listCarts.Sum(c => c.price * c.quantity),
                        payment_id = payment1.payment_id,
                        shipment_id = shipment1.shipment_id,
                        user_id = user.user_id,
                    };
                    db.Orders.Add(newOrder);
                    db.SaveChanges();

                    // Adding order items to the database
                    if (listCarts != null && listCarts.Any())
                    {
                        var orderItems = listCarts.Select(item => new Order_Items
                        {
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            order_id = newOrder.order_id,
                            quantity = item.quantity,
                            product_id = item.product_id,
                            price = item.price,
                            Order = newOrder
                        }).ToList();
                        db.Order_Items.AddRange(orderItems);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                    Session[CartSession] = null;
                    return RedirectToAction("OrderComplete", "Orders");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log the error (uncomment the line below after adding a logger)
                    // _logger.LogError(ex, "Error during checkout");
                    return View("Error");
                }
            }
        }



    }

}
