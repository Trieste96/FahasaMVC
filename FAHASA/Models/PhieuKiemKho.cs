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
    
    public partial class PhieuKiemKho
    {
        public PhieuKiemKho()
        {
            this.CT_PhieuKiemKho = new HashSet<CT_PhieuKiemKho>();
        }
    
        public int MaPhieu { get; set; }
        public System.DateTime NgayGio { get; set; }
        public bool TinhTrang { get; set; }
    
        public virtual ICollection<CT_PhieuKiemKho> CT_PhieuKiemKho { get; set; }
    }
}
