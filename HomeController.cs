using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class HomeController : Controller
    {
        HaberDB db = new HaberDB();
        // GET: Home
        public ActionResult Index(string AramaYap = null, int KategoriId = 0)
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "Id", "KategoriAdi");
            var haberler = from a in db.Habers
                           select a;
            var sonhaberler = haberler.OrderByDescending(i => i.Tarih).Take(20).ToList();               
                            
            if (KategoriId != 0)
            {
                haberler = haberler.Where(i => i.KategoriId == KategoriId);
                return View(haberler);
            }
            if (!string.IsNullOrEmpty(AramaYap))
            {
                haberler = haberler.Where(i => i.HaberBaslik.Contains(AramaYap));
                return View(haberler);
            }
            else if (KategoriId == 0 && AramaYap == null)
            {
                return View(sonhaberler);
            }
            return View(sonhaberler);
        }

        public ActionResult Create()
        {

            return View();
        }

        // POST: Kullanici/Create
        [HttpPost]
        public ActionResult Create(Kullanici model)
        {
            try
            {
                var deger = db.Kullanicis.Where(m => m.KullaniciAd == model.KullaniciAd).SingleOrDefault();
                if (deger != null)
                {
                    return View();

                }

                if (model.Sifre == null)
                {
                    return View();

                }
                model.Tarih = DateTime.Now;
                model.YetkiId = 1;
                db.Kullanicis.Add(model);
                db.SaveChanges();
                Session["username"] = model.KullaniciAd;


                return RedirectToAction("Index", "Kullanici");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(Kullanici model)
        {
            try
            {
                var kullanici = db.Kullanicis.Where(ks => ks.KullaniciAd == model.KullaniciAd).SingleOrDefault();
                if (kullanici == null)
                {
                    return View();
                }
                if (kullanici.Sifre == model.Sifre)
                {
                    Session["username"] = kullanici.KullaniciAd;
                    return RedirectToAction("Index", "Kullanici");
                }
                else
                {

                    return View();
                }
            }
            catch
            {
                return View();
            }



        }


        public ActionResult Details(int id)
        {
            var haberdetay = db.Habers.Where(d => d.Id == id).SingleOrDefault();
            return View(haberdetay);
        }
    }
}