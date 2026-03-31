using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class UIConfigController : Controller
    {
        private QLCFEntities db = new QLCFEntities();

        public ActionResult Index()
        {
            var settings = db.WebSettings.ToDictionary(x => x.SettingKey, x => x.SettingValue);
            return View(settings);
        }

        [HttpPost]
        public ActionResult SaveConfig(FormCollection form)
        {
            foreach (var key in form.AllKeys)
            {
                var value = form[key];
                var setting = db.WebSettings.Find(key);
                if (setting != null)
                {
                    setting.SettingValue = value;
                }
                else
                {
                    db.WebSettings.Add(new WebSetting { SettingKey = key, SettingValue = value });
                }
            }
            db.SaveChanges();
            // Xóa cache để cập nhật mới
            HttpContext.Application["WebSettings"] = null;
            return RedirectToAction("Index");
        }
    }
}