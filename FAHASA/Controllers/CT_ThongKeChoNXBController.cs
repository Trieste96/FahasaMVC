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
    public class CT_ThongKeChoNXBController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: CT_ThongKeChoNXB
        public ActionResult Index()
        {
            var cT_ThongKeChoNXB = db.CT_ThongKeChoNXB.Include(c => c.Sach).Include(c => c.ThongKeChoNXB);
            return View(cT_ThongKeChoNXB.ToList());
        }

        // GET: CT_ThongKeChoNXB/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_ThongKeChoNXB cT_ThongKeChoNXB = db.CT_ThongKeChoNXB.Find(id);
            if (cT_ThongKeChoNXB == null)
            {
                return HttpNotFound();
            }
            return View(cT_ThongKeChoNXB);
        }

        // GET: CT_ThongKeChoNXB/Create
        public ActionResult Create(int id)
        {
            ViewBag.MaPhieu = id;
            ICollection<Sach> Saches = new List<Sach>(0);
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(id);
            ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
            foreach (var phieuNhap in phieuNhaps)
            {
                db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                {
                    int flag = 0;
                    foreach (var sach in Saches)
                    {
                        if (sach.MaSach == ctpn.MaSach)
                            break;
                        else
                            flag++;
                    }
                    if (flag == Saches.Count)
                    {
                        Saches.Add(db.Saches.Find(ctpn.MaSach));
                    }
                }
            }
            ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: CT_ThongKeChoNXB/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaPhieu,MaSach,GiaNhap,SoLuong")] CT_ThongKeChoNXB cT_ThongKeChoNXB)
        {
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(cT_ThongKeChoNXB.MaPhieu);
            if (ModelState.IsValid)
            {
                CT_ThongKeChoNXB tempCTTKCNXB = db.CT_ThongKeChoNXB.Where(cttkcnxb => cttkcnxb.MaPhieu == cT_ThongKeChoNXB.MaPhieu && cttkcnxb.MaSach == cT_ThongKeChoNXB.MaSach).FirstOrDefault();
                if(tempCTTKCNXB != null)
                {
                    int SoLuongTungThongKe = tempCTTKCNXB.SoLuong;
                    ICollection<ThongKeChoNXB> thongKeChoNXBs = db.ThongKeChoNXBs.Where(tkcnxb => tkcnxb.TinhTrang == true && tkcnxb.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var thongke in thongKeChoNXBs)
                    {
                        db.Entry<ThongKeChoNXB>(thongke).Collection(tkcnxb => tkcnxb.CTThongKeChoNXBs).Load();
                        foreach (var cttk in thongke.CTThongKeChoNXBs)
                        {
                            if(cttk.MaSach == tempCTTKCNXB.MaSach)
                            {
                                SoLuongTungThongKe += cttk.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongMua = 0;
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                        foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                        {
                            if (ctpn.MaSach == tempCTTKCNXB.MaSach)
                            {
                                SoLuongMua += ctpn.SoLuong;
                                break;
                            }
                        }
                    }
                    if (SoLuongMua < SoLuongTungThongKe + cT_ThongKeChoNXB.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong thống kê vượt quá số sách đã mua.";
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuNhap> phieuNhap1s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
                        foreach (var phieuNhap in phieuNhap1s)
                        {
                            db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                            foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpn.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpn.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
                        return View(cT_ThongKeChoNXB);
                    }
                    db.Entry(tempCTTKCNXB).State = EntityState.Modified;
                    thongKeChoNXB.TongTien -= tempCTTKCNXB.ThanhTien;
                    tempCTTKCNXB.SoLuong += cT_ThongKeChoNXB.SoLuong;
                    tempCTTKCNXB.GiaNhap = cT_ThongKeChoNXB.GiaNhap;
                    thongKeChoNXB.TongTien += tempCTTKCNXB.ThanhTien;
                }
                else
                {
                    int SoLuongTungThongKe = 0;
                    ICollection<ThongKeChoNXB> thongKeChoNXBs = db.ThongKeChoNXBs.Where(tkcnxb => tkcnxb.TinhTrang == true && tkcnxb.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach(var thongke in thongKeChoNXBs)
                    {
                        db.Entry<ThongKeChoNXB>(thongke).Collection(tkcnxb => tkcnxb.CTThongKeChoNXBs).Load();
                        foreach(var cttk in thongke.CTThongKeChoNXBs)
                        {
                            if(cttk.MaSach == cT_ThongKeChoNXB.MaSach)
                            {
                                SoLuongTungThongKe += cttk.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongMua = 0;
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                        foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                        {
                            if (ctpn.MaSach == cT_ThongKeChoNXB.MaSach)
                            {
                                SoLuongMua += ctpn.SoLuong;
                                break;
                            }
                        }
                    }
                    if (SoLuongMua < SoLuongTungThongKe + cT_ThongKeChoNXB.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong thống kê vượt quá số sách đã mua.";
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuNhap> phieuNhap1s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
                        foreach (var phieuNhap in phieuNhap1s)
                        {
                            db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                            foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpn.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpn.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
                        return View(cT_ThongKeChoNXB);
                    }
                    thongKeChoNXB.TongTien += cT_ThongKeChoNXB.ThanhTien;
                    db.CT_ThongKeChoNXB.Add(cT_ThongKeChoNXB);
                }
                db.Entry(thongKeChoNXB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ThongKeChoNXBs", new { id = cT_ThongKeChoNXB.MaPhieu });
            }

            ICollection<Sach> Sach1es = new List<Sach>(0);
            ICollection<PhieuNhap> phieuNhap2s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
            foreach (var phieuNhap in phieuNhap2s)
            {
                db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                {
                    int flag = 0;
                    foreach (var sach in Sach1es)
                    {
                        if (sach.MaSach == ctpn.MaSach)
                            break;
                        else
                            flag++;
                    }
                    if (flag == Sach1es.Count)
                    {
                        Sach1es.Add(db.Saches.Find(ctpn.MaSach));
                    }
                }
            }
            ViewBag.MaSach = new SelectList(Sach1es, "MaSach", "TenSach");
            ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
            return View(cT_ThongKeChoNXB);
        }

        // GET: CT_ThongKeChoNXB/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_ThongKeChoNXB cT_ThongKeChoNXB = db.CT_ThongKeChoNXB.Find(id);
            if (cT_ThongKeChoNXB == null)
            {
                return HttpNotFound();
            }
            ICollection<Sach> Saches = new List<Sach>(0);
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(cT_ThongKeChoNXB.MaPhieu);
            ICollection<PhieuNhap> phieuNhap1s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
            foreach (var phieuNhap in phieuNhap1s)
            {
                db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                {
                    int flag = 0;
                    foreach (var sach in Saches)
                    {
                        if (sach.MaSach == ctpn.MaSach)
                            break;
                        else
                            flag++;
                    }
                    if (flag == Saches.Count)
                    {
                        Saches.Add(db.Saches.Find(ctpn.MaSach));
                    }
                }
            }
            ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
            ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
            return View(cT_ThongKeChoNXB);
        }

        // POST: CT_ThongKeChoNXB/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaPhieu,MaSach,GiaNhap,SoLuong")] CT_ThongKeChoNXB cT_ThongKeChoNXB)
        {
            ThongKeChoNXB thongKeChoNXB = db.ThongKeChoNXBs.Find(cT_ThongKeChoNXB.MaPhieu);
            if (ModelState.IsValid)
            {
                CT_ThongKeChoNXB tempCTTKCNXB = db.CT_ThongKeChoNXB.Where(cttkcnxb => cttkcnxb.MaPhieu == cT_ThongKeChoNXB.MaPhieu && cttkcnxb.MaSach == cT_ThongKeChoNXB.MaSach).FirstOrDefault();
                if (tempCTTKCNXB != null && tempCTTKCNXB.ID != cT_ThongKeChoNXB.ID)
                {
                    int SoLuongTungThongKe = tempCTTKCNXB.SoLuong;
                    ICollection<ThongKeChoNXB> thongKeChoNXBs = db.ThongKeChoNXBs.Where(tkcnxb => tkcnxb.TinhTrang == true && tkcnxb.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var thongke in thongKeChoNXBs)
                    {
                        db.Entry<ThongKeChoNXB>(thongke).Collection(tkcnxb => tkcnxb.CTThongKeChoNXBs).Load();
                        foreach (var cttk in thongke.CTThongKeChoNXBs)
                        {
                            if (cttk.MaSach == tempCTTKCNXB.MaSach)
                            {
                                SoLuongTungThongKe += cttk.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongMua = 0;
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                        foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                        {
                            if (ctpn.MaSach == tempCTTKCNXB.MaSach)
                            {
                                SoLuongMua += ctpn.SoLuong;
                                break;
                            }
                        }
                    }
                    if (SoLuongMua < SoLuongTungThongKe + cT_ThongKeChoNXB.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong thống kê vượt quá số sách đã mua.";
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuNhap> phieuNhap1s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
                        foreach (var phieuNhap in phieuNhap1s)
                        {
                            db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                            foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpn.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpn.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
                        return View(cT_ThongKeChoNXB);
                    }
                    db.Entry(tempCTTKCNXB).State = EntityState.Modified;
                    thongKeChoNXB.TongTien -= tempCTTKCNXB.ThanhTien;
                    tempCTTKCNXB.SoLuong += cT_ThongKeChoNXB.SoLuong;
                    tempCTTKCNXB.GiaNhap = cT_ThongKeChoNXB.GiaNhap;
                    thongKeChoNXB.TongTien += tempCTTKCNXB.ThanhTien;
                    CT_ThongKeChoNXB deletedCTTKCNXB = db.CT_ThongKeChoNXB.Find(cT_ThongKeChoNXB.ID);
                    db.CT_ThongKeChoNXB.Remove(deletedCTTKCNXB);
                }
                else
                {
                    CT_ThongKeChoNXB oldCTTKCNXB = db.CT_ThongKeChoNXB.Find(cT_ThongKeChoNXB.ID);
                    int SoLuongTungThongKe = 0;
                    thongKeChoNXB.TongTien -= oldCTTKCNXB.ThanhTien;
                    ICollection<ThongKeChoNXB> thongKeChoNXBs = db.ThongKeChoNXBs.Where(tkcnxb => tkcnxb.TinhTrang == true && tkcnxb.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var thongke in thongKeChoNXBs)
                    {
                        db.Entry<ThongKeChoNXB>(thongke).Collection(tkcnxb => tkcnxb.CTThongKeChoNXBs).Load();
                        foreach (var cttk in thongke.CTThongKeChoNXBs)
                        {
                            if (cttk.MaSach == cT_ThongKeChoNXB.MaSach)
                            {
                                SoLuongTungThongKe += cttk.SoLuong;
                                break;
                            }
                        }
                    }
                    int SoLuongMua = 0;
                    ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio < thongKeChoNXB.NgayGio).ToList();
                    foreach (var phieuNhap in phieuNhaps)
                    {
                        db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                        foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                        {
                            if (ctpn.MaSach == cT_ThongKeChoNXB.MaSach)
                            {
                                SoLuongMua += ctpn.SoLuong;
                                break;
                            }
                        }
                    }
                    if (SoLuongMua < SoLuongTungThongKe + cT_ThongKeChoNXB.SoLuong)
                    {
                        ViewBag.ErrorMessage = "Số sách trong thống kê vượt quá số sách đã mua.";
                        ICollection<Sach> Saches = new List<Sach>(0);
                        ICollection<PhieuNhap> phieuNhap1s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
                        foreach (var phieuNhap in phieuNhap1s)
                        {
                            db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                            foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                            {
                                int flag = 0;
                                foreach (var sach in Saches)
                                {
                                    if (sach.MaSach == ctpn.MaSach)
                                        break;
                                    else
                                        flag++;
                                }
                                if (flag == Saches.Count)
                                {
                                    Saches.Add(db.Saches.Find(ctpn.MaSach));
                                }
                            }
                        }
                        ViewBag.MaSach = new SelectList(Saches, "MaSach", "TenSach");
                        ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
                        return View(cT_ThongKeChoNXB);
                    }
                    thongKeChoNXB.TongTien += cT_ThongKeChoNXB.ThanhTien;
                    CT_ThongKeChoNXB updatedCTTKCNXB = db.CT_ThongKeChoNXB.Find(cT_ThongKeChoNXB.ID);
                    updatedCTTKCNXB.SoLuong = cT_ThongKeChoNXB.SoLuong;
                    updatedCTTKCNXB.GiaNhap = cT_ThongKeChoNXB.GiaNhap;
                    updatedCTTKCNXB.MaSach = cT_ThongKeChoNXB.MaSach;
                    db.Entry(updatedCTTKCNXB).State = EntityState.Modified;
                }
                db.Entry(thongKeChoNXB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ThongKeChoNXBs", new { id = cT_ThongKeChoNXB.MaPhieu });
            }
            ICollection<Sach> Sach1es = new List<Sach>(0);
            ICollection<PhieuNhap> phieuNhap2s = db.PhieuNhaps.Where(pn => pn.MaNXB == thongKeChoNXB.MaNXB && pn.NgayGio <= thongKeChoNXB.NgayGio).ToList();
            foreach (var phieuNhap in phieuNhap2s)
            {
                db.Entry<PhieuNhap>(phieuNhap).Collection(px => px.CTPhieuNhaps).Load();
                foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                {
                    int flag = 0;
                    foreach (var sach in Sach1es)
                    {
                        if (sach.MaSach == ctpn.MaSach)
                            break;
                        else
                            flag++;
                    }
                    if (flag == Sach1es.Count)
                    {
                        Sach1es.Add(db.Saches.Find(ctpn.MaSach));
                    }
                }
            }
            ViewBag.MaSach = new SelectList(Sach1es, "MaSach", "TenSach");
            ViewBag.MaPhieu = cT_ThongKeChoNXB.MaPhieu;
            return View(cT_ThongKeChoNXB);
        }

        // GET: CT_ThongKeChoNXB/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_ThongKeChoNXB cT_ThongKeChoNXB = db.CT_ThongKeChoNXB.Find(id);
            if (cT_ThongKeChoNXB == null)
            {
                return HttpNotFound();
            }
            return View(cT_ThongKeChoNXB);
        }

        // POST: CT_ThongKeChoNXB/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_ThongKeChoNXB cT_ThongKeChoNXB = db.CT_ThongKeChoNXB.Find(id);
            db.CT_ThongKeChoNXB.Remove(cT_ThongKeChoNXB);
            db.SaveChanges();
            return RedirectToAction("Details","ThongKeChoNXBs",new { id = cT_ThongKeChoNXB.MaPhieu });
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
