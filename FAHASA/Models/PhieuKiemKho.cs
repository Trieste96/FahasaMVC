using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("PhieuKiemKho")]
    public class PhieuKiemKho
    {
        public PhieuKiemKho()
        {
            this.TinhTrang = true;
        }
        [Key]
        public int MaPhieu { get; set; }

        public DateTime NgayGio { get; set; }

        public bool TinhTrang { get; set; }

        public ICollection<CT_PhieuKiemKho> CTPhieuKiemKhoes { get; set; }
    }
}