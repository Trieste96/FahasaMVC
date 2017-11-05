using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("ThongKeChoNXB")]
    public class ThongKeChoNXB
    {
        public ThongKeChoNXB()
        {
            this.TinhTrang = true;
            this.TongTien = 0;
        }
        [Key]
        public int MaPhieu { get; set; }
        public DateTime NgayGio { get; set; }
        public int TongTien { get; set; }
        public bool TinhTrang { get; set; }
        [ForeignKey("NhaXuatBan")]
        public int MaNXB { get; set; }
        public NhaXuatBan NhaXuatBan { get; set; }
        public ICollection<CT_ThongKeChoNXB> CTThongKeChoNXBs { get; set; }
    }
}