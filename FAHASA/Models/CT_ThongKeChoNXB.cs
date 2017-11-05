using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    [Table("CT_ThongKeChoNXB")]
    public class CT_ThongKeChoNXB
    {
        public CT_ThongKeChoNXB()
        {

        }
        [Key]
        public int ID { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("ThongKeChoNXB")]
        public int MaPhieu { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public int GiaNhap { get; set; }

        public int SoLuong { get; set; }

        public int ThanhTien
        {
            get
            {
                return GiaNhap * SoLuong;
            }
            private set
            {

            }
        }

        public ThongKeChoNXB ThongKeChoNXB { get; set; }
        public Sach Sach { get; set; }
    }
}