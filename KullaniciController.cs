using HaberSitesi.Helpers;
using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class KullaniciController : YetkiController
    {
        HaberDB db = new HaberDB();
        // GET: Kullanici
        public ActionResult Index()
        {
            var kim = Session["username"].ToString();
            var kisi = db.Kullanicis.Where(ks => ks.KullaniciAd == kim).SingleOrDefault();

            return View(kisi);
            
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int id)
        {
            var kisi = db.Kullanicis.Where(ks => ks.Id == id).SingleOrDefault();

            return View(kisi);
        }
        public ActionResult Profil()
        {
            var kim = Session["username"].ToString();
            var kisi = db.Kullanicis.Where(ks => ks.KullaniciAd == kim).SingleOrDefault();

            return View(kisi);
        }


        // GET: Kullanici/Create
      

        public ActionResult Logout() {
            Session["username"] = null;
            return RedirectToAction("Index","Home");


        }

        // GET: Kullanici/Edit/5
        public ActionResult Edit(int id)
        {

            string kullaniciadi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(k => k.KullaniciAd == kullaniciadi).SingleOrDefault();
            if (Ortaksinif.EditYetkivarmi(id , kullanici))
            { var uye = db.Kullanicis.Where(k => k.Id == id).SingleOrDefault();
                return View(uye);

            }
            
               
             

            return HttpNotFound();


           
        }

        // POST: Kullanici/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Kullanici model)
        {
            try
            {
                var uye = db.Kullanicis.Where(k => k.Id == id).SingleOrDefault();
                uye.Ad = model.Ad;
                uye.Soyad = model.Soyad;
                uye.Sifre = model.Sifre;
                db.SaveChanges();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: Kullanici/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kullanici/Delete/5
        [HttpPost]
        public ActionResult Delete(int id,Kullanici model)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
