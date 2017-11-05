using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_PhieuYeuCauBan")]
    public class CT_PhieuYeuCauBan
    {
        public CT_PhieuYeuCauBan()
        {

        }

        [Key]
        public int ID { get; set; }

        public int SoLuong { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("PhieuYeuCauBan")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public PhieuYeuCauBan PhieuYeuCauBan { get; set; }

        public Sach Sach { get; set; }
    }
}