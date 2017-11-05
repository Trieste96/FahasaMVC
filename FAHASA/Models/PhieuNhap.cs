using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("PhieuNhap")]
    public class PhieuNhap
    {
        public PhieuNhap()
        {
            this.TinhTrang = true;
        }

        [Key]
        public int MaPhieu { get; set; }

        [StringLength(50)]
        public string NguoiGiaoSach { get; set; }

        public DateTime NgayGio { get; set; }

        [ForeignKey("NhaXuatBan")]
        public int MaNXB { get; set; }

        [ForeignKey("NhanVien")]
        public int MaNhanVien { get; set; }

        public bool TinhTrang { get; set; }

        public NhaXuatBan NhaXuatBan { get; set; }

        public NhanVien NhanVien { get; set; }

        public ICollection<CT_PhieuNhap> CTPhieuNhaps { get; set; }
    }
}