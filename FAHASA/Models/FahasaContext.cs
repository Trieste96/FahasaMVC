using FAHASA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace FAHASA.Models
{
    public class FahasaContext : DbContext
    {
        public FahasaContext() : base("FAHASA")
        { }

        public DbSet<Sach> Saches { get; set; }
        public DbSet<DaiLy> DaiLies { get; set; }
        public DbSet<BaoCaoBanHang> BaoCaoBanHangs { get; set; }
        public DbSet<CT_BaoCaoBanHang> CTBaoCaoBanHangs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer<FahasaContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }

        public System.Data.Entity.DbSet<FAHASA.Models.NhaXuatBan> NhaXuatBans { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.PhieuNhap> PhieuNhaps { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.NhanVien> NhanViens { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.PhieuXuat> PhieuXuats { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.PhieuGhiNoDaiLy> PhieuGhiNoDaiLies { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.PhieuKiemKho> PhieuKiemKhoes { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.PhieuYeuCauBan> PhieuYeuCauBans { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.PhieuYeuCauMua> PhieuYeuCauMuas { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.ThongKeChoNXB> ThongKeChoNXBs { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_PhieuGhiNoDaiLy> CT_PhieuGhiNoDaiLy { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_PhieuKiemKho> CT_PhieuKiemKho { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_PhieuNhap> CT_PhieuNhap { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_PhieuXuat> CT_PhieuXuat { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_PhieuYeuCauBan> CT_PhieuYeuCauBan { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_PhieuYeuCauMua> CT_PhieuYeuCauMua { get; set; }

        public System.Data.Entity.DbSet<FAHASA.Models.CT_ThongKeChoNXB> CT_ThongKeChoNXB { get; set; }
    }
}