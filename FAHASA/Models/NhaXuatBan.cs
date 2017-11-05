using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("NhaXuatBan")]
	public class NhaXuatBan
	{
        public NhaXuatBan()
        {

        }
        [Key]
        public int MaNXB { get; set; }
        [StringLength(50)]
        public string TenNXB { get; set; }
        [StringLength(50)]
        public string DiaChi { get; set; }
        [StringLength(15)]
        public string SoDienThoai { get; set; }
	}
}