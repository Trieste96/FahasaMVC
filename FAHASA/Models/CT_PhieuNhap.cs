using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_PhieuNhap")]
    public class CT_PhieuNhap
    {
        public CT_PhieuNhap()
        {

        }

        [Key]
        public int ID { get; set; }

        public int DonGiaNhap { get; set; }

        public int SoLuong { get; set; }

        public int ThanhTien {
            get { return DonGiaNhap * SoLuong; }
            private set { }
        }

        [Index(IsUnique = true)]
        [ForeignKey("PhieuNhap")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public PhieuNhap PhieuNhap { get; set; }

        public Sach Sach { get; set; }
    }
}