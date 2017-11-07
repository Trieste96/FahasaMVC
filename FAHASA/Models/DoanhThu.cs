using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    public class DoanhThu
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongNhap { get; set; }
        public int TongGiaNhap { get; set; }
        public int SoLuongBanDuoc { get; set; }
        public int TongGiaBanDuoc { get; set; }
        public int TamTinh {
            get
            {
                return TongGiaBanDuoc - TongGiaNhap;
            }
            private set
            {

            }
        }
    }
}