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
    
    public partial class PhieuNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhap()
        {
            this.CT_PhieuNhap = new HashSet<CT_PhieuNhap>();
        }
    
        public int MaPhieu { get; set; }
        public string NguoiGiaoSach { get; set; }
        public System.DateTime NgayGio { get; set; }
        public int MaNXB { get; set; }
        public int MaNhanVien { get; set; }
        public bool TinhTrang { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_PhieuNhap> CT_PhieuNhap { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        public virtual NhaXuatBan NhaXuatBan { get; set; }
    }
}
