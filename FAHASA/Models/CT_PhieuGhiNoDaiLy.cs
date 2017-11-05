using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_PhieuGhiNoDaiLy")]
    public class CT_PhieuGhiNoDaiLy
    {
        public CT_PhieuGhiNoDaiLy()
        {
            
        }
        [Key]
        public int ID { get; set; }

        public int DonGiaXuat { get; set; }

        public int SoLuongTon {
            get
            {
                return SoLuongDaXuat - SoLuongBanDuoc;
            }
            private set
            {

            }
        }

        public int TienNo {
            get
            {
                return SoLuongTon * DonGiaXuat;
            }
            private set {

            }
        }

        public int SoLuongDaXuat { get; set; }

        public int SoLuongBanDuoc { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("PhieuGhiNoDaiLy")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public PhieuGhiNoDaiLy PhieuGhiNoDaiLy { get; set; }

        public Sach Sach { get; set; }
    }
}