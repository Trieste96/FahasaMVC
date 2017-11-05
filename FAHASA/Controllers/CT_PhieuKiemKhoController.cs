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
    public class CT_PhieuKiemKhoController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: CT_PhieuKiemKho
        public ActionResult Index()
        {
            var cT_PhieuKiemKho = db.CT_PhieuKiemKho.Include(c => c.PhieuKiemKho).Include(c => c.Sach);
            return View(cT_PhieuKiemKho.ToList());
        }

        // GET: CT_PhieuKiemKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuKiemKho cT_PhieuKiemKho = db.CT_PhieuKiemKho.Find(id);
            if (cT_PhieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuKiemKho);
        }

        // GET: CT_PhieuKiemKho/Create
        public ActionResult Create(int id)
        {
            ViewBag.MaPhieu = id;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_PhieuKiemKho/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SoLuongTon,MaPhieu,MaSach")] CT_PhieuKiemKho cT_PhieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuKiemKho tempCTPKK = db.CT_PhieuKiemKho.AsNoTracking().Where(ctpkk => ctpkk.MaPhieu == cT_PhieuKiemKho.MaPhieu && ctpkk.MaSach == cT_PhieuKiemKho.MaSach).FirstOrDefault();
                if(tempCTPKK == null)
                {
                    DateTime ngayKiemKho = db.PhieuKiemKhoes.Where(pkk => pkk.MaPhieu == cT_PhieuKiemKho.MaPhieu).FirstOrDefault().NgayGio;
                    PhieuKiemKho temp = db.PhieuKiemKhoes.Where(pkk => pkk.NgayGio < ngayKiemKho).OrderByDescending(pkk => pkk.NgayGio).FirstOrDefault();
                    DateTime ngayKiemKhoGanNhat = temp != null ? temp.NgayGio : new DateTime(1970, 1, 1);
                    int SoLuongNhap = 0;
                    int SoLuongXuat = 0;
                    int SoLuongTonTruoc = 0;
                    if (temp != null)
                    {
                        db.Entry<PhieuKiemKho>(temp).Collection(pkk => pkk.CTPhieuKiemKhoes).Load();
                        foreach (var ctpkk in temp.CTPhieuKiemKhoes)
                        {
                            if (ctpkk.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongTonTruoc = ctpkk.SoLuongTon;
                                break;
                            }
                        }
                    }
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= ngayKiemKho && pn.NgayGio > ngayKiemKhoGanNhat).ToList();
                    foreach(var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                        foreach(var ctpn in phieuNhap.CTPhieuNhaps)
                        {
                            if(ctpn.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongNhap += ctpn.SoLuong;
                            }
                        }
                    }
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio <= ngayKiemKho && px.NgayGio > ngayKiemKhoGanNhat).ToList();
                    foreach (var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(pn => pn.CTPhieuXuats).Load();
                        foreach (var ctpn in phieuXuat.CTPhieuXuats)
                        {
                            if (ctpn.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongXuat += ctpn.SoLuong;
                            }
                        }
                    }
                    cT_PhieuKiemKho.SoLuongTon = SoLuongNhap - SoLuongXuat + SoLuongTonTruoc;
                    db.CT_PhieuKiemKho.Add(cT_PhieuKiemKho);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", "PhieuKiemKhoes", new { id = cT_PhieuKiemKho.MaPhieu });
            }

            ViewBag.MaPhieu = cT_PhieuKiemKho.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuKiemKho.MaSach);
            return View(cT_PhieuKiemKho);
        }

        // GET: CT_PhieuKiemKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuKiemKho cT_PhieuKiemKho = db.CT_PhieuKiemKho.Find(id);
            if (cT_PhieuKiemKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhieu = cT_PhieuKiemKho.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuKiemKho.MaSach);
            return View(cT_PhieuKiemKho);
        }

        // POST: CT_PhieuKiemKho/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SoLuongTon,MaPhieu,MaSach")] CT_PhieuKiemKho cT_PhieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                CT_PhieuKiemKho tempCTPKK = db.CT_PhieuKiemKho.AsNoTracking().Where(ctpkk => ctpkk.MaPhieu == cT_PhieuKiemKho.MaPhieu && ctpkk.MaSach == cT_PhieuKiemKho.MaSach).FirstOrDefault();
                if (tempCTPKK == null)
                {
                    DateTime ngayKiemKho = db.PhieuKiemKhoes.Where(pkk => pkk.MaPhieu == cT_PhieuKiemKho.MaPhieu).FirstOrDefault().NgayGio;
                    PhieuKiemKho temp = db.PhieuKiemKhoes.Where(pkk => pkk.NgayGio < ngayKiemKho).OrderByDescending(pkk => pkk.NgayGio).FirstOrDefault();
                    DateTime ngayKiemKhoGanNhat = temp != null ? temp.NgayGio : new DateTime(1970, 1, 1);
                    int SoLuongNhap = 0;
                    int SoLuongXuat = 0;
                    int SoLuongTonTruoc = 0;
                    if(temp != null)
                    {
                        db.Entry<PhieuKiemKho>(temp).Collection(pkk => pkk.CTPhieuKiemKhoes).Load();
                        foreach(var ctpkk in temp.CTPhieuKiemKhoes)
                        {
                            if(ctpkk.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongTonTruoc = ctpkk.SoLuongTon;
                                break;
                            }
                        }
                    }
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= ngayKiemKho && pn.NgayGio > ngayKiemKhoGanNhat).ToList();
                    foreach (var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                        foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                        {
                            if (ctpn.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongNhap += ctpn.SoLuong;
                            }
                        }
                    }
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.NgayGio <= ngayKiemKho && px.NgayGio > ngayKiemKhoGanNhat).ToList();
                    foreach (var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(pn => pn.CTPhieuXuats).Load();
                        foreach (var ctpn in phieuXuat.CTPhieuXuats)
                        {
                            if (ctpn.MaSach == cT_PhieuKiemKho.MaSach)
                            {
                                SoLuongXuat += ctpn.SoLuong;
                            }
                        }
                    }
                    db.Entry(cT_PhieuKiemKho).State = EntityState.Modified;
                    cT_PhieuKiemKho.SoLuongTon = SoLuongNhap - SoLuongXuat + SoLuongTonTruoc;
                }
                else
                {
                    CT_PhieuKiemKho deletedCTPKK = db.CT_PhieuKiemKho.Find(cT_PhieuKiemKho.ID);
                    db.CT_PhieuKiemKho.Remove(deletedCTPKK);
                }
                db.SaveChanges();
                return RedirectToAction("Details","PhieuKiemKhoes",new { id = cT_PhieuKiemKho.MaPhieu });
            }
            ViewBag.MaPhieu = cT_PhieuKiemKho.MaPhieu;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", cT_PhieuKiemKho.MaSach);
            return View(cT_PhieuKiemKho);
        }

        // GET: CT_PhieuKiemKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_PhieuKiemKho cT_PhieuKiemKho = db.CT_PhieuKiemKho.Find(id);
            if (cT_PhieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(cT_PhieuKiemKho);
        }

        // POST: CT_PhieuKiemKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_PhieuKiemKho cT_PhieuKiemKho = db.CT_PhieuKiemKho.Find(id);
            db.CT_PhieuKiemKho.Remove(cT_PhieuKiemKho);
            db.SaveChanges();
            return RedirectToAction("Details","PhieuKiemKhoes", new { id = cT_PhieuKiemKho.MaPhieu });
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
