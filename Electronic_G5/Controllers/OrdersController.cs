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
    public class OrdersController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Payment).Include(o => o.Shipment).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
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

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.payment_id = new SelectList(db.Payments, "payment_id", "payment_method");
            ViewBag.shipment_id = new SelectList(db.Shipments, "shipment_id", "address");
            ViewBag.user_id = new SelectList(db.Users, "user_id", "full_name");
            return View();
        }

        // POST: Orders/Create
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

        // GET: Orders/Edit/5
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

        // POST: Orders/Edit/5
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

        // GET: Orders/Delete/5
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

        // POST: Orders/Delete/5
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
        public ActionResult OderHistory()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            int userId = int.Parse(Session["UserID"].ToString());
            var orders = db.Orders.Where(o => o.user_id == userId).ToList();
            return View(orders);
        }

        public ActionResult OrderDetail(int id)
        {
            var detail = db.Orders.Include(o=>o.Order_Items).FirstOrDefault(d=>d.order_id == id);
            return View(detail);
        }
    }
}
