using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_BaoCaoBanHang")]
    public class CT_BaoCaoBanHang
    {
        public CT_BaoCaoBanHang()
        {

        }
        [Key, Column(Order = 0)]
        public int ID { get; set; }
        [Index(IsUnique = true)]
        [ForeignKey("BaoCaoBanHang")]
        public int MaBC { get; set; }
        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }
        public int GiaBan { get; set; }
        public int SoLuong { get; set; }
        public int ThanhTien {
            get
            {
                return GiaBan * SoLuong;
            }
            private set
            {

            }
        }
        public BaoCaoBanHang BaoCaoBanHang { get; set; }
        public Sach Sach { get; set; }
    }
}