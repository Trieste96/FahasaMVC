//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FAHASA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CT_PhieuXuat
    {
        public int ID { get; set; }
        public int DonGiaXuat { get; set; }
        public Nullable<int> ThanhTien { get; set; }
        public int SoLuong { get; set; }
        public int MaPhieu { get; set; }
        public int MaSach { get; set; }
    
        public virtual PhieuXuat PhieuXuat { get; set; }
        public virtual Sach Sach { get; set; }
    }
}
