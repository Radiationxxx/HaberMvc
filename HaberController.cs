using HaberSitesi.Helpers;
using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class HaberController : Controller
    {
        HaberDB db = new HaberDB();
        // GET: Haber
        public ActionResult Index(string AramaYap = null, int KategoriId = 0)
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "Id", "KategoriAdi");
            var haberler = from a in db.Habers
                           select a;
            if (KategoriId != 0)
            {
                haberler = haberler.Where(i => i.KategoriId == KategoriId);
            }
            if (!string.IsNullOrEmpty(AramaYap))
            {
                haberler = haberler.Where(i => i.HaberBaslik.Contains(AramaYap));
            }

            return View(haberler);
        }

        // GET: Haber/Details/5
        public ActionResult Details(int id)
        {
            var haberdetay = db.Habers.Where(d => d.Id == id).SingleOrDefault();
            return View(haberdetay);
        }

        public JsonResult YorumYap(string Yorum, int HaberID)
        {

            var YorumKisi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(i => i.KullaniciAd == YorumKisi).SingleOrDefault();
            if (Yorum == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorums.Add(new Yorum
            {
                KullaniciId = kullanici.Id,
                HaberId = HaberID,
                YorumTarih = DateTime.Now,
                YorumIcerik = Yorum




            });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);


        }

        // GET: Haber/Create
        public ActionResult Create()
        {
            var kullaniciadi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(i => i.KullaniciAd == kullaniciadi).SingleOrDefault().YetkiId;

            ViewBag.KategoriId = new SelectList(db.Kategoris, "Id", "KategoriAdi");
            if (kullanici >= 2)
            {
                return View();
            }
            else
                return HttpNotFound();
        }

        public ActionResult HaberlerimiListele()
        {
            var kullaniciadi = Session["username"].ToString();
            var kullanicihaber = db.Kullanicis.Where(i => i.KullaniciAd == kullaniciadi).SingleOrDefault().Habers.ToList();
            return View(kullanicihaber);

        }

        // POST: Haber/Create
        [HttpPost]
        public ActionResult Create(Haber model, string Etiket)
        {
            try
            {
                var kullaniciadi = Session["username"].ToString();
                var olusturankisi = db.Kullanicis.Where(k => k.KullaniciAd == kullaniciadi).SingleOrDefault();
                model.OlusturanId = olusturankisi.Id;
                model.Tarih = DateTime.Now;
                db.Habers.Add(model);
                if (!string.IsNullOrEmpty(Etiket))
                {
                    string[] etiketler = Etiket.Split(',');
                    foreach (string e in etiketler)

                    {
                        var yenietiket = new Etiket { EtiketAd = e };

                        db.Etikets.Add(yenietiket);
                        model.Etikets.Add(yenietiket);
                    }

                }

                db.SaveChanges();
                // TODO: Add insert logic here

                return RedirectToAction("Index", "Kullanici");
            }
            catch
            {
                return View();
            }
        }

        // GET: Haber/Edit/5
        public ActionResult Edit(int id)
        {
            var kullaniciadi = Session["username"].ToString();
            var haber = db.Habers.Where(i => i.Id == id).SingleOrDefault();
            if (haber == null)
            {
                return HttpNotFound();

            }
            if (haber.Kullanici.KullaniciAd == kullaniciadi)
            {
                ViewBag.KategoriId = new SelectList(db.Kategoris, "Id", "KategoriAdi");
                return View();
            }
            else
                return HttpNotFound();
        }

        // POST: Haber/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Haber model)
        {

            try
            {
                var haber = db.Habers.Where(i => i.Id == id).SingleOrDefault();
                haber.HaberIcerik = model.HaberIcerik;
                haber.HaberBaslik = model.HaberBaslik;
                haber.KategoriId = model.KategoriId;
                haber.Etikets = model.Etikets;
                db.SaveChanges();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAd == kullaniciadi).SingleOrDefault();

                var haber = db.Habers.Where(i => i.Id == id).SingleOrDefault();
                if (kullanici.Id == haber.OlusturanId)
                {
                    foreach (var i in haber.Yorums)
                    {
                        db.Yorums.Remove(i);
                    }
                    foreach (var i in haber.Etikets)
                    {
                        db.Etikets.Remove(i);
                    }
                    db.Habers.Remove(haber);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Hata", " Yetki", new { yazilacak = "Haber silinemedi!" });
            }
        }

        public ActionResult YorumDelete(int id)
        {
            try
            {


                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAd == kullaniciadi).SingleOrDefault();
                var yorum = db.Yorums.Where(i => i.Id == id).SingleOrDefault();
                var haber = db.Habers.Where(i => i.Id == yorum.HaberId).SingleOrDefault();
                if (yorum == null)
                {
                    return RedirectToAction("Hata", " Yetki", new { yazilacak = "Yorum bulunamadı!" });

                }

                if (Ortaksinif.DeleteYetkivarmi(id, kullanici) || haber.OlusturanId == kullanici.Id || yorum.Kullanici.KullaniciAd == kullaniciadi)
                {
                    db.Yorums.Remove(yorum);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Haber", new { id = yorum.HaberId });

                }
                return RedirectToAction("Hata", " Yetki", new { yazilacak = "Yorum silinemedi!" });
            }
            catch
            {
                return RedirectToAction("Hata", " Yetki", new { yazilacak = "Yorum silinemedi!" });
            }


        }
        public ActionResult YorumEdit(int id)
        {
            return View();

        }
        [HttpPost]
        public ActionResult YorumEdit(int id, Yorum model)
        {
            var yorum = db.Yorums.Where(i => i.Id == id).SingleOrDefault();
            yorum.YorumIcerik = model.YorumIcerik;
            yorum.YorumTarih = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}