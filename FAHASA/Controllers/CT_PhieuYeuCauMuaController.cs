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
    public class CT_PhieuYeuCauMuaController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: CT_PhieuYeuCauMua
        public ActionResult Index()
        {
            var cT_PhieuYeuCauMua = db.CT_PhieuYeuCauMua.Include(c => c.PhieuYeuCauMua).Include(c => c.Sach);
            return View(cT_PhieuYeuCauMua.ToList());
        }

        // GET: CT_PhieuYeuCauMua/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuYeuCauMua cT_PhieuYeuCauMua = db.CT_PhieuYeuCauMua.Find(id);
            if (cT_PhieuYeuCauMua == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuYeuCauMua);
        }

        // GET: CT_PhieuYeuCauMua/Create
        public ActionResult Create(int id)
        {
            ViewBag.MaPhieu = id;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_PhieuYeuCauMua/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SoLuong,MaPhieu,MaSach")] CT_PhieuYeuCauMua cT_PhieuYeuCauMua)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuYeuCauMua tempCTPYCM = db.CT_PhieuYeuCauMua.AsNoTracking().Where(ctpycm => ctpycm.MaPhieu == cT_PhieuYeuCauMua.MaPhieu && ctpycm.MaSach == cT_PhieuYeuCauMua.MaSach).FirstOrDefault();
                DateTime NgayLapPhieuMua = db.PhieuYeuCauMuas.Find(cT_PhieuYeuCauMua.MaPhieu).NgayGio;
                ICollection<PhieuNhap> PhieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= NgayLapPhieuMua).ToList();
                ICollection<PhieuXuat> PhieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio < NgayLapPhieuMua).ToList();
                int SoLuongTonTruoc = 0;
                foreach(var phieuNhap in PhieuNhaps)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach(var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if(ctpn.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonTruoc += ctpn.SoLuong;
                            break;
                        }
                    }
                }
                foreach(var phieuXuat in PhieuXuats)
                {
                    db.Entry(phieuXuat).Collection(px => px.CTPhieuXuats).Load();
                    foreach(var ctpx in phieuXuat.CTPhieuXuats)
                    {
                        if(ctpx.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonTruoc -= ctpx.SoLuong;
                        }
                    }
                }
                SoLuongTonTruoc -= cT_PhieuYeuCauMua.SoLuong;
                if (SoLuongTonTruoc < 0)
                {
                    ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
                    return View(cT_PhieuYeuCauMua);
                }
                
                ICollection<PhieuNhap> PhieuNhap1s = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio > NgayLapPhieuMua).ToList();
                ICollection<PhieuXuat> PhieuXuat1s = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio > NgayLapPhieuMua).ToList();
                int SoLuongTonSau = 0;
                foreach (var phieuNhap in PhieuNhap1s)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonSau += ctpn.SoLuong;
                            break;
                        }
                    }
                }
                foreach (var phieuXuat in PhieuXuat1s)
                {
                    db.Entry(phieuXuat).Collection(px => px.CTPhieuXuats).Load();
                    foreach (var ctpx in phieuXuat.CTPhieuXuats)
                    {
                        if (ctpx.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonSau -= ctpx.SoLuong;
                        }
                    }
                }
                if (SoLuongTonTruoc + SoLuongTonSau < 0)
                {
                    ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
                    return View(cT_PhieuYeuCauMua);
                }
                if (tempCTPYCM != null)
                {
                    db.Entry(tempCTPYCM).State = EntityState.Modified;
                    tempCTPYCM.SoLuong += cT_PhieuYeuCauMua.SoLuong;
                }
                else
                {
                    db.CT_PhieuYeuCauMua.Add(cT_PhieuYeuCauMua);
                }
                db.SaveChanges();
                return RedirectToAction("Details", "PhieuYeuCauMuas", new { id = cT_PhieuYeuCauMua.MaPhieu });
            }

            ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
            return View(cT_PhieuYeuCauMua);
        }

        // GET: CT_PhieuYeuCauMua/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuYeuCauMua cT_PhieuYeuCauMua = db.CT_PhieuYeuCauMua.Find(id);
            if (cT_PhieuYeuCauMua == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
            return View(cT_PhieuYeuCauMua);
        }

        // POST: CT_PhieuYeuCauMua/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SoLuong,MaPhieu,MaSach")] CT_PhieuYeuCauMua cT_PhieuYeuCauMua)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuYeuCauMua tempCTPYCM = db.CT_PhieuYeuCauMua.AsNoTracking().Where(ctpycm => ctpycm.MaPhieu == cT_PhieuYeuCauMua.MaPhieu && ctpycm.MaSach == cT_PhieuYeuCauMua.MaSach).FirstOrDefault();
                DateTime NgayLapPhieuMua = db.PhieuYeuCauMuas.Find(cT_PhieuYeuCauMua.MaPhieu).NgayGio;
                ICollection<PhieuNhap> PhieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= NgayLapPhieuMua).ToList();
                ICollection<PhieuXuat> PhieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio < NgayLapPhieuMua).ToList();
                int SoLuongTonTruoc = 0;
                foreach (var phieuNhap in PhieuNhaps)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonTruoc += ctpn.SoLuong;
                            break;
                        }
                    }
                }
                foreach (var phieuXuat in PhieuXuats)
                {
                    db.Entry(phieuXuat).Collection(px => px.CTPhieuXuats).Load();
                    foreach (var ctpx in phieuXuat.CTPhieuXuats)
                    {
                        if (ctpx.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonTruoc -= ctpx.SoLuong;
                        }
                    }
                }
                if (tempCTPYCM != null && tempCTPYCM.ID != cT_PhieuYeuCauMua.ID)
                    SoLuongTonTruoc -= (tempCTPYCM.SoLuong + cT_PhieuYeuCauMua.SoLuong);
                else
                    SoLuongTonTruoc -= cT_PhieuYeuCauMua.SoLuong;
                if (SoLuongTonTruoc < 0)
                {
                    ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
                    return View(cT_PhieuYeuCauMua);
                }

                ICollection<PhieuNhap> PhieuNhap1s = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio > NgayLapPhieuMua).ToList();
                ICollection<PhieuXuat> PhieuXuat1s = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio > NgayLapPhieuMua).ToList();
                int SoLuongTonSau = 0;
                foreach (var phieuNhap in PhieuNhap1s)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonSau += ctpn.SoLuong;
                            break;
                        }
                    }
                }
                foreach (var phieuXuat in PhieuXuat1s)
                {
                    db.Entry(phieuXuat).Collection(px => px.CTPhieuXuats).Load();
                    foreach (var ctpx in phieuXuat.CTPhieuXuats)
                    {
                        if (ctpx.MaSach == cT_PhieuYeuCauMua.MaSach)
                        {
                            SoLuongTonSau -= ctpx.SoLuong;
                        }
                    }
                }
                if (SoLuongTonTruoc + SoLuongTonSau < 0)
                {
                    ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
                    return View(cT_PhieuYeuCauMua);
                }
                if (tempCTPYCM != null && tempCTPYCM.ID != cT_PhieuYeuCauMua.ID)
                {
                    db.Entry(tempCTPYCM).State = EntityState.Modified;
                    tempCTPYCM.SoLuong += cT_PhieuYeuCauMua.SoLuong;
                    CT_PhieuYeuCauMua deletedCTPYCM = db.CT_PhieuYeuCauMua.Find(cT_PhieuYeuCauMua.ID);
                    db.CT_PhieuYeuCauMua.Remove(deletedCTPYCM);
                }
                else
                {
                    db.Entry(cT_PhieuYeuCauMua).State = EntityState.Modified;
                     
                }
                db.SaveChanges();
                return RedirectToAction("Details", "PhieuYeuCauMuas", new { id = cT_PhieuYeuCauMua.MaPhieu });
            }
            ViewBag.MaPhieu = cT_PhieuYeuCauMua.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuYeuCauMua.MaSach);
            return View(cT_PhieuYeuCauMua);
        }

        // GET: CT_PhieuYeuCauMua/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuYeuCauMua cT_PhieuYeuCauMua = db.CT_PhieuYeuCauMua.Find(id);
            if (cT_PhieuYeuCauMua == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuYeuCauMua);
        }

        // POST: CT_PhieuYeuCauMua/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_PhieuYeuCauMua cT_PhieuYeuCauMua = db.CT_PhieuYeuCauMua.Find(id);
            db.CT_PhieuYeuCauMua.Remove(cT_PhieuYeuCauMua);
            db.SaveChanges();
            return RedirectToAction("Details", "PhieuYeuCauMuas", new { id = cT_PhieuYeuCauMua.MaPhieu });
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
