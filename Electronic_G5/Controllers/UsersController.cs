using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Electronic_G5.Models;

namespace Electronic_G5.Controllers
{
    public class UsersController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role1);
            return View(users.ToList());
        }

        // GET: Users/Details/5
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

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name");
            return View();
        }

        // POST: Users/Create
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

        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
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

        // GET: Users/Delete/5
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

        // POST: Users/Delete/5
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

        //login
        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
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
                    // Lưu thông báo vào TempData
                    TempData["SuccessMessage"] = "Bạn đã đăng nhập thành công.";
                    // Chuyển hướng đến trang chính sau khi đăng nhập thành công
                    return RedirectToAction("Index", "Home");
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


        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]

        public ActionResult Register(User model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu chưa
                if (db.Users.Any(u => u.email == model.email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                // Tạo một đối tượng User mới từ dữ liệu đăng ký
                User user = new User();
                user.full_name = model.full_name;
                user.email = model.email;
                user.password = model.password;
                user.address = model.address;
                user.phone_number = model.phone_number;
                user.image = model.image;
                user.role = model.role;
                user.role_id = model.role_id;
                //User newUser = new User
                //{
                //    full_name = 
                //    email =,
                //    password = ,
                //    address = string.Empty,
                //    phone_number = string.Empty,
                //    image = string.Empty,
                //    role = model.role,
                //    role_id = model.role_id,
                // Các thuộc tính khác nếu có

                //};
                user.created_at = DateTime.Now;
                user.updated_at = DateTime.Now;

                // Thêm người dùng mới vào cơ sở dữ liệu
                db.Users.Add(user);
                db.SaveChanges();

                // Đăng nhập người dùng mới sau khi đăng ký thành công
                //Session["UserId"] = newUser.UserId;
                Session["email"] = user.email;
                Session["password"] = user.password;

                // Chuyển hướng đến trang chính sau khi đăng ký thành công
                return RedirectToAction("Login", "Users");
                //}else
                //{
                //    ViewBag.error = "Loi xay ra";
                //    return View(model);
                //}

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        System.Diagnostics.Debug.WriteLine($"{validationError.PropertyName}: {validationError.ErrorMessage}");

                    }
                }
                ViewBag.error = "Có lỗi xác thực. Vui lòng kiểm tra lại thông tin.";
                return View(model);
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi chung
                ViewBag.error = "Có lỗi xảy ra: " + ex.Message;
                return View(model);
            }



        }


        [HttpGet]
        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy email!";
                return View();
            }
            db.SaveChanges();

            // Send reset email

            ViewBag.Message = "Link lấy lại mật khẩu đã được gửi tới email của bạn!";
            return View();
        }

        private void SendResetEmail(string email, string resetToken)
        {
            var resetUrl = Url.Action("ForgotPass", "Users", new { token = resetToken }, protocol: Request.Url.Scheme);
            var fromAddress = new MailAddress("your-email@example.com", "Your App Name");
            var toAddress = new MailAddress(email);
            const string subject = "Password Reset";
            string body = $"Please reset your password by clicking <a href='{resetUrl}'>here</a>.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // Update with your SMTP server
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "your-email-password")
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

    }
    //hehe
}





