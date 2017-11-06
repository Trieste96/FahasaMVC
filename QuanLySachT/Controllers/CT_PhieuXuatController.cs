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
    public class CT_PhieuXuatController : Controller
    {
        private demoQLSTEntities db = new demoQLSTEntities();
        /*private void tinhlaitongtien(CT_PhieuXuat cT_PhieuXuat)
        {
            
            var total = db.CT_PhieuXuat.Where(ct => ct.MaSach == cT_PhieuXuat.MaSach).Sum(ct => ct.ThanhTien).GetValueOrDefault();
            db.Entry(cT_PhieuXuat).State = EntityState.Modified;
            db.SaveChanges();
        }*/

        // GET: CT_PhieuXuat
        public ActionResult Index()
        {
            var cT_PhieuXuat = db.CT_PhieuXuat.OrderByDescending(c => c.MaPhieu).Include(c => c.PhieuXuat).Include(c => c.Sach);
            return View(cT_PhieuXuat.ToList());
        }

        // GET: CT_PhieuXuat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuXuat cT_PhieuXuat = db.CT_PhieuXuat.Find(id);
            if (cT_PhieuXuat == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuXuat);
        }

        // GET: CT_PhieuXuat/Create
        public ActionResult Create()
        {
            ViewBag.MaPhieu = new SelectList(db.PhieuXuats.Where(px => px.TinhTrang == true), "MaPhieu", "MaPhieu");
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_PhieuXuat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DonGiaXuat,ThanhTien,SoLuong,MaPhieu,MaSach")] CT_PhieuXuat cT_PhieuXuat)
        {
            if (ModelState.IsValid)
            {
                Sach sach = db.Saches.Find(cT_PhieuXuat.MaSach);
                var existctpx = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).Select(ct => ct.MaSach).Any();
                if (existctpx == null)
                {
                    var ctpn = db.CT_PhieuNhap.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
                    var ctpx = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
                    var soluongton = ctpn - ctpx;
                    if (soluongton > 0)
                    {
                        cT_PhieuXuat.ThanhTien = cT_PhieuXuat.SoLuong * cT_PhieuXuat.DonGiaXuat;
                        db.CT_PhieuXuat.Add(cT_PhieuXuat);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    /*var ctpn = db.CT_PhieuNhap.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
                    var ctpx = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
                    var soluongton = ctpn - ctpx;*/
                    //if (soluongton > 0)
                    //{
                    cT_PhieuXuat.ThanhTien = cT_PhieuXuat.SoLuong * cT_PhieuXuat.DonGiaXuat;
                    db.CT_PhieuXuat.Add(cT_PhieuXuat);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //}
                }
                /*var ctpn = db.CT_PhieuNhap.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
                var ctpx = db.CT_PhieuXuat.Where(ct => ct.MaSach == sach.MaSach).Sum(ct => ct.SoLuong);
                var soluongton = ctpn-ctpx;
                if(soluongton > 0)
                {
                    cT_PhieuXuat.ThanhTien = cT_PhieuXuat.SoLuong * cT_PhieuXuat.DonGiaXuat;
                    db.CT_PhieuXuat.Add(cT_PhieuXuat);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } */
            }
            ViewBag.MaPhieu = new SelectList(db.PhieuXuats, "MaPhieu", "NguoiNhan", cT_PhieuXuat.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
            return View(cT_PhieuXuat);
        }

        // GET: CT_PhieuXuat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuXuat cT_PhieuXuat = db.CT_PhieuXuat.Find(id);
            if (cT_PhieuXuat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhieu = new SelectList(db.PhieuXuats, "MaPhieu", "NguoiNhan", cT_PhieuXuat.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
            return View(cT_PhieuXuat);
        }

        // POST: CT_PhieuXuat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DonGiaXuat,ThanhTien,SoLuong,MaPhieu,MaSach")] CT_PhieuXuat cT_PhieuXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cT_PhieuXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPhieu = new SelectList(db.PhieuXuats, "MaPhieu", "NguoiNhan", cT_PhieuXuat.MaPhieu);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
            return View(cT_PhieuXuat);
        }

        // GET: CT_PhieuXuat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuXuat cT_PhieuXuat = db.CT_PhieuXuat.Find(id);
            if (cT_PhieuXuat == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuXuat);
        }

        // POST: CT_PhieuXuat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_PhieuXuat cT_PhieuXuat = db.CT_PhieuXuat.Find(id);
            db.CT_PhieuXuat.Remove(cT_PhieuXuat);
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

            var model = db.CT_PhieuXuat.Where(x => (MaPhieu.HasValue) ?
                    (x.MaPhieu == MaPhieu && x.MaSach == MaSach) :
                    (x.MaSach == MaSach)
                );

            return Json(model.Count() == 0);

        }
    }
}
