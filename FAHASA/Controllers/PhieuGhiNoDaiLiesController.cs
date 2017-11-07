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
    public class PhieuGhiNoDaiLiesController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        // GET: PhieuGhiNoDaiLies
        public ActionResult Index()
        {
            return View(db.PhieuGhiNoDaiLies.Where(pgndl => pgndl.TinhTrang == true).ToList());
        }

        // GET: PhieuGhiNoDaiLies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuGhiNoDaiLy phieuGhiNoDaiLy = db.PhieuGhiNoDaiLies.Find(id);
            if (phieuGhiNoDaiLy == null || !phieuGhiNoDaiLy.TinhTrang)
            {
                return HttpNotFound();
            }
            db.Entry<PhieuGhiNoDaiLy>(phieuGhiNoDaiLy).Collection(pgndl => pgndl.CT_PhieuGhiNoDaiLy).Load();
            foreach(var item in phieuGhiNoDaiLy.CT_PhieuGhiNoDaiLy)
            {
                item.Sach = db.Saches.Find(item.MaSach);
            }
            return View(phieuGhiNoDaiLy);
        }

        // GET: PhieuGhiNoDaiLies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhieuGhiNoDaiLies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,NgayGio,TongNo,TinhTrang")] PhieuGhiNoDaiLy phieuGhiNoDaiLy)
        {
            if (ModelState.IsValid)
            {
                db.PhieuGhiNoDaiLies.Add(phieuGhiNoDaiLy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phieuGhiNoDaiLy);
        }

        // GET: PhieuGhiNoDaiLies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuGhiNoDaiLy phieuGhiNoDaiLy = db.PhieuGhiNoDaiLies.Find(id);
            if (phieuGhiNoDaiLy == null || !phieuGhiNoDaiLy.TinhTrang)
            {
                return HttpNotFound();
            }
            return View(phieuGhiNoDaiLy);
        }

        // POST: PhieuGhiNoDaiLies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieu,NgayGio,TongNo,TinhTrang")] PhieuGhiNoDaiLy phieuGhiNoDaiLy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuGhiNoDaiLy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phieuGhiNoDaiLy);
        }

        // GET: PhieuGhiNoDaiLies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuGhiNoDaiLy phieuGhiNoDaiLy = db.PhieuGhiNoDaiLies.Find(id);
            if (phieuGhiNoDaiLy == null || !phieuGhiNoDaiLy.TinhTrang)
            {
                return HttpNotFound();
            }
            return View(phieuGhiNoDaiLy);
        }

        // POST: PhieuGhiNoDaiLies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuGhiNoDaiLy phieuGhiNoDaiLy = db.PhieuGhiNoDaiLies.Find(id);
            if (ModelState.IsValid)
            {
                db.Entry(phieuGhiNoDaiLy).State = EntityState.Modified;
                phieuGhiNoDaiLy.TinhTrang = false;
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
