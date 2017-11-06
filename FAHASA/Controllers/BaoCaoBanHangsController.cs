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
    public class BaoCaoBanHangsController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        // GET: BaoCaoBanHangs
        public ActionResult Index()
        {
            var baoCaoBanHangs = db.BaoCaoBanHangs.Include(b => b.DaiLy);
            return View(baoCaoBanHangs.ToList());
        }

        // GET: BaoCaoBanHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            db.Entry<BaoCaoBanHang>(baoCaoBanHang).Collection(bcbh => bcbh.CT_BaoCaoBanHang).Load();
            foreach (var item in baoCaoBanHang.CT_BaoCaoBanHang)
            {
                item.Sach = db.Saches.Find(item.MaSach);
            }
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy");
            return View();
        }

        // POST: BaoCaoBanHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBC,NgayGio,TongTien,MaDaiLy")] BaoCaoBanHang baoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.BaoCaoBanHangs.Add(baoCaoBanHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", baoCaoBanHang.MaDaiLy);
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", baoCaoBanHang.MaDaiLy);
            return View(baoCaoBanHang);
        }

        // POST: BaoCaoBanHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBC,NgayGio,TongTien,MaDaiLy")] BaoCaoBanHang baoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", baoCaoBanHang.MaDaiLy);
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoBanHang);
        }

        // POST: BaoCaoBanHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (ModelState.IsValid)
            {
                db.Entry(baoCaoBanHang).State = EntityState.Modified;
                baoCaoBanHang.TinhTrang = false;
                db.SaveChanges();
            }
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
