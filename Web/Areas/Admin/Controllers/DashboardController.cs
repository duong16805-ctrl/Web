using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models; // Thay 'Web' bằng tên Project của bạn nếu khác

namespace Web.Areas.Admin.Controllers
{
    // Cần chỉ định rõ Controller này thuộc Area "Admin"
    public class DashboardController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            // Lấy dữ liệu cho bảng danh sách, bao gồm cả thông tin Category để hiển thị tên danh mục
            var products = db.Products.Include("Category").OrderByDescending(p => p.Id).ToList();

            // Thống kê cho các thẻ Stats (Web động - dữ liệu thực tế từ DB)
            ViewBag.TotalProducts = db.Products.Count();
            ViewBag.TotalCategories = db.Categories.Count();
            ViewBag.TotalTables = db.TableCafes.Count();
            ViewBag.TotalOrders = db.Orders.Count();

            // Dữ liệu cho DropdownList trong Modal Thêm/Sửa
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            return View(products);
        }

        // Chức năng Thêm mới sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["Success"] = "Đã thêm món '" + product.Name + "' vào thực đơn thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi thêm sản phẩm: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Chức năng Cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var p = db.Products.Find(product.Id);
                    if (p != null)
                    {
                        p.Name = product.Name;
                        p.Price = product.Price;
                        p.CategoryId = product.CategoryId;
                        db.SaveChanges();
                        TempData["Success"] = "Cập nhật thông tin món ăn thành công!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Chức năng Xóa sản phẩm (Sử dụng AJAX để web động, không load lại trang)
        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var p = db.Products.Find(id);
                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Đã xóa sản phẩm thành công." });
                }
                return Json(new { success = false, message = "Không tìm thấy sản phẩm yêu cầu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
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