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

        // GET: CT_BaoCaoBanHang
        public ActionResult Index()
        {
            var cTBaoCaoBanHangs = db.CT_BaoCaoBanHang.Include(c => c.BaoCaoBanHang).Include(c => c.Sach);
            return View(cTBaoCaoBanHangs.ToList());
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

        // GET: CT_BaoCaoBanHang/Create
        public ActionResult Create(int id)
        {
            ViewBag.MaBC = id;
            ICollection<Sach> Saches = new List<Sach>(0);
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio <= baoCaoBanHang.NgayGio).ToList();
            foreach(var phieuXuat in phieuXuats)
            {
                db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                foreach(var ctpx in phieuXuat.CT_PhieuXuat)
                {
                    int flag = 0;
                    foreach(var sach in Saches)
                    {
                        if (sach.MaSach == ctpx.MaSach)
                            break;
                        else
                            flag++;
                    }
                    if(flag == Saches.Count)
                    {
                        Saches.Add(db.Saches.Find(ctpx.MaSach));
                    }
                }
            }
            ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_BaoCaoBanHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaBC,MaSach,GiaBan,SoLuong,ThanhTien")] CT_BaoCaoBanHang cT_BaoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(cT_BaoCaoBanHang.MaBC);
                CT_BaoCaoBanHang tempCTBCBH = db.CT_BaoCaoBanHang.Where(ctbcbh => ctbcbh.MaBC == cT_BaoCaoBanHang.MaBC && ctbcbh.MaSach == cT_BaoCaoBanHang.MaSach).FirstOrDefault();
                if(tempCTBCBH != null)
                {
                    int SoLuongTungBan = tempCTBCBH.SoLuong;
                    ICollection<BaoCaoBanHang> baoCaoBanHangs = db.BaoCaoBanHangs.Where(bcbh => bcbh.TinhTrang == true && bcbh.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach (var bcbhang in baoCaoBanHangs)
                    {
                        db.Entry<BaoCaoBanHang>(bcbhang).Collection(bcbh => bcbh.CT_BaoCaoBanHang).Load();
                        foreach (var ctbcbh in bcbhang.CT_BaoCaoBanHang)
                        {
                            if (ctbcbh.MaSach == tempCTBCBH.MaSach)
                            {
                                SoLuongTungBan += ctbcbh.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongXuat = 0;
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach (var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                        foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                        {
                            if (ctpx.MaSach == tempCTBCBH.MaSach)
                            {
                                SoLuongXuat += ctpx.SoLuong;
                            }
                        }
                    }
                    if (SoLuongXuat < SoLuongTungBan + cT_BaoCaoBanHang.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong báo cáo bán hàng vượt quá số sách mà đại lí mua.";
                        ViewBag.MaBC = tempCTBCBH.MaBC;
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuXuat> phieuXuat1s = db.PhieuXuats.Where(px => px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio <= baoCaoBanHang.NgayGio).ToList();
                        foreach (var phieuXuat in phieuXuats)
                        {
                            db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                            foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpx.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpx.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        return View(cT_BaoCaoBanHang);
                    }
                    db.Entry(tempCTBCBH).State = EntityState.Modified;
                    baoCaoBanHang.TongTien -= tempCTBCBH.ThanhTien;
                    tempCTBCBH.SoLuong += cT_BaoCaoBanHang.SoLuong;
                    tempCTBCBH.GiaBan = cT_BaoCaoBanHang.GiaBan;
                    baoCaoBanHang.TongTien += tempCTBCBH.ThanhTien;
                }
                else
                {
                    int SoLuongTungBan = 0;
                    ICollection<BaoCaoBanHang> baoCaoBanHangs = db.BaoCaoBanHangs.Where(bcbh => bcbh.TinhTrang == true && bcbh.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach(var bcbhang in baoCaoBanHangs)
                    {
                        db.Entry<BaoCaoBanHang>(bcbhang).Collection(bcbh => bcbh.CT_BaoCaoBanHang).Load();
                        foreach(var ctbcbh in bcbhang.CT_BaoCaoBanHang)
                        {
                            if(ctbcbh.MaSach == cT_BaoCaoBanHang.MaSach)
                            {
                                SoLuongTungBan += ctbcbh.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongXuat = 0;
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach(var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                        foreach(var ctpx in phieuXuat.CT_PhieuXuat)
                        {
                            if(ctpx.MaSach == cT_BaoCaoBanHang.MaSach)
                            {
                                SoLuongXuat += ctpx.SoLuong;
                                break;
                            }
                        }
                    }
                    if(SoLuongXuat < SoLuongTungBan + cT_BaoCaoBanHang.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong báo cáo bán hàng vượt quá số sách mà đại lí mua.";
                        ViewBag.MaBC = cT_BaoCaoBanHang.MaBC;
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuXuat> phieuXuat1s = db.PhieuXuats.Where(px => px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio <= baoCaoBanHang.NgayGio).ToList();
                        foreach (var phieuXuat in phieuXuats)
                        {
                            db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                            foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpx.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpx.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        return View(cT_BaoCaoBanHang);
                    }
                    baoCaoBanHang.TongTien += cT_BaoCaoBanHang.ThanhTien;
                    db.CT_BaoCaoBanHang.Add(cT_BaoCaoBanHang);
                }
                db.Entry<BaoCaoBanHang>(baoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "BaoCaoBanHangs", new { id = cT_BaoCaoBanHang.MaBC });
            }

            ViewBag.MaBC = cT_BaoCaoBanHang.MaBC;
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
            ViewBag.MaBC = cT_BaoCaoBanHang.MaBC;
            ICollection<Sach> Saches = new List<Sach>(0);
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(cT_BaoCaoBanHang.MaBC);
            ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio <= baoCaoBanHang.NgayGio).ToList();
            foreach (var phieuXuat in phieuXuats)
            {
                db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                {
                    int flag = 0;
                    foreach (var sach in Saches)
                    {
                        if (sach.MaSach == ctpx.MaSach)
                            break;
                        else
                            flag++;
                    }
                    if (flag == Saches.Count)
                    {
                        Saches.Add(db.Saches.Find(ctpx.MaSach));
                    }
                }
            }
            ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
            return View(cT_BaoCaoBanHang);
        }

        // POST: CT_BaoCaoBanHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaBC,MaSach,GiaBan,SoLuong,ThanhTien")] CT_BaoCaoBanHang cT_BaoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(cT_BaoCaoBanHang.MaBC);
                CT_BaoCaoBanHang tempCTBCBH = db.CT_BaoCaoBanHang.Where(ctbcbh => ctbcbh.MaBC == cT_BaoCaoBanHang.MaBC && ctbcbh.MaSach == cT_BaoCaoBanHang.MaSach).FirstOrDefault();
                if (tempCTBCBH != null && tempCTBCBH.ID != cT_BaoCaoBanHang.ID)
                {
                    int SoLuongTungBan = tempCTBCBH.SoLuong;
                    ICollection<BaoCaoBanHang> baoCaoBanHangs = db.BaoCaoBanHangs.Where(bcbh => bcbh.TinhTrang == true && bcbh.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach (var bcbhang in baoCaoBanHangs)
                    {
                        db.Entry<BaoCaoBanHang>(bcbhang).Collection(bcbh => bcbh.CT_BaoCaoBanHang).Load();
                        foreach (var ctbcbh in bcbhang.CT_BaoCaoBanHang)
                        {
                            if (ctbcbh.MaSach == tempCTBCBH.MaSach)
                            {
                                SoLuongTungBan += ctbcbh.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongXuat = 0;
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach (var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                        foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                        {
                            if (ctpx.MaSach == tempCTBCBH.MaSach)
                            {
                                SoLuongXuat += ctpx.SoLuong;
                            }
                        }
                    }
                    if (SoLuongXuat < SoLuongTungBan + cT_BaoCaoBanHang.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong báo cáo bán hàng vượt quá số sách mà đại lí mua.";
                        ViewBag.MaBC = tempCTBCBH.MaBC;
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuXuat> phieuXuat1s = db.PhieuXuats.Where(px => px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio <= baoCaoBanHang.NgayGio).ToList();
                        foreach (var phieuXuat in phieuXuat1s)
                        {
                            db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                            foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpx.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpx.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        return View(cT_BaoCaoBanHang);
                    }
                    db.Entry(tempCTBCBH).State = EntityState.Modified;
                    baoCaoBanHang.TongTien -= tempCTBCBH.ThanhTien;
                    tempCTBCBH.SoLuong += cT_BaoCaoBanHang.SoLuong;
                    tempCTBCBH.GiaBan = cT_BaoCaoBanHang.GiaBan;
                    baoCaoBanHang.TongTien += tempCTBCBH.ThanhTien;
                    CT_BaoCaoBanHang deletedCTBCBH = db.CT_BaoCaoBanHang.Find(cT_BaoCaoBanHang.ID);
                    db.CT_BaoCaoBanHang.Remove(deletedCTBCBH);
                }
                else
                {
                    CT_BaoCaoBanHang oldBaoCaoBanHang = db.CT_BaoCaoBanHang.Find(cT_BaoCaoBanHang.ID);
                    int SoLuongTungBan = 0;
                    baoCaoBanHang.TongTien -= oldBaoCaoBanHang.ThanhTien;
                    ICollection<BaoCaoBanHang> baoCaoBanHangs = db.BaoCaoBanHangs.Where(bcbh => bcbh.TinhTrang == true && bcbh.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach (var bcbhang in baoCaoBanHangs)
                    {
                        db.Entry<BaoCaoBanHang>(bcbhang).Collection(bcbh => bcbh.CT_BaoCaoBanHang).Load();
                        foreach (var ctbcbh in bcbhang.CT_BaoCaoBanHang)
                        {
                            if (ctbcbh.MaSach == cT_BaoCaoBanHang.MaSach)
                            {
                                SoLuongTungBan += ctbcbh.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongXuat = 0;
                    ICollection<PhieuXuat> phieuXuats = db.PhieuXuats.Where(px => px.TinhTrang == true && px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio < baoCaoBanHang.NgayGio).ToList();
                    foreach (var phieuXuat in phieuXuats)
                    {
                        db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                        foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                        {
                            if (ctpx.MaSach == cT_BaoCaoBanHang.MaSach)
                            {
                                SoLuongXuat += ctpx.SoLuong;
                                break;
                            }
                        }
                    }
                    if (SoLuongXuat < SoLuongTungBan + cT_BaoCaoBanHang.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong báo cáo bán hàng vượt quá số sách mà đại lí mua.";
                        ViewBag.MaBC = cT_BaoCaoBanHang.MaBC;
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuXuat> phieuXuat1s = db.PhieuXuats.Where(px => px.MaDaiLy == baoCaoBanHang.MaDaiLy && px.NgayGio <= baoCaoBanHang.NgayGio).ToList();
                        foreach (var phieuXuat in phieuXuat1s)
                        {
                            db.Entry<PhieuXuat>(phieuXuat).Collection(px => px.CT_PhieuXuat).Load();
                            foreach (var ctpx in phieuXuat.CT_PhieuXuat)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpx.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpx.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        return View(cT_BaoCaoBanHang);
                    }
                    baoCaoBanHang.TongTien += cT_BaoCaoBanHang.ThanhTien;
                    CT_BaoCaoBanHang updatedBaoCaoBanHang = db.CT_BaoCaoBanHang.Find(cT_BaoCaoBanHang.ID);
                    updatedBaoCaoBanHang.SoLuong = cT_BaoCaoBanHang.SoLuong;
                    updatedBaoCaoBanHang.GiaBan = cT_BaoCaoBanHang.GiaBan;
                    updatedBaoCaoBanHang.MaSach = cT_BaoCaoBanHang.MaSach;
                    db.Entry(updatedBaoCaoBanHang).State = EntityState.Modified;
                }
                db.Entry<BaoCaoBanHang>(baoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "BaoCaoBanHangs", new { id = cT_BaoCaoBanHang.MaBC });
            }
            ViewBag.MaBC = cT_BaoCaoBanHang.MaBC;
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
            return RedirectToAction("Details", "BaoCaoBanHangs", new { id = cT_BaoCaoBanHang.MaBC });
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
