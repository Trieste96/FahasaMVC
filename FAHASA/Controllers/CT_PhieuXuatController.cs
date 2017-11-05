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
    public class CT_PhieuXuatController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: CT_PhieuXuat
        public ActionResult Index()
        {
            var cT_PhieuXuat = db.CT_PhieuXuat.Include(c => c.PhieuXuat).Include(c => c.Sach);
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
        public ActionResult Create(int id)
        {
            ViewBag.MaPhieu = id;
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
                CT_PhieuXuat tempCTPX = db.CT_PhieuXuat.AsNoTracking().Where(ctpycm => ctpycm.MaPhieu == cT_PhieuXuat.MaPhieu && ctpycm.MaSach == cT_PhieuXuat.MaSach).FirstOrDefault();
                DateTime NgayLapPhieuXuat = db.PhieuXuats.Find(cT_PhieuXuat.MaPhieu).NgayGio;
                ICollection<PhieuNhap> PhieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= NgayLapPhieuXuat).ToList();
                ICollection<PhieuXuat> PhieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio <= NgayLapPhieuXuat).ToList();
                int SoLuongTonTruoc = 0;
                foreach (var phieuNhap in PhieuNhaps)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuXuat.MaSach)
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
                        if (ctpx.MaSach == cT_PhieuXuat.MaSach)
                        {
                            SoLuongTonTruoc -= ctpx.SoLuong;
                        }
                    }
                }
                SoLuongTonTruoc -= cT_PhieuXuat.SoLuong;
                if (SoLuongTonTruoc < 0)
                {
                    ViewBag.ErrorMessage = "Số sách trong kho hiện tại không còn đủ";
                    ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
                    return View(cT_PhieuXuat);
                }
                ICollection<PhieuNhap> PhieuNhap1s = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio > NgayLapPhieuXuat).ToList();
                ICollection<PhieuXuat> PhieuXuat1s = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio > NgayLapPhieuXuat).ToList();
                int SoLuongTonSau = 0;
                foreach (var phieuNhap in PhieuNhap1s)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuXuat.MaSach)
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
                        if (ctpx.MaSach == cT_PhieuXuat.MaSach)
                        {
                            SoLuongTonSau -= ctpx.SoLuong;
                        }
                    }
                }
                if (SoLuongTonTruoc + SoLuongTonSau < 0)
                {
                    ViewBag.ErrorMessage = "Số sách trong kho hiện tại không còn đủ";
                    ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
                    return View(cT_PhieuXuat);
                }
                if (tempCTPX != null)
                {
                    PhieuXuat phieuXuat = db.PhieuXuats.Find(cT_PhieuXuat.MaPhieu);
                    DaiLy daiLy = db.DaiLies.Find(phieuXuat.MaDaiLy);
                    db.Entry(tempCTPX).State = EntityState.Modified;
                    db.Entry(phieuXuat).State = EntityState.Modified;
                    db.Entry(daiLy).State = EntityState.Modified;
                    phieuXuat.TongTien -= tempCTPX.ThanhTien;
                    daiLy.SoNo -= tempCTPX.ThanhTien;
                    tempCTPX.SoLuong += cT_PhieuXuat.SoLuong;
                    phieuXuat.TongTien += tempCTPX.ThanhTien;
                    daiLy.SoNo += tempCTPX.ThanhTien;

                }
                else
                {
                    PhieuXuat phieuXuat = db.PhieuXuats.Find(cT_PhieuXuat.MaPhieu);
                    DaiLy daiLy = db.DaiLies.Find(phieuXuat.MaDaiLy);
                    db.CT_PhieuXuat.Add(cT_PhieuXuat);
                    db.Entry(phieuXuat).State = EntityState.Modified;
                    db.Entry(daiLy).State = EntityState.Modified;
                    phieuXuat.TongTien += cT_PhieuXuat.ThanhTien;
                    daiLy.SoNo += cT_PhieuXuat.ThanhTien;
                }
                db.SaveChanges();
                return RedirectToAction("Details","PhieuXuats",new { id = cT_PhieuXuat.MaPhieu });
            }

            ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
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
            ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
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
                CT_PhieuXuat tempCTPX = db.CT_PhieuXuat.AsNoTracking().Where(ctpycm => ctpycm.MaPhieu == cT_PhieuXuat.MaPhieu && ctpycm.MaSach == cT_PhieuXuat.MaSach).FirstOrDefault();
                DateTime NgayLapPhieuXuat = db.PhieuXuats.Find(cT_PhieuXuat.MaPhieu).NgayGio;
                ICollection<PhieuNhap> PhieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= NgayLapPhieuXuat).ToList();
                ICollection<PhieuXuat> PhieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio < NgayLapPhieuXuat).ToList();
                int SoLuongTonTruoc = 0;
                foreach (var phieuNhap in PhieuNhaps)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuXuat.MaSach)
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
                        if (ctpx.MaSach == cT_PhieuXuat.MaSach)
                        {
                            SoLuongTonTruoc -= ctpx.SoLuong;
                        }
                    }
                }
                if (tempCTPX != null && tempCTPX.ID != cT_PhieuXuat.ID)
                    SoLuongTonTruoc -= (cT_PhieuXuat.SoLuong + tempCTPX.SoLuong);
                else
                    SoLuongTonTruoc -= cT_PhieuXuat.SoLuong;
                if (SoLuongTonTruoc < 0)
                {
                    ViewBag.ErrorMessage = "Số sách trong kho hiện tại không còn đủ";
                    ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
                    return View(cT_PhieuXuat);
                }
                ICollection<PhieuNhap> PhieuNhap1s = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio > NgayLapPhieuXuat).ToList();
                ICollection<PhieuXuat> PhieuXuat1s = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio > NgayLapPhieuXuat).ToList();
                int SoLuongTonSau = 0;
                foreach (var phieuNhap in PhieuNhap1s)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == cT_PhieuXuat.MaSach)
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
                        if (ctpx.MaSach == cT_PhieuXuat.MaSach)
                        {
                            SoLuongTonSau -= ctpx.SoLuong;
                        }
                    }
                }
                if (SoLuongTonTruoc + SoLuongTonSau < 0)
                {
                    ViewBag.ErrorMessage = "Số sách trong kho hiện tại không còn đủ";
                    ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
                    ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuXuat.MaSach);
                    return View(cT_PhieuXuat);
                }
                if (tempCTPX != null && tempCTPX.ID != cT_PhieuXuat.ID)
                {
                    PhieuXuat phieuXuat = db.PhieuXuats.Find(cT_PhieuXuat.MaPhieu);
                    DaiLy daiLy = db.DaiLies.Find(phieuXuat.MaDaiLy);

                    db.Entry(tempCTPX).State = EntityState.Modified;
                    db.Entry(phieuXuat).State = EntityState.Modified;
                    db.Entry(daiLy).State = EntityState.Modified;

                    phieuXuat.TongTien -= tempCTPX.ThanhTien;
                    daiLy.SoNo -= tempCTPX.ThanhTien - cT_PhieuXuat.ThanhTien;

                    tempCTPX.SoLuong += cT_PhieuXuat.SoLuong;

                    phieuXuat.TongTien += tempCTPX.ThanhTien;
                    daiLy.SoNo += tempCTPX.ThanhTien;

                    CT_PhieuYeuCauMua deletedCTPYCM = db.CT_PhieuYeuCauMua.Find(cT_PhieuXuat.ID);
                    db.CT_PhieuYeuCauMua.Remove(deletedCTPYCM);
                }
                else
                {
                    PhieuXuat phieuXuat = db.PhieuXuats.Find(cT_PhieuXuat.MaPhieu);
                    DaiLy daiLy = db.DaiLies.Find(phieuXuat.MaDaiLy);

                    CT_PhieuXuat updatedCTPhieuXuat = db.CT_PhieuXuat.Find(cT_PhieuXuat.ID);
                    updatedCTPhieuXuat.SoLuong = cT_PhieuXuat.SoLuong;
                    updatedCTPhieuXuat.DonGiaXuat = cT_PhieuXuat.DonGiaXuat;
                    updatedCTPhieuXuat.MaSach = cT_PhieuXuat.MaSach;

                    db.Entry(updatedCTPhieuXuat).State = EntityState.Modified;
                    db.Entry(phieuXuat).State = EntityState.Modified;
                    db.Entry(daiLy).State = EntityState.Modified;

                    phieuXuat.TongTien += cT_PhieuXuat.ThanhTien;
                    daiLy.SoNo += cT_PhieuXuat.ThanhTien;
                }
                db.SaveChanges();
                return RedirectToAction("Details","PhieuXuats",new { id = cT_PhieuXuat.MaPhieu});
            }
            ViewBag.MaPhieu = cT_PhieuXuat.MaPhieu;
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
    }
}
