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
    
    public partial class DaiLy
    {
        public DaiLy()
        {
            this.BaoCaoBanHangs = new HashSet<BaoCaoBanHang>();
            this.PhieuXuats = new HashSet<PhieuXuat>();
            this.PhieuYeuCauMuas = new HashSet<PhieuYeuCauMua>();
        }
    
        public int MaDaiLy { get; set; }
        public string TenDaiLy { get; set; }
        public Nullable<int> SoNo { get; set; }
    
        public virtual ICollection<BaoCaoBanHang> BaoCaoBanHangs { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuats { get; set; }
        public virtual ICollection<PhieuYeuCauMua> PhieuYeuCauMuas { get; set; }
    }
}
