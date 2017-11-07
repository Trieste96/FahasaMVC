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
    public class CT_PhieuGhiNoDaiLyController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        // GET: CT_PhieuGhiNoDaiLy
        public ActionResult Index()
        {
            var cT_PhieuGhiNoDaiLy = db.CT_PhieuGhiNoDaiLy.Include(c => c.PhieuGhiNoDaiLy).Include(c => c.Sach);
            return View(cT_PhieuGhiNoDaiLy.ToList());
        }

        // GET: CT_PhieuGhiNoDaiLy/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuGhiNoDaiLy cT_PhieuGhiNoDaiLy = db.CT_PhieuGhiNoDaiLy.Find(id);
            if (cT_PhieuGhiNoDaiLy == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuGhiNoDaiLy);
        }

        // GET: CT_PhieuGhiNoDaiLy/Create
        public ActionResult Create()
        {
            ViewBag.MaPhieu = new SelectList(db.PhieuGhiNoDaiLies, "MaPhieu", "MaPhieu");
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_PhieuGhiNoDaiLy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DonGiaXuat,SoLuongTon,TienNo,SoLuongDaXuat,SoLuongBanDuoc,MaPhieu,MaSach")] CT_PhieuGhiNoDaiLy cT_PhieuGhiNoDaiLy)
        {
            if (ModelState.IsValid)
            {
                db.CT_PhieuGhiNoDaiLy.Add(cT_PhieuGhiNoDaiLy);
                db.SaveChanges();
                return RedirectToAction("Details","PhieuGhiNoDaiLies",new { id = cT_PhieuGhiNoDaiLy.MaPhieu });
            }

            ViewBag.MaPhieu = new SelectList(db.PhieuGhiNoDaiLies, "MaPhieu", "MaPhieu", cT_PhieuGhiNoDaiLy.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuGhiNoDaiLy.MaSach);
            return View(cT_PhieuGhiNoDaiLy);
        }

        // GET: CT_PhieuGhiNoDaiLy/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuGhiNoDaiLy cT_PhieuGhiNoDaiLy = db.CT_PhieuGhiNoDaiLy.Find(id);
            if (cT_PhieuGhiNoDaiLy == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhieu = new SelectList(db.PhieuGhiNoDaiLies, "MaPhieu", "MaPhieu", cT_PhieuGhiNoDaiLy.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuGhiNoDaiLy.MaSach);
            return View(cT_PhieuGhiNoDaiLy);
        }

        // POST: CT_PhieuGhiNoDaiLy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DonGiaXuat,SoLuongTon,TienNo,SoLuongDaXuat,SoLuongBanDuoc,MaPhieu,MaSach")] CT_PhieuGhiNoDaiLy cT_PhieuGhiNoDaiLy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cT_PhieuGhiNoDaiLy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPhieu = new SelectList(db.PhieuGhiNoDaiLies, "MaPhieu", "MaPhieu", cT_PhieuGhiNoDaiLy.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuGhiNoDaiLy.MaSach);
            return View(cT_PhieuGhiNoDaiLy);
        }

        // GET: CT_PhieuGhiNoDaiLy/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuGhiNoDaiLy cT_PhieuGhiNoDaiLy = db.CT_PhieuGhiNoDaiLy.Find(id);
            if (cT_PhieuGhiNoDaiLy == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuGhiNoDaiLy);
        }

        // POST: CT_PhieuGhiNoDaiLy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_PhieuGhiNoDaiLy cT_PhieuGhiNoDaiLy = db.CT_PhieuGhiNoDaiLy.Find(id);
            db.CT_PhieuGhiNoDaiLy.Remove(cT_PhieuGhiNoDaiLy);
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
