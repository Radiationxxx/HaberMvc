
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class YetkiController : Controller
    {
        // GET: Yetki
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["username"] == null)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;

            }
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Hata(string yazilacak) {
            ViewBag.yaz = yazilacak;
            return View();


        }

    }
}