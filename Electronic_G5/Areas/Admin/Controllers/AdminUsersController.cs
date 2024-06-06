using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Electronic_G5.Models;

namespace Electronic_G5.Areas.Admin.Controllers
{
    public class AdminUsersController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Admin/AdminUsers
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role1);
            return View(users.ToList());
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
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            return View(user);
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
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            return View(user);
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
        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.email == model.email && u.password == model.password);
                if (user != null)
                {
                    // Đăng nhập thành công, lưu thông tin người dùng vào session hoặc cookie
                    // Session["user_id"] = user.user_id;
                    Session["email"] = user.email;
                    Session["password"] = user.password;

                    // Chuyển hướng đến trang chính sau khi đăng nhập thành công
                    return RedirectToAction("Index", "AdminHome");
                }
                else
                {
                    // Thông báo lỗi khi đăng nhập không thành công
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác!");
                }
            }

            // Trả về view đăng nhập với model nếu có lỗi
            return View(model);
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
