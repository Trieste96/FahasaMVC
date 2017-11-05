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
    public class CT_PhieuYeuCauBanController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: CT_PhieuYeuCauBan
        public ActionResult Index()
        {
            var cT_PhieuYeuCauBan = db.CT_PhieuYeuCauBan.Include(c => c.PhieuYeuCauBan).Include(c => c.Sach);
            return View(cT_PhieuYeuCauBan.ToList());
        }

        // GET: CT_PhieuYeuCauBan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuYeuCauBan cT_PhieuYeuCauBan = db.CT_PhieuYeuCauBan.Find(id);
            if (cT_PhieuYeuCauBan == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuYeuCauBan);
        }

        // GET: CT_PhieuYeuCauBan/Create
        public ActionResult Create(int id)
        {
            ViewBag.MaPhieu = id;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_PhieuYeuCauBan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SoLuong,MaPhieu,MaSach")] CT_PhieuYeuCauBan cT_PhieuYeuCauBan)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuYeuCauBan tempCTPYCB = db.CT_PhieuYeuCauBan.AsNoTracking().Where(ctpycb => ctpycb.MaPhieu == cT_PhieuYeuCauBan.MaPhieu && ctpycb.MaSach == cT_PhieuYeuCauBan.MaSach).FirstOrDefault();
                if (tempCTPYCB != null)
                {
                    tempCTPYCB.SoLuong += cT_PhieuYeuCauBan.SoLuong;
                    db.Entry(tempCTPYCB).State = EntityState.Modified;
                }
                else
                {
                    db.CT_PhieuYeuCauBan.Add(cT_PhieuYeuCauBan);
                }
                db.SaveChanges();
                return RedirectToAction("Details","PhieuYeuCauBans",new { id = cT_PhieuYeuCauBan.MaPhieu });
            }

            ViewBag.MaPhieu = cT_PhieuYeuCauBan.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauBan.MaSach);
            return View(cT_PhieuYeuCauBan);
        }

        // GET: CT_PhieuYeuCauBan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuYeuCauBan cT_PhieuYeuCauBan = db.CT_PhieuYeuCauBan.Find(id);
            if (cT_PhieuYeuCauBan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhieu = cT_PhieuYeuCauBan.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauBan.MaSach);
            return View(cT_PhieuYeuCauBan);
        }

        // POST: CT_PhieuYeuCauBan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SoLuong,MaPhieu,MaSach")] CT_PhieuYeuCauBan cT_PhieuYeuCauBan)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuYeuCauBan tempCTPYCB = db.CT_PhieuYeuCauBan.AsNoTracking().Where(ctpycb => ctpycb.MaPhieu == cT_PhieuYeuCauBan.MaPhieu && ctpycb.MaSach == cT_PhieuYeuCauBan.MaSach).FirstOrDefault();
                if(tempCTPYCB != null || tempCTPYCB.ID != cT_PhieuYeuCauBan.ID)
                {
                    db.Entry(tempCTPYCB).State = EntityState.Modified;
                    tempCTPYCB.SoLuong += cT_PhieuYeuCauBan.SoLuong;
                    CT_PhieuYeuCauBan deletedCTPYCB = db.CT_PhieuYeuCauBan.Find(cT_PhieuYeuCauBan.ID);
                    db.CT_PhieuYeuCauBan.Remove(deletedCTPYCB);
                }
                else
                {
                    db.Entry(cT_PhieuYeuCauBan).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Details", "PhieuYeuCauBans", new { id = cT_PhieuYeuCauBan.MaPhieu});
            }
            ViewBag.MaPhieu = cT_PhieuYeuCauBan.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauBan.MaSach);
            return View(cT_PhieuYeuCauBan);
        }

        // GET: CT_PhieuYeuCauBan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuYeuCauBan cT_PhieuYeuCauBan = db.CT_PhieuYeuCauBan.Find(id);
            if (cT_PhieuYeuCauBan == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuYeuCauBan);
        }

        // POST: CT_PhieuYeuCauBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_PhieuYeuCauBan cT_PhieuYeuCauBan = db.CT_PhieuYeuCauBan.Find(id);
            db.CT_PhieuYeuCauBan.Remove(cT_PhieuYeuCauBan);
            db.SaveChanges();
            return RedirectToAction("Details", "PhieuYeuCauBans", new { id = cT_PhieuYeuCauBan.MaPhieu });
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
