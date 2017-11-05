using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("Sach")]
    public class Sach
    {
        public Sach()
        {

        }

        [Key]
        public int MaSach { get; set; }

        [StringLength(50)]
        public string TenSach { get; set; }

        [StringLength(50)]
        public string LinhVuc { get; set; }

        public int GiaXuat { get; set; }
        public int GiaNhap { get; set; }

    }
}