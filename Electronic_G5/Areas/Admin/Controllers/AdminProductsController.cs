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
    public class AdminProductsController : Controller
    {
        private ElectronicDb db = new ElectronicDb();

        // GET: Admin/Products
        public ActionResult Index(int? page, int? pageSize, string searchString, string sortProperty, string sortOrder = "")
        {
            //return View(products.ToList());

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });

            items.Add(new SelectListItem { Text = "All", Value = "all" });
            var products = db.Products.Include(p => p.Category);

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
                products = products.Where(ct => ct.product_name.Contains(searchString)).OrderByDescending(ct => ct.created_at);
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
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "product_name";
            if (sortOrder == "desc")
            {
                products = products.OrderBy(sortProperty + " desc");
            }
            else
            {
                products = products.OrderBy(sortProperty);
            }


            if (page == null) page = 1;
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, defaultPageSize));
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            var viewModel = new ProductViewModel
            {
                Product = new Product(),
                ProductOptions = new List<ProductOption>(),
                Images = new List<Image>()
            };
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine(viewModel.Product.category_id);
                    var existingCategory = db.Categories.FirstOrDefault(c => c.category_id == viewModel.Product.category_id);
                    if (existingCategory == null)
                    {
                        ModelState.AddModelError("Product.category_id", "Mã danh mục không hợp lệ. Vui lòng chọn một danh mục đã tồn tại.");
                        ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                        return View(viewModel);
                    }
                    viewModel.Product.image_url = "";

                    var imageInput = Request.Files["imageInput"];
                    System.Diagnostics.Debug.WriteLine(imageInput);

                    // Kiểm tra xem có ảnh được tải lên hay không
                    if (imageInput != null && imageInput.ContentLength > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(imageInput);

                        // Lấy tên tệp và đường dẫn để lưu trữ
                        string filename = Path.GetFileName(imageInput.FileName);
                        string path = Path.Combine(Server.MapPath("~/wwwroot/Images/"), filename);
                        imageInput.SaveAs(path);
                        // Cập nhật thuộc tính image_url của sản phẩm
                        viewModel.Product.image_url = filename;
                    }

                    // Cập nhật thời gian tạo và cập nhật của sản phẩm
                    viewModel.Product.created_at = DateTime.Now;
                    viewModel.Product.updated_at = DateTime.Now;

                    // Thêm sản phẩm vào cơ sở dữ liệu
                    db.Products.Add(viewModel.Product);

                    // Lưu các lựa chọn sản phẩm
                    foreach (var option in viewModel.ProductOptions)
                    {
                        option.product_id = viewModel.Product.product_id;
                        db.ProductOptions.Add(option);
                    }
                    System.Diagnostics.Debug.WriteLine(viewModel.Product.image_url);

                    // Lưu các hình ảnh khác của sản phẩm
                    foreach (var image in viewModel.Images)
                    {
                        image.product_id = viewModel.Product.product_id;
                        db.Images.Add(image);
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                    return View(viewModel);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Validation Error: {validationError.PropertyName} - {validationError.ErrorMessage}");
                    }
                }
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                return View(viewModel);
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi
                // Kiểm tra log lỗi để tìm hiểu thêm thông tin
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Update Exception Error: {ex.InnerException.InnerException.Message}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Update Exception Error: {ex.Message}");
                }

                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                return View(viewModel);
            }
        }



        //partial
        [HttpPost]
        public ActionResult AddOptionPartial(ProductOption option)
        {
            try
            {
                var options = (List<ProductOption>)Session["ProductOptions"] ?? new List<ProductOption>();
                options.Add(option);
                Session["ProductOptions"] = options;

                var html = this.RenderPartialViewToString("_ProductOptionList", options);
                return Json(new { success = true, html });
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult RemoveOptionPartial(int id)
        {
            try
            {
                var options = (List<ProductOption>)Session["ProductOptions"];
                var option = options.FirstOrDefault(o => o.product_option_id == id);
                if (option != null)
                {
                    options.Remove(option);
                    Session["ProductOptions"] = options;
                    var html = this.RenderPartialViewToString("_ProductOptionList", options);
                    return Json(new { success = true, html });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }

        }

        // POST: Products/UploadImage
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = System.IO.Path.GetFileName(file.FileName);
                    var path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/Images/"), fileName);
                    file.SaveAs(path);
                    var image = new Image { image_url = "/wwwroot/Images/" + fileName };
                    // Add image to session or database as needed
                    var images = (List<Image>)Session["Images"] ?? new List<Image>();
                    images.Add(image);
                    Session["Images"] = images;

                    var html = this.RenderPartialViewToString("_ImageList", images);
                    return Json(new { success = true, image, html });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Products/RemoveImage
        [HttpPost]
        public ActionResult RemoveImage(int id)
        {
            try
            {
                var images = (List<Image>)Session["Images"];
                var image = images.FirstOrDefault(i => i.image_id == id);
                if (image != null)
                {
                    images.Remove(image);
                    Session["Images"] = images;
                    var html = this.RenderPartialViewToString("_ImageList", images);
                    return Json(new { success = true, html });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }

        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
        //end 

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Include(p => p.ProductOptions)
                                     .Include(p => p.Images)
                                     .FirstOrDefault(p => p.product_id == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ProductViewModel
            {
                Product = product,
                ProductOptions = product.ProductOptions.ToList(),
                Images = product.Images.ToList()
            };

            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.category_id);
            return View(viewModel);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = db.Categories.FirstOrDefault(c => c.category_id == viewModel.Product.category_id);
                    if (existingCategory == null)
                    {
                        ModelState.AddModelError("Product.category_id", "Mã danh mục không hợp lệ. Vui lòng chọn một danh mục đã tồn tại.");
                        ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                        return View(viewModel);
                    }

                    var imageInput = Request.Files["imageInput"];
                    if (imageInput != null && imageInput.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(imageInput.FileName);
                        string path = Path.Combine(Server.MapPath("~/wwwroot/Images/"), filename);
                        imageInput.SaveAs(path);
                        viewModel.Product.image_url = filename;
                    }
                    viewModel.Product.updated_at = DateTime.Now;

                    System.Diagnostics.Debug.WriteLine("Product not ", viewModel.Product.image_url);
                    var productToUpdate = db.Products.Include(p => p.ProductOptions)
                                                     .Include(p => p.Images)
                                                     .FirstOrDefault(p => p.product_id == viewModel.Product.product_id);

                    if (productToUpdate == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Product not found", productToUpdate);
                    }

                    db.Entry(productToUpdate).CurrentValues.SetValues(viewModel.Product);

                    db.ProductOptions.RemoveRange(productToUpdate.ProductOptions);
                    foreach (var option in viewModel.ProductOptions)
                    {
                        option.product_id = viewModel.Product.product_id;
                        db.ProductOptions.Add(option);
                    }

                    db.Images.RemoveRange(productToUpdate.Images);
                    foreach (var image in viewModel.Images)
                    {
                        image.product_id = viewModel.Product.product_id;
                        db.Images.Add(image);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminProducts");
                }
                else
                {
                    ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                    return View(viewModel);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi xác thực: {validationError.PropertyName} - {validationError.ErrorMessage}");
                    }
                }
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                return View(viewModel);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật: {ex.InnerException.InnerException.Message}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật: {ex.Message}");
                }
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi: " + ex.StackTrace);
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
                return View(viewModel);
            }
        }



        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var productOptions = db.ProductOptions.Where(pr => pr.product_id == id);
                if (productOptions != null)
                {
                    db.ProductOptions.RemoveRange(productOptions);
                }
                var imageList = db.Images.Where(image => image.product_id == id);
                if (imageList != null)
                {
                    db.Images.RemoveRange(imageList);
                }
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ViewBag.error = "Bạn chưa thể xóa trường này " + ex.Message;
                return View();
            }

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
