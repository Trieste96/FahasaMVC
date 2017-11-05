using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("NhanVien")]
    public class NhanVien
    {
        public NhanVien()
        {

        }
        [Key]
        public int MaNhanVien { get; set; }
        [StringLength(50)]
        public string TenNhanVien { get; set; }

        public ICollection<PhieuNhap> PhieuNhaps { get; set; }
    }
}