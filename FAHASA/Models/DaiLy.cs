using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("DaiLy")]
    public class DaiLy
    {
        public DaiLy()
        {
            this.SoNo = 0;
        }
        [Key, Column(Order = 0)]
        public int MaDaiLy { get; set; }
        [StringLength(50)]
        public string TenDaiLy { get; set; }
        public int SoNo { get; set; }
        public ICollection<BaoCaoBanHang> BaoCaoBanHangs { get; set; }
    }
}