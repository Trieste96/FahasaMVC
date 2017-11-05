using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("PhieuXuat")]
    public class PhieuXuat
    {
        public PhieuXuat()
        {
            this.TongTien = 0;
            this.TinhTrang = true;
        }

        [Key]
        public int MaPhieu { get; set; }

        public DateTime NgayGio { get; set; }

        [StringLength(50)]
        public string NguoiNhan { get; set; }

        public int TongTien { get; set; }

        [ForeignKey("DaiLy")]
        public int MaDaiLy { get; set; }

        public bool TinhTrang { get; set; }

        public DaiLy DaiLy { get; set; }

        public ICollection<CT_PhieuXuat> CTPhieuXuats { get; set; }
    }
}