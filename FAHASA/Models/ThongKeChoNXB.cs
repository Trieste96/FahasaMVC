//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FAHASA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThongKeChoNXB
    {
        public ThongKeChoNXB()
        {
            this.CT_ThongKeChoNXB = new HashSet<CT_ThongKeChoNXB>();
        }
    
        public int MaPhieu { get; set; }
        public System.DateTime NgayGio { get; set; }
        public int TongTien { get; set; }
        public int MaNXB { get; set; }
        public bool TinhTrang { get; set; }
    
        public virtual ICollection<CT_ThongKeChoNXB> CT_ThongKeChoNXB { get; set; }
        public virtual NhaXuatBan NhaXuatBan { get; set; }
    }
}
