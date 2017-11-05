using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("BaoCaoBanHang")]
    public class BaoCaoBanHang
    {
        public BaoCaoBanHang()
        {
            this.TinhTrang = true;
            this.TongTien = 0;
        }
        [Key, Column(Order = 0)]
        public int MaBC { get; set; }
        public DateTime NgayGio { get; set; }
        public int TongTien { get; set; }
        public bool TinhTrang { get; set; }
        [ForeignKey("DaiLy")]
        public int MaDaiLy { get; set; }
        public ICollection<CT_BaoCaoBanHang> CTBaoCaoBanHangs { get; set; } 
        public virtual DaiLy DaiLy { get; set; }
    }
}