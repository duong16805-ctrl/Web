using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    // ==========================================
    // 1. QUẢN LÝ THỰC ĐƠN (SẢN PHẨM & DANH MỤC)
    // ==========================================
    public class ProductManagerController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        public ActionResult Index() => View(db.Products.Include(p => p.Category).ToList());

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", p.CategoryId);
            return View(p);
        }

        public ActionResult Edit(int id)
        {
            var p = db.Products.Find(id);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", p.CategoryId);
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(p);
        }

        public ActionResult Delete(int id)
        {
            var p = db.Products.Find(id);
            db.Products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    // ==========================================
    // 2. QUẢN LÝ ĐƠN HÀNG
    // ==========================================
    public class OrderManagerController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        public ActionResult Index() => View(db.Orders.OrderByDescending(o => o.OrderDate).ToList());

        public ActionResult Details(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderID == id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, string status)
        {
            var order = db.Orders.Find(id);
            if (order != null)
            {
                order.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var o = db.Orders.Find(id);
            db.Orders.Remove(o);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    // ==========================================
    // 3. QUẢN LÝ BÀN COFFEE
    // ==========================================
    public class TableManagerController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        public ActionResult Index() => View(db.TableCafes.ToList());

        public ActionResult Create() => View();

        [HttpPost]
        public ActionResult Create(TableCafe table)
        {
            db.TableCafes.Add(table);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ToggleStatus(int id)
        {
            var table = db.TableCafes.Find(id);
            if (table != null)
            {
                table.IsOccupied = !table.IsOccupied;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var t = db.TableCafes.Find(id);
            db.TableCafes.Remove(t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    // ==========================================
    // 4. QUẢN LÝ KHO & NHÀ CUNG CẤP
    // ==========================================
    public class WarehouseController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        // Quản lý nguyên liệu tồn kho
        public ActionResult Inventory() => View(db.Inventories.Include(i => i.Supplier).ToList());

        [HttpPost]
        public ActionResult UpdateStock(int id, decimal quantity)
        {
            var item = db.Inventories.Find(id);
            if (item != null)
            {
                item.Quantity += quantity;
                db.SaveChanges();
            }
            return RedirectToAction("Inventory");
        }

        // Quản lý danh sách nhà cung cấp
        public ActionResult Suppliers() => View(db.Suppliers.ToList());

        public ActionResult CreateSupplier() => View();

        [HttpPost]
        public ActionResult CreateSupplier(Supplier s)
        {
            db.Suppliers.Add(s);
            db.SaveChanges();
            return RedirectToAction("Suppliers");
        }
    }

    // ==========================================
    // 5. QUẢN LÝ GIAO DIỆN (MENU ĐỘNG)
    // ==========================================
    public class MenuManagerController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        public ActionResult Index() => View(db.WebMenus.OrderBy(m => m.DisplayOrder).ToList());

        public ActionResult Create() => View();

        [HttpPost]
        public ActionResult Create(WebMenu menu)
        {
            if (ModelState.IsValid)
            {
                db.WebMenus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        public ActionResult Edit(int id) => View(db.WebMenus.Find(id));

        [HttpPost]
        public ActionResult Edit(WebMenu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        public ActionResult Delete(int id)
        {
            var m = db.WebMenus.Find(id);
            if (m != null)
            {
                db.WebMenus.Remove(m);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
