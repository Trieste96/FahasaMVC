using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_PhieuYeuCauMua")]
    public class CT_PhieuYeuCauMua
    {
        public CT_PhieuYeuCauMua()
        {

        }

        [Key]
        public int ID { get; set; }

        public int SoLuong { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("PhieuYeuCauMua")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public PhieuYeuCauMua PhieuYeuCauMua { get; set; }

        public Sach Sach { get; set; }
    }
}