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
    public class CT_PhieuNhapController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: CT_PhieuNhap
        public ActionResult Index()
        {
            var cT_PhieuNhap = db.CT_PhieuNhap.Include(c => c.PhieuNhap).Include(c => c.Sach);
            return View(cT_PhieuNhap.ToList());
        }

        // GET: CT_PhieuNhap/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuNhap cT_PhieuNhap = db.CT_PhieuNhap.Find(id);
            if (cT_PhieuNhap == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuNhap);
        }

        // GET: CT_PhieuNhap/Create
        public ActionResult Create(int id)
        {
            ViewBag.MaPhieu = id;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_PhieuNhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DonGiaNhap,SoLuong,ThanhTien,MaPhieu,MaSach")] CT_PhieuNhap cT_PhieuNhap)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuNhap tempCTPN = db.CT_PhieuNhap.AsNoTracking().Where(ctpn => ctpn.MaPhieu == cT_PhieuNhap.MaPhieu && ctpn.MaSach == cT_PhieuNhap.MaSach).FirstOrDefault();
                if(tempCTPN != null)
                {
                    db.Entry(tempCTPN).State = EntityState.Modified;
                    tempCTPN.SoLuong += cT_PhieuNhap.SoLuong;
                    tempCTPN.DonGiaNhap = cT_PhieuNhap.DonGiaNhap;
                }
                else
                {
                    db.CT_PhieuNhap.Add(cT_PhieuNhap);
                }
                db.SaveChanges();
                return RedirectToAction("Details", "PhieuNhaps", new { id = cT_PhieuNhap.MaPhieu });
            }

            ViewBag.MaPhieu = new SelectList(db.PhieuNhaps, "MaPhieu", "MaPhieu", cT_PhieuNhap.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuNhap.MaSach);
            return View(cT_PhieuNhap);
        }

        // GET: CT_PhieuNhap/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuNhap cT_PhieuNhap = db.CT_PhieuNhap.Find(id);
            if (cT_PhieuNhap == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhieu = cT_PhieuNhap.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuNhap.MaSach);
            return View(cT_PhieuNhap);
        }

        // POST: CT_PhieuNhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DonGiaNhap,SoLuong,ThanhTien,MaPhieu,MaSach")] CT_PhieuNhap cT_PhieuNhap)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuNhap tempCTPN = db.CT_PhieuNhap.AsNoTracking().Where(ctpn => ctpn.MaPhieu == cT_PhieuNhap.MaPhieu && ctpn.MaSach == cT_PhieuNhap.MaSach).FirstOrDefault();
                if(tempCTPN != null && tempCTPN.ID != cT_PhieuNhap.ID)
                {
                    db.Entry(tempCTPN).State = EntityState.Modified;
                    tempCTPN.SoLuong += cT_PhieuNhap.SoLuong;
                    tempCTPN.DonGiaNhap = cT_PhieuNhap.DonGiaNhap;
                    CT_PhieuNhap deletedCTPN = db.CT_PhieuNhap.Find(cT_PhieuNhap.ID);
                    db.CT_PhieuNhap.Remove(deletedCTPN);
                }
                else
                {
                    db.Entry(cT_PhieuNhap).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Details", "PhieuNhaps", new { id = cT_PhieuNhap.MaPhieu });
            }
            ViewBag.MaPhieu = cT_PhieuNhap.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuNhap.MaSach);
            return View(cT_PhieuNhap);
        }

        // GET: CT_PhieuNhap/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuNhap cT_PhieuNhap = db.CT_PhieuNhap.Find(id);
            if (cT_PhieuNhap == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuNhap);
        }

        // POST: CT_PhieuNhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_PhieuNhap cT_PhieuNhap = db.CT_PhieuNhap.Find(id);
            db.CT_PhieuNhap.Remove(cT_PhieuNhap);
            db.SaveChanges();
            return RedirectToAction("Details", "PhieuNhaps", new { id = cT_PhieuNhap.MaPhieu });
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
