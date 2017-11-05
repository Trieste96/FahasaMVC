using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("PhieuYeuCauMua")]
    public class PhieuYeuCauMua
    {
        public PhieuYeuCauMua()
        {
            this.TinhTrang = true;
        }

        [Key]
        public int MaPhieu { get; set; }

        public DateTime NgayGio { get; set; }

        [ForeignKey("DaiLy")]
        public int MaDaiLy { get; set; }

        public bool TinhTrang { get; set; }

        public DaiLy DaiLy { get; set; }

        public ICollection<CT_PhieuYeuCauMua> CTPhieuYeuCauMuas { get; set; }
    }
}