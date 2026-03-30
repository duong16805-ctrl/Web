using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using static System.Collections.Specialized.BitVector32;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        QLCFEntities db = new QLCFEntities();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra user trong database
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Lưu session để dùng cho các trang sau
                    Session["User"] = user;
                    Session["UserName"] = user.Username;

                    // Phân quyền dựa trên Role trong DB của bạn
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}