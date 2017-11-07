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
    public class PhieuKiemKhoesController : Controller
    {
        private FAHASAEntities db = new FAHASAEntities();

        // GET: PhieuKiemKhoes
        public ActionResult Index()
        {
            var phieuKiemKho = db.PhieuKiemKhoes.Where(pkk => pkk.TinhTrang == true);
            return View(phieuKiemKho.ToList());
        }

        // GET: PhieuKiemKhoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null || !phieuKiemKho.TinhTrang)
            {
                return HttpNotFound();
            }
            db.Entry<PhieuKiemKho>(phieuKiemKho).Collection(pkk => pkk.CT_PhieuKiemKho).Load();
            foreach(var item in phieuKiemKho.CT_PhieuKiemKho)
            {
                item.Sach = db.Saches.Find(item.MaSach);
            }
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKhoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhieuKiemKhoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,NgayGio,TinhTrang")] PhieuKiemKho phieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.PhieuKiemKhoes.Add(phieuKiemKho);
                ICollection<Sach> Saches = db.Saches.ToList();
                foreach(var sach in Saches)
                {
                    CT_PhieuKiemKho cT_PhieuKiemKho = new CT_PhieuKiemKho();
                    cT_PhieuKiemKho.MaPhieu = phieuKiemKho.MaPhieu;
                    cT_PhieuKiemKho.MaSach = sach.MaSach;
                    DateTime ngayKiemKho = phieuKiemKho.NgayGio;
                    int SoLuongNhap = 0;
                    int SoLuongXuat = 0;
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= ngayKiemKho).ToList();
                    foreach (var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CT_PhieuNhap).Load();
                        foreach (var ctpn in phieuNhap.CT_PhieuNhap)
                        {
                            if (ctpn.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongNhap += ctpn.SoLuong;
                            }
                        }
                    }
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio <= ngayKiemKho).ToList();
                    foreach (var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(pn => pn.CT_PhieuXuat).Load();
                        foreach (var ctpn in phieuXuat.CT_PhieuXuat)
                        {
                            if (ctpn.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongXuat += ctpn.SoLuong;
                            }
                        }
                    }
                    cT_PhieuKiemKho.SoLuongTon = SoLuongNhap - SoLuongXuat;
                    db.CT_PhieuKiemKho.Add(cT_PhieuKiemKho);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKhoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null || !phieuKiemKho.TinhTrang)
            {
                return HttpNotFound();
            }
            return View(phieuKiemKho);
        }

        // POST: PhieuKiemKhoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieu,NgayGio,TinhTrang")] PhieuKiemKho phieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuKiemKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKhoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null || !phieuKiemKho.TinhTrang)
            {
                return HttpNotFound();
            }
            return View(phieuKiemKho);
        }

        // POST: PhieuKiemKhoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (ModelState.IsValid)
            {
                db.Entry(phieuKiemKho).State = EntityState.Modified;
                phieuKiemKho.TinhTrang = false;
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
