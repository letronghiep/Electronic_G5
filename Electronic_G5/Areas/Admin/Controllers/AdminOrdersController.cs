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
    public class AdminOrdersController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Admin/Orders
        public ActionResult Index(int? page, int? pageSize, string searchString, string sortProperty, string sortOrder = "")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });

            items.Add(new SelectListItem { Text = "All", Value = "all" });
            var orders = db.Orders.Include(o => o.Payment).Include(o => o.Shipment).Include(u => u.User);

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
                orders = orders.Where(ct => ct.User.full_name.Contains(searchString)).OrderByDescending(ct => ct.created_at);
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
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "User.full_name";
            if (sortOrder == "desc")
            {
                orders = orders.OrderBy(sortProperty + " desc");
            }
            else
            {
                orders = orders.OrderBy(sortProperty);
            }


            if (page == null) page = 1;
            int pageNumber = (page ?? 1);

            return View(orders.ToPagedList(pageNumber, defaultPageSize));
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.payment_id = new SelectList(db.Payments, "payment_id", "payment_method");
            ViewBag.shipment_id = new SelectList(db.Shipments, "shipment_id", "address");
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "order_id,order_date,total_price,user_id,order_status,fulfillment_status,payment_id,shipment_id,created_at,updated_at")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.payment_id = new SelectList(db.Payments, "payment_id", "payment_method", order.payment_id);
            ViewBag.shipment_id = new SelectList(db.Shipments, "shipment_id", "address", order.shipment_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name", order.user_id);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.payment_id = new SelectList(db.Payments, "payment_id", "payment_method", order.payment_id);
            ViewBag.shipment_id = new SelectList(db.Shipments, "shipment_id", "address", order.shipment_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name", order.user_id);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "order_id,order_date,total_price,user_id,order_status,fulfillment_status,payment_id,shipment_id,created_at,updated_at")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.payment_id = new SelectList(db.Payments, "payment_id", "payment_method", order.payment_id);
            ViewBag.shipment_id = new SelectList(db.Shipments, "shipment_id", "address", order.shipment_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name", order.user_id);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
    }
}
