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
    
    public partial class BaoCaoBanHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaoCaoBanHang()
        {
            this.TinhTrang = true;
            this.CT_BaoCaoBanHang = new HashSet<CT_BaoCaoBanHang>();
        }
    
        public int MaBC { get; set; }
        public System.DateTime NgayGio { get; set; }
        public int TongTien { get; set; }
        public int MaDaiLy { get; set; }
        public bool TinhTrang { get; set; }
    
        public virtual DaiLy DaiLy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_BaoCaoBanHang> CT_BaoCaoBanHang { get; set; }
    }
}
