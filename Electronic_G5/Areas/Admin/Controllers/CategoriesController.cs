using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Electronic_G5.Models;
using System.Drawing;

namespace Electronic_G5.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Admin/Categories
        public ActionResult Index(int? page, int? pageSize, string searchString, string sortProperty, string sortOrder = "")
        {
            // Phân trang 
            // Bạn có thể thêm bớt tùy ý --- dammio.com
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });

            items.Add(new SelectListItem { Text = "All", Value = "all" });
            var categories = from category in db.Categories select category;

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
                categories = categories.Where(ct => ct.category_name.Contains(searchString)).OrderByDescending(ct => ct.created_at);
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
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "category_name";
            if (sortOrder == "desc")
            {
                categories = categories.OrderBy(sortProperty + " desc");
            }
            else
            {
                categories = categories.OrderBy(sortProperty);
            }


            if (page == null) page = 1;
            int pageNumber = (page ?? 1);

            return View(categories.ToPagedList(pageNumber, defaultPageSize));
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.parent_id = new SelectList(db.Categories, "category_id", "category_name");
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "category_id,category_name,parent_id,created_at,updated_at")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    category.created_at = DateTime.Now;
                    category.updated_at = DateTime.Now;
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ViewBag.err = ex.Message;
                    return View();
                }

            }

            ViewBag.parent_id = new SelectList(db.Categories, "category_id", "category_name", category.parent_id);
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.parent_id = new SelectList(db.Categories, "category_id", "category_name", category.parent_id);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "category_id,category_name,parent_id,created_at,updated_at")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    category.updated_at = DateTime.Now;
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ViewBag.err = ex.Message ;
                    return View();
                }

            }
            ViewBag.parent_id = new SelectList(db.Categories, "category_id", "category_name", category.parent_id);
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
