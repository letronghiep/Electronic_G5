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
    public class ProductOptionsController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Admin/ProductOptions
        public ActionResult Index()
        {
            var productOptions = db.ProductOptions.Include(p => p.Product);
            return View(productOptions.ToList());
        }

        // GET: Admin/ProductOptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOption productOption = db.ProductOptions.Find(id);
            if (productOption == null)
            {
                return HttpNotFound();
            }
            return View(productOption);
        }

        // GET: Admin/ProductOptions/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name");
            return View();
        }

        // POST: Admin/ProductOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_option_id,product_id,product_option_name,product_option_value,product_option_price")] ProductOption productOption)
        {
            if (ModelState.IsValid)
            {
                db.ProductOptions.Add(productOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", productOption.product_id);
            return View(productOption);
        }

        // GET: Admin/ProductOptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOption productOption = db.ProductOptions.Find(id);
            if (productOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", productOption.product_id);
            return View(productOption);
        }

        // POST: Admin/ProductOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_option_id,product_id,product_option_name,product_option_value,product_option_price")] ProductOption productOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", productOption.product_id);
            return View(productOption);
        }

        // GET: Admin/ProductOptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOption productOption = db.ProductOptions.Find(id);
            if (productOption == null)
            {
                return HttpNotFound();
            }
            return View(productOption);
        }

        // POST: Admin/ProductOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductOption productOption = db.ProductOptions.Find(id);
            db.ProductOptions.Remove(productOption);
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
