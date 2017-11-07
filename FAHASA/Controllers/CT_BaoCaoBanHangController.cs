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
    public class CT_BaoCaoBanHangController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        private void recalculateSum(CT_BaoCaoBanHang cT_BaoCaoBanHang)
        {
            //Tính lại tổng tiền
            var phieu = db.BaoCaoBanHangs.Find(cT_BaoCaoBanHang.MaBC);

            phieu.TongTien = db.CT_BaoCaoBanHang.Where(ct => ct.MaBC == cT_BaoCaoBanHang.MaBC).Sum(ct => ct.ThanhTien).GetValueOrDefault();
            db.Entry(phieu).State = EntityState.Modified;
            db.SaveChanges();
        }
        // GET: CT_BaoCaoBanHang
        public ActionResult Index()
        {
            var cT_BaoCaoBanHang = db.CT_BaoCaoBanHang.Include(c => c.BaoCaoBanHang).Include(c => c.Sach);
            return View(cT_BaoCaoBanHang.ToList());
        }

        // GET: CT_BaoCaoBanHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_BaoCaoBanHang cT_BaoCaoBanHang = db.CT_BaoCaoBanHang.Find(id);
            if (cT_BaoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(cT_BaoCaoBanHang);
        }

        // POST: CT_BaoCaoBanHang/Create
        public ActionResult Create(int? MaBC)
        {
            ViewBag.MaBC = MaBC;
            IQueryable<Sach> ct_phieu = db.CT_BaoCaoBanHang.Where(ct => ct.MaBC == MaBC).Select(ct => ct.Sach);

            ViewBag.MaSach = new SelectList(db.Saches.Except(ct_phieu), "MaSach", "TenSach");
            return View();
        }

        // POST: CT_BaoCaoBanHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GiaBan,SoLuong,ThanhTien,MaBC,MaSach")] CT_BaoCaoBanHang cT_BaoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.CT_BaoCaoBanHang.Add(cT_BaoCaoBanHang);
                try { db.SaveChanges(); }
                catch (Exception)
                {
                    string message = "Sách này đã có trong phiếu! Xin vui lòng thêm sách khác.";
                    return Content(String.Format("<script language='javascript' type='text/javascript'>alert('{0}');</script>", message));
                }
                recalculateSum(cT_BaoCaoBanHang);
                return RedirectToAction("Edit", "BaoCaoBanHang", new { id=cT_BaoCaoBanHang.MaBC});
            }

            ViewBag.MaBC = new SelectList(db.BaoCaoBanHangs, "MaBC", "MaBC", cT_BaoCaoBanHang.MaBC);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_BaoCaoBanHang.MaSach);
            return View(cT_BaoCaoBanHang);
        }

        // GET: CT_BaoCaoBanHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_BaoCaoBanHang cT_BaoCaoBanHang = db.CT_BaoCaoBanHang.Find(id);
            if (cT_BaoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaBC = new SelectList(db.BaoCaoBanHangs, "MaBC", "MaBC", cT_BaoCaoBanHang.MaBC);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_BaoCaoBanHang.MaSach);
            return View(cT_BaoCaoBanHang);
        }

        // POST: CT_BaoCaoBanHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GiaBan,SoLuong,ThanhTien,MaBC,MaSach,ID")] CT_BaoCaoBanHang cT_BaoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cT_BaoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                recalculateSum(cT_BaoCaoBanHang);
                return RedirectToAction("Edit", "BaoCaoBanHang", new { id= cT_BaoCaoBanHang.MaBC });
            }
            ViewBag.MaBC = new SelectList(db.BaoCaoBanHangs, "MaBC", "MaBC", cT_BaoCaoBanHang.MaBC);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_BaoCaoBanHang.MaSach);
            return View(cT_BaoCaoBanHang);
        }

        // GET: CT_BaoCaoBanHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_BaoCaoBanHang cT_BaoCaoBanHang = db.CT_BaoCaoBanHang.Find(id);
            if (cT_BaoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(cT_BaoCaoBanHang);
        }

        // POST: CT_BaoCaoBanHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_BaoCaoBanHang cT_BaoCaoBanHang = db.CT_BaoCaoBanHang.Find(id);
            db.CT_BaoCaoBanHang.Remove(cT_BaoCaoBanHang);
            db.SaveChanges();
            return RedirectToAction("Edit", "BaoCaoBanHang", new { id = cT_BaoCaoBanHang.MaBC });
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
