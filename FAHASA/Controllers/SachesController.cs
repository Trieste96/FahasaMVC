using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAHASA.Models;

namespace FAHASA.Controllers
{
    public class SachesController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        // GET: Saches
        public ActionResult Index()
        {
            return View(db.Saches.ToList());
        }

        // GET: Saches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            var ctpn = db.CT_PhieuNhap.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
            var ctpx = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
            var soluongton = ctpn - ctpx;
            if (soluongton == 0)
            {
                ViewBag.SoLuong = "Hết hàng";
            }
            else
                if (soluongton < 0)
            {
                ViewBag.SoLuong = "Thiếu " + soluongton;
            }
            ViewBag.SoLuong = soluongton;
            var giaxuat = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).OrderByDescending(ct => ct.MaPhieu).Select(ct => ct.DonGiaXuat).FirstOrDefault();
            ViewBag.GiaXuat = giaxuat;
            var gianhap = db.CT_PhieuNhap.Where(ct => ct.MaSach == sach.MaSach).OrderByDescending(ct => ct.MaPhieu).Select(ct => ct.DonGiaNhap).FirstOrDefault();
            ViewBag.GiaNhap = gianhap;
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Saches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Saches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSach,TenSach,LinhVuc,GiaXuat,GiaNhap")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Saches.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sach);
        }

        // GET: Saches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            var giaxuat = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).OrderByDescending(ct => ct.MaPhieu).Select(ct => ct.DonGiaXuat).FirstOrDefault();
            ViewBag.GiaXuat = giaxuat;
            var gianhap = db.CT_PhieuNhap.Where(ct => ct.MaSach == sach.MaSach).OrderByDescending(ct => ct.MaPhieu).Select(ct => ct.DonGiaNhap).FirstOrDefault();
            ViewBag.GiaNhap = gianhap;
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Saches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSach,TenSach,LinhVuc")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }

        // GET: Saches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Saches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sach sach = db.Saches.Find(id);
            db.Saches.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
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
