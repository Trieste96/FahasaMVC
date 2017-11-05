using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_PhieuKiemKho")]
    public class CT_PhieuKiemKho
    {
        public CT_PhieuKiemKho()
        {

        }
        [Key]
        public int ID { get; set; }

        public int SoLuongTon { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("PhieuKiemKho")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public PhieuKiemKho PhieuKiemKho { get; set; }

        public Sach Sach { get; set; }
    }
}