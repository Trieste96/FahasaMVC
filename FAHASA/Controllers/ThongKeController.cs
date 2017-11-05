using FAHASA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAHASA.Controllers
{
    public class ThongKeController : Controller
    {
        private FahasaContext db = new FahasaContext();

        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }
        // GET: ThongKeChoNXBs/Create
        public ActionResult CreateTongChi()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTongChi(DateTime NgayGio)
        {
            ICollection<ThongKeTongChi> ThongKes = new List<ThongKeTongChi>();
            ICollection<NhaXuatBan> NhaXuatBans = db.NhaXuatBans.ToList();
            foreach (var nxb in NhaXuatBans)
            {
                ThongKeTongChi thongke = new ThongKeTongChi();
                thongke.MaNXB = nxb.MaNXB;

                ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn => pn.TinhTrang == true && pn.NgayGio <= NgayGio && pn.MaNXB == nxb.MaNXB).ToList();
                int SoTienPhaiTra = 0;
                foreach (var phieuNhap in phieuNhaps)
                {
                    db.Entry<PhieuNhap>(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        SoTienPhaiTra += ctpn.ThanhTien;
                    }
                }
                thongke.SoTienPhaiTra = SoTienPhaiTra;
                ThongKes.Add(thongke);
            }
            ViewBag.ThongKes = ThongKes;
            return View();
        }
        // GET: ThongKeChoNXBs/Create
        public ActionResult CreateDoanhThu()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDoanhThu(DateTime TuNgayGio, DateTime DenNgayGio)
        {
            ICollection<DoanhThu> DoanhThus = new List<DoanhThu>();
            ICollection<Sach> Saches = db.Saches.ToList();
            int DoanhThu = 0;
            foreach (var sach in Saches)
            {
                DoanhThu doanhThu = new DoanhThu();
                doanhThu.MaSach = sach.MaSach;
                doanhThu.TenSach = sach.TenSach;
                int TongSoLuongNhap = 0;
                int TongGiaNhap = 0;
                ICollection<PhieuNhap> phieuNhaps = db.PhieuNhaps.Where(pn =>pn.TinhTrang == true && pn.NgayGio >= TuNgayGio && pn.NgayGio <= DenNgayGio).ToList();
                foreach (var phieuNhap in phieuNhaps)
                {
                    db.Entry(phieuNhap).Collection(pn => pn.CTPhieuNhaps).Load();
                    foreach (var ctpn in phieuNhap.CTPhieuNhaps)
                    {
                        if (ctpn.MaSach == sach.MaSach)
                        {
                            TongSoLuongNhap += ctpn.SoLuong;
                            TongGiaNhap += ctpn.ThanhTien;
                        }
                    }
                }
                doanhThu.SoLuongNhap = TongSoLuongNhap;
                doanhThu.TongGiaNhap = TongGiaNhap;
                int TongSoLuongBanDuoc = 0;
                int TongGiaBanDuoc = 0;
                ICollection<BaoCaoBanHang> baoCaoBanHangs = db.BaoCaoBanHangs.Where(bcbh => bcbh.TinhTrang == true && bcbh.NgayGio >= TuNgayGio && bcbh.NgayGio <= DenNgayGio).ToList();
                foreach (var baoCaoBanHang in baoCaoBanHangs)
                {
                    db.Entry(baoCaoBanHang).Collection(bcbh => bcbh.CTBaoCaoBanHangs).Load();
                    foreach (var ctbc in baoCaoBanHang.CTBaoCaoBanHangs)
                    {
                        if (ctbc.MaSach == sach.MaSach)
                        {
                            TongSoLuongBanDuoc = ctbc.SoLuong;
                            TongGiaBanDuoc = TongSoLuongBanDuoc * sach.GiaXuat;
                        }
                    }
                }
                doanhThu.SoLuongBanDuoc = TongSoLuongBanDuoc;
                doanhThu.TongGiaBanDuoc = TongGiaBanDuoc;
                DoanhThus.Add(doanhThu);
                DoanhThu += doanhThu.TamTinh;
            }
            ViewBag.DoanhThus = DoanhThus;
            return View();
        }
    }
}