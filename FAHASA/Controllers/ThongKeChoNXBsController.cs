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
    public class ThongKeChoNXBsController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: ThongKeChoNXBs
        public ActionResult Index()
        {
            var thongKeChoNXBs = db.ThongKeChoNXBs.Where(tkcnxb => tkcnxb.TinhTrang == true).Include(t => t.NhaXuatBan);
            return View(thongKeChoNXBs.ToList());
        }

        // GET: ThongKeChoNXBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(id);
            if (thongKeChoNXB == null || !thongKeChoNXB.TinhTrang)
            {
                return HttpNotFound();
            }
            db.Entry<ThongKeChoNXB>(thongKeChoNXB).Collection(tkcnxb => tkcnxb.CTThongKeChoNXBs).Load();
            foreach(var item in thongKeChoNXB.CTThongKeChoNXBs)
            {
                item.Sach = db.Saches.Find(item.MaSach);
            }
            return View(thongKeChoNXB);
        }

        // GET: ThongKeChoNXBs/Create
        public ActionResult Create()
        {
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            return View();
        }

        // POST: ThongKeChoNXBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaPhieu,NgayGio,TongTien,TinhTrang,MaNXB")] ThongKeChoNXB thongKeChoNXB)
        {
            if (ModelState.IsValid)
            {
                db.ThongKeChoNXBs.Add(thongKeChoNXB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", thongKeChoNXB.MaNXB);
            return View(thongKeChoNXB);
        }

        // GET: ThongKeChoNXBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(id);
            if (thongKeChoNXB == null || !thongKeChoNXB.TinhTrang)
            {
                return HttpNotFound();
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", thongKeChoNXB.MaNXB);
            return View(thongKeChoNXB);
        }

        // POST: ThongKeChoNXBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaPhieu,NgayGio,TongTien,TinhTrang,MaNXB")] ThongKeChoNXB thongKeChoNXB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thongKeChoNXB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", thongKeChoNXB.MaNXB);
            return View(thongKeChoNXB);
        }

        // GET: ThongKeChoNXBs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(id);
            if (thongKeChoNXB == null || !thongKeChoNXB.TinhTrang)
            {
                return HttpNotFound();
            }
            return View(thongKeChoNXB);
        }

        // POST: ThongKeChoNXBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(id);
            if (ModelState.IsValid)
            {
                db.Entry(thongKeChoNXB).State = EntityState.Modified;
                thongKeChoNXB.TinhTrang = false;
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
