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
    public class PhieuXuatsController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        // GET: PhieuXuats
        public ActionResult Index()
        {
            var phieuXuats = db.PhieuXuats.OrderByDescending(p => p.NgayGio).Include(p => p.DaiLy);
            return View(phieuXuats.ToList());
        }

        // GET: PhieuXuats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuat phieuXuat = db.PhieuXuats.Find(id);
            var tongtien = db.CT_PhieuXuat.Where(ct => ct.MaPhieu == phieuXuat.MaPhieu).Sum(ct => ct.ThanhTien);
            ViewBag.TongTien = tongtien;
            if (phieuXuat == null)
            {
                return HttpNotFound();
            }
            return View(phieuXuat);
        }

        // GET: PhieuXuats/Create
        public ActionResult Create()
        {
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy");
            return View();
        }

        // POST: PhieuXuats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,NgayGio,NguoiNhan,TongTien,MaDaiLy,TinhTrang")] PhieuXuat phieuXuat)
        {
            if (ModelState.IsValid)
            {
                phieuXuat.NgayGio = DateTime.Now;
                phieuXuat.TinhTrang = true;
                db.PhieuXuats.Add(phieuXuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", phieuXuat.MaDaiLy);
            return View(phieuXuat);
        }

        // GET: PhieuXuats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuat phieuXuat = db.PhieuXuats.Find(id);
            if (phieuXuat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", phieuXuat.MaDaiLy);
            return View(phieuXuat);
        }

        // POST: PhieuXuats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieu,NgayGio,NguoiNhan,TongTien,MaDaiLy,TinhTrang")] PhieuXuat phieuXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuXuat).State = EntityState.Modified;
                phieuXuat.TinhTrang = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", phieuXuat.MaDaiLy);
            return View(phieuXuat);
        }

        // GET: PhieuXuats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuat phieuXuat = db.PhieuXuats.Find(id);
            if (phieuXuat == null)
            {
                return HttpNotFound();
            }
            return View(phieuXuat);
        }

        // POST: PhieuXuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuXuat phieuXuat = db.PhieuXuats.Find(id);
            phieuXuat.TinhTrang = false;
            //db.PhieuXuats.Remove(phieuXuat);
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
