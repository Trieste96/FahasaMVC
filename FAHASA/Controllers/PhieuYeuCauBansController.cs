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
    public class PhieuYeuCauBansController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: PhieuYeuCauBans
        public ActionResult Index()
        {
            var phieuYeuCauBans = db.PhieuYeuCauBans.Where(pycb => pycb.TinhTrang == true).Include(p => p.NhaXuatBan);
            return View(phieuYeuCauBans.ToList());
        }

        // GET: PhieuYeuCauBans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuYeuCauBan phieuYeuCauBan = db.PhieuYeuCauBans.Find(id);
            if (phieuYeuCauBan == null || !phieuYeuCauBan.TinhTrang)
            {
                return HttpNotFound();
            }
            phieuYeuCauBan.NhaXuatBan = db.NhaXuatBans.Find(phieuYeuCauBan.MaNXB);
            db.Entry<PhieuYeuCauBan>(phieuYeuCauBan).Collection(pycb => pycb.CTPhieuYeuCauBans).Load();
            foreach(var item in phieuYeuCauBan.CTPhieuYeuCauBans)
            {
                item.Sach = db.Saches.Find(item.MaSach);
            }
            return View(phieuYeuCauBan);
        }

        // GET: PhieuYeuCauBans/Create
        public ActionResult Create()
        {
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            return View();
        }

        // POST: PhieuYeuCauBans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,NgayGio,MaNXB,TinhTrang")] PhieuYeuCauBan phieuYeuCauBan)
        {
            if (ModelState.IsValid)
            {
                db.PhieuYeuCauBans.Add(phieuYeuCauBan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", phieuYeuCauBan.MaNXB);
            return View(phieuYeuCauBan);
        }

        // GET: PhieuYeuCauBans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuYeuCauBan phieuYeuCauBan = db.PhieuYeuCauBans.Find(id);
            if (phieuYeuCauBan == null || !phieuYeuCauBan.TinhTrang)
            {
                return HttpNotFound();
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", phieuYeuCauBan.MaNXB);
            return View(phieuYeuCauBan);
        }

        // POST: PhieuYeuCauBans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieu,NgayGio,MaNXB,TinhTrang")] PhieuYeuCauBan phieuYeuCauBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuYeuCauBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", phieuYeuCauBan.MaNXB);
            return View(phieuYeuCauBan);
        }

        // GET: PhieuYeuCauBans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuYeuCauBan phieuYeuCauBan = db.PhieuYeuCauBans.Find(id);
            if (phieuYeuCauBan == null)
            {
                return HttpNotFound();
            }
            return View(phieuYeuCauBan);
        }

        // POST: PhieuYeuCauBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuYeuCauBan phieuYeuCauBan = db.PhieuYeuCauBans.Find(id);
            if (ModelState.IsValid)
            {
                phieuYeuCauBan.TinhTrang = false;
                db.Entry(phieuYeuCauBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
