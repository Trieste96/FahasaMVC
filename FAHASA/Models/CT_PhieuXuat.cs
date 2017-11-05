using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_PhieuXuat")]
    public class CT_PhieuXuat
    {
        public CT_PhieuXuat()
        {

        }

        [Key]
        public int ID { get; set; }

        public int DonGiaXuat { get; set; }

        public int ThanhTien {
            get
            {
                return DonGiaXuat * SoLuong;
            }
            private set
            {

            }
        }

        public int SoLuong { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("PhieuXuat")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public PhieuXuat PhieuXuat { get; set; }

        public Sach Sach { get; set; }
    }
}