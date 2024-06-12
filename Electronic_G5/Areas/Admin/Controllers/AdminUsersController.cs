using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq.Dynamic.Core;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Electronic_G5.Models;
using PagedList;
using Antlr.Runtime.Misc;
using System.Data.Entity.Validation;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace Electronic_G5.Areas.Admin.Controllers
{
    public class AdminUsersController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Admin/AdminUsers
        //public ActionResult Index()
        //{
        //    var customers = (from user in db.Users
        //                    where user.role_id == 1
        //                     join o in db.Orders
        //                    on user.user_id equals o.user_id 
        //                    select new Customer
        //                    {
        //                        customer_id = user.user_id,
        //                        customer_name = user.full_name,
        //                        customer_email = user.email,
        //                        customer_order_count = o.Order_Items.Count(),
        //                        customer_order_price = o.Order_Items.Sum(x => x.price),
        //                        customer_city = user.address,
        //                        customer_phone = user.phone_number,
        //                        customer_last_order = o.order_date
        //                    }).ToList();

        //    //var users = db.Users.Include(u => u.Role);
        //    return View(customers);
        //}
        public ActionResult Index(int? page, int? pageSize, string searchString, string sortProperty, string sortOrder = "")
        {
            //return View(products.ToList());

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });

            items.Add(new SelectListItem { Text = "All", Value = "all" });
            var customers = (from user in db.Users
                             where user.Role.role_name == "Customer" || user.Role.role_name == "User"
                             join o in db.Orders
                             on user.user_id equals o.user_id into userOrders
                             from order in userOrders.DefaultIfEmpty()
                             group new { user, order } by new
                             {
                                 user.user_id,
                                 user.image,
                                 user.full_name,
                                 user.email,
                                 user.address,
                                 user.phone_number
                             } into grouped
                             select new Customer
                             {
                                 customer_id = grouped.Key.user_id,
                                 customer_image_url = grouped.Key.image,
                                 customer_name = grouped.Key.full_name,
                                 customer_email = grouped.Key.email,
                                 customer_order_count = grouped.Count(g => g.order != null),
                                 customer_order_price = grouped.Sum(g => (decimal?)g.order.total_price ?? 0),
                                 customer_city = grouped.Key.address,
                                 customer_phone = grouped.Key.phone_number,
                                 customer_last_order = grouped.Max(g => (DateTime?)g.order.order_date ?? DateTime.Now)
                             });

            // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
            foreach (var item in items)
            {
                if (item.Value == pageSize.ToString()) item.Selected = true;
            }

            // 1.2. Tạo các biến ViewBag
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = pageSize; // tạo biến kích thước trang hiện tại

            // 2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            // 4. Tạo kích thước trang (pageSize), mặc định là 5.
            int size = (pageSize ?? 5);

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.


            // Search

            ViewBag.Keyword = searchString;
            int defaultPageSize = 10;
            if (pageSize.HasValue && pageSize > 0)
            {
                defaultPageSize = pageSize.Value;
            }
            if (!String.IsNullOrEmpty(searchString))
                customers = customers.Where(ct => ct.customer_name.Contains(searchString)).OrderByDescending(ct => ct.customer_last_order);
            switch (sortOrder)
            {
                case "asc":
                    ViewBag.SortOrder = "desc";
                    break;
                case "desc":
                    ViewBag.SortOrder = "";
                    break;
                case "":
                    ViewBag.SortOrder = "asc";
                    break;
            }
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "customer_name";
            if (sortOrder == "desc")
            {
                customers = customers.OrderBy(sortProperty + " desc");
            }
            else
            {
                customers = customers.OrderBy(sortProperty);
            }


            if (page == null) page = 1;
            int pageNumber = (page ?? 1);

            return View(customers.ToPagedList(pageNumber, defaultPageSize));
        }

        // GET: Admin/AdminUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/AdminUsers/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name");
            return View();
        }

        // POST: Admin/AdminUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,full_name,email,password,address,phone_number,image,role,created_at,updated_at,role_id")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.image = "";
                    var imageInput = Request.Files["imageInput"];
                    if (imageInput != null && imageInput.ContentLength > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(imageInput);

                        // Lấy tên tệp và đường dẫn để lưu trữ
                        string filename = Path.GetFileName(imageInput.FileName);
                        string path = Path.Combine(Server.MapPath("~/wwwroot/Images/"), filename);
                        imageInput.SaveAs(path);
                        System.Diagnostics.Debug.WriteLine("OKE");

                        // Cập nhật thuộc tính image_url của sản phẩm
                        user.image = filename;
                    }
                    // Cập nhật thời gian tạo và cập nhật của sản phẩm
                    user.created_at = DateTime.Now;
                    user.updated_at = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
                return View(user);
            }

        }

        // GET: Admin/AdminUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            return View(user);
        }

        // POST: Admin/AdminUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,full_name,email,password,address,phone_number,image,role,created_at,updated_at,role_id")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var imageInput = Request.Files["imageInput"];
                    if (imageInput != null && imageInput.ContentLength > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(imageInput);

                        // Lấy tên tệp và đường dẫn để lưu trữ
                        string filename = Path.GetFileName(imageInput.FileName);
                        string path = Path.Combine(Server.MapPath("~/wwwroot/Images/"), filename);
                        imageInput.SaveAs(path);

                        // Cập nhật thuộc tính image_url của sản phẩm
                        user.image = filename;
                    }
                    user.updated_at = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
                return View(user);
            }


        }

        // GET: Admin/AdminUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);

            db.Users.Remove(user);
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
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Login(User model, bool remember = false)
        {
            try
            {
                //    if (ModelState.IsValid)
                //    {
                // Tìm user trong database
                var user = db.Users.FirstOrDefault(u => u.email == model.email && u.password == model.password);

                if (user != null)
                {
                    // Tạo session cho user
                    //Session["email"] = user.email.ToString();
                    //Session["password"] = user.password.ToString();
                    Session["UserID"] = user.user_id.ToString();
                    Session["UserName"] = user.full_name.ToString();

                    if (remember)
                    {
                        // Lưu thông tin đăng nhập vào cookie
                        HttpCookie cookie = new HttpCookie("UserLogin");
                        cookie.Values.Add("email", model.email);
                        cookie.Values.Add("password", model.password);
                        cookie.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(cookie);
                    }

                    return RedirectToAction("Index", "AdminHome");
                }
                else
                {
                    ViewBag.error = "Email hoặc mật khẩu không đúng.";
                }
                // }
                return View(model);
            }
            catch (Exception ex)
            {

                ViewBag.error = "Có lỗi" + ex.Message;
                return View(model);
            }
        }

        // GET: Users/Logout
        public ActionResult Logout()
        {
            // Xóa thông tin người dùng từ session hoặc cookie
            Session.Clear();
            // Chuyển hướng đến trang đăng nhập sau khi đăng xuất
            return RedirectToAction("Login");
        }
    }
}
