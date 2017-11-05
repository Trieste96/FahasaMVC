using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("PhieuYeuCauBan")]
    public class PhieuYeuCauBan
    {
        public PhieuYeuCauBan()
        {
            this.TinhTrang = true;
        }

        [Key]
        public int MaPhieu { get; set; }

        public DateTime NgayGio { get; set; }

        [ForeignKey("NhaXuatBan")]
        public int MaNXB { get; set; }

        public bool TinhTrang { get; set; }

        public NhaXuatBan NhaXuatBan { get; set; }

        public ICollection<CT_PhieuYeuCauBan> CTPhieuYeuCauBans { get; set; }
    }
}