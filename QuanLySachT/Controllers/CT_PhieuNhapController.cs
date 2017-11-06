using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLySachT;

namespace QuanLySachT.Controllers
{
    public class CT_PhieuNhapController : Controller
    {
        private demoQLSTEntities db = new demoQLSTEntities();

        // GET: CT_PhieuNhap
        public ActionResult Index()
        {
            var cT_PhieuNhap = db.CT_PhieuNhap.OrderByDescending(c => c.MaPhieu).Include(c => c.PhieuNhap).Include(c => c.Sach);
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
        public ActionResult Create()
        {
            ViewBag.MaPhieu = new SelectList(db.PhieuNhaps.Where(pn => pn.TinhTrang == true), "MaPhieu", "MaPhieu");
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
                cT_PhieuNhap.ThanhTien = cT_PhieuNhap.SoLuong * cT_PhieuNhap.DonGiaNhap;
                db.CT_PhieuNhap.Add(cT_PhieuNhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaPhieu = new SelectList(db.PhieuNhaps, "MaPhieu", "NguoiGiaoSach", cT_PhieuNhap.MaPhieu);
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
            ViewBag.MaPhieu = new SelectList(db.PhieuNhaps, "MaPhieu", "NguoiGiaoSach", cT_PhieuNhap.MaPhieu);
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
                db.Entry(cT_PhieuNhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPhieu = new SelectList(db.PhieuNhaps, "MaPhieu", "NguoiGiaoSach", cT_PhieuNhap.MaPhieu);
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
        public JsonResult doesProductNameExistUnderCategory(int? MaPhieu, int? MaSach)
        {

            var model = db.CT_PhieuNhap.Where(x => (MaPhieu.HasValue) ?
                    (x.MaPhieu == MaPhieu && x.MaSach == MaSach) :
                    (x.MaSach == MaSach)
                );

            return Json(model.Count() == 0);

        }
    }
}
