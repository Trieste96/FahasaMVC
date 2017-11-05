using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{

    [Table("PhieuGhiNoDaiLy")]
    public class PhieuGhiNoDaiLy
    {
        public PhieuGhiNoDaiLy()
        {
            this.TinhTrang = true;
            this.TongNo = 0;
        }
        [Key]
        public int MaPhieu { get; set; }

        public DateTime NgayGio { get; set; }

        public int TongNo { get; set; }

        public bool TinhTrang { get; set; }

        public ICollection<CT_PhieuGhiNoDaiLy> CTPhieuGhiNoDaiLies { get; set; }
    }
}