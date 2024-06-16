using Electronic_G5.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using OfficeOpenXml;
using System.IO;
using System.IO.Packaging;
using OfficeOpenXml.Style;


namespace Electronic_G5.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {

        private ElectronicDb db = new ElectronicDb();
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            //kiểm tra xem đăng nhập chưa
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("Login", "AdminUsers", new { area = "Admin" });
            //}
            //Lấy số sản phẩm đang trong qua trình xử lý
            var pendingProductCount = (from order in db.Orders
                                       where order.order_status == "Đang xử lý"
                                       select order).ToList().Count();
            var outStock = (from product in db.Products
                            where product.stock == 0
                            select product).ToList().Count();
            var totalPrice = (from order in db.Orders 
                              where order.order_date == DateTime.Now 
                              select order).ToList().Sum(o => o.total_price);
            ViewBag.TotalPrice = totalPrice;
            // Lấy tháng và năm hiện tại
            var currentDate = DateTime.Now;
            int month = currentDate.Month;
            int year = currentDate.Year;



            // select top 5 sản phẩm bán chạy nhất

            var topSalesProduct = db.Order_Items.GroupBy(o => o.product_id).Select(g => new
            {
                product_id = g.Key,
                order_quantity = g.Sum(oi => oi.quantity),
                product_name = g.FirstOrDefault().Product.product_name,
                image_url = g.FirstOrDefault().Product.image_url,
                revenue = g.Sum(oi => oi.price),
            }).OrderByDescending(g => g.order_quantity).Take(5).ToList();

            // Lấy top 5 sản phẩm không bán được
            var bottomProducts = db.Products
                .GroupJoin(
                    db.Order_Items,
                    p => p.product_id,
                    oi => oi.product_id,
                    (p, ois) => new
                    {
                        product_id = p.product_id,
                        product_name = p.product_name,
                        image_url = p.image_url,
                        price = p.price,
                        stock = p.stock,
                        created_at = p.created_at,
                        order_quantity = ois.Sum(oi => (int?)oi.quantity) ?? 0 // Sử dụng null-coalescing operator để xử lý sản phẩm chưa có đơn hàng
                    }
                ).Where(p => p.order_quantity == 0)
                .OrderBy(p => p.order_quantity)
                .Take(5)
                .ToList();
            // Gọi action MonthlyRevenue để lấy dữ liệu doanh thu của tháng hiện tại
            MonthlyRevenue(month, year);
            ViewBag.currentMonth = month;
            ViewBag.currentYear = year;
            // Truyền dữ liệu sản phẩm bán chạy nhất qua ViewBag
            ViewBag.TopSalesProduct = JsonConvert.SerializeObject(topSalesProduct);
            ViewBag.BottomSalesProduct = JsonConvert.SerializeObject(bottomProducts);
            ViewBag.productOrderCount = pendingProductCount;
            ViewBag.outStock = outStock;

            return View("Index");
        }
        [HttpGet]
        public ActionResult MonthlyRevenue(int? month, int? year)
        {
            if (!month.HasValue || !year.HasValue)
            {
                // Nếu không có tham số nào được truyền vào, lấy tháng và năm hiện tại
                var currentDate = DateTime.Now;
                month = currentDate.Month;
                year = currentDate.Year;
            }
            var monthlyStats = db.Orders
                .Where(o => o.order_date.Year == year && o.order_date.Month == month)
                .GroupBy(o => o.order_date.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    Revenue = g.Sum(o => o.total_price)
                })
                .ToList();
          
            ViewBag.MonthlyStats = JsonConvert.SerializeObject(monthlyStats);
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View("Index");
        }
        public ActionResult ExportToExcel(int? month, int? year)
        {
            try
            {

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {

                    // Lấy tháng và năm hiện tại
                    if (!month.HasValue || !year.HasValue)
                    {
                        // Nếu không có tham số nào được truyền vào, lấy tháng và năm hiện tại
                        var currentDate = DateTime.Now;
                        month = currentDate.Month;
                        year = currentDate.Year;
                    }



                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A3"].Style.Font.Name = "Time New Roman";
                    worksheet.Cells["D3:R3"].Merge = true;
                    worksheet.Cells["D3"].Value = $"BÁO CÁO DOANH THU THÁNG {month}, {year}";
                    worksheet.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["D3"].Style.Font.Size = 18;
                    worksheet.Cells["D3"].Style.Font.Bold = true;

                    worksheet.Cells["B5:C5"].Merge = true;
                    worksheet.Cells["B5"].Value = "Ngày đặt hàng";
                    worksheet.Cells["B5"].Style.Font.Bold = true;
                    worksheet.Cells["D5:G5"].Merge = true;
                    worksheet.Cells["D5"].Value = "Tên sản phẩm";
                    worksheet.Cells["D5"].Style.Font.Bold = true;
                    worksheet.Cells["H5"].Value = "Số lượng";
                    worksheet.Cells["H5"].Style.Font.Bold = true;
                    worksheet.Cells["I5"].Value = "Giá";
                    worksheet.Cells["I5"].Style.Font.Bold = true;
                    worksheet.Cells["J5:K5"].Merge = true;
                    worksheet.Cells["J5"].Value = "Trạng thái";
                    worksheet.Cells["J5"].Style.Font.Bold = true;
                    worksheet.Cells[$"B5:J5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"B5:J5"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                    worksheet.Cells[$"B5:J5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    var vietnamCulture = new System.Globalization.CultureInfo("vi-VN");
                    var statistical = (from order in db.Orders
                                       join oi in db.Order_Items
                                       on order.order_id equals oi.order_id
                                       join product in db.Products
                                       on oi.product_id equals product.product_id
                                       select new
                                       {
                                           order_date = order.order_date,
                                           product_name = product.product_name,
                                           quantity = oi.quantity,
                                           price = oi.price,
                                           order_status = order.order_status
                                       }).ToList();



                    int startRow = 6; // Hàng bắt đầu
                    int endRow = startRow + statistical.Count - 1; // Số hàng cần merge

                    int row = startRow;
                    for (int i = 0; i < statistical.Count; i++)
                    {
                        worksheet.Cells[row, 2, row, 3].Merge = true;
                        worksheet.Cells[row, 2].Value = statistical[i].order_date;
                        worksheet.Cells[row, 4, row, 7].Merge = true;

                        worksheet.Cells[row, 4].Value = statistical[i].product_name;
                        worksheet.Cells[row, 8].Value = statistical[i].quantity;
                        worksheet.Cells[row, 9].Value = statistical[i].price.ToString("N0", vietnamCulture) + " đ";
                        worksheet.Cells[row, 10, row, 11].Merge = true;

                        worksheet.Cells[row, 10].Value = statistical[i].order_status;
                        worksheet.Cells[row, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        row++;
                    }

                    worksheet.Cells[row, 10].Value = "Tổng tiền";

                    worksheet.Cells[row, 11].Value = statistical.Sum(e => e.price).ToString("N0", vietnamCulture) + " đ";

                    worksheet.Cells[$"A3:J11"].AutoFitColumns();
                    worksheet.Cells[$"A4:J4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                    worksheet.Cells[$"A4:J11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                    string output = $"Thống kê tháng {month}, {year}";

                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{output}.xlsx");
                }
            }
            catch (FileNotFoundException ex)
            {
                // Log detailed error
                System.Diagnostics.Debug.WriteLine("FileNotFounds" + ex.Message);

                return View("Index");
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine("aa" + ex.StackTrace);
                return View("Index");
            }
        }

    }
}