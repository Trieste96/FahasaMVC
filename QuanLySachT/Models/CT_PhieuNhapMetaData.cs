using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuanLySachT.Models
{
    [MetadataType(typeof(CT_PhieuNhap.CT_PhieuNhapMetaData))]
    public partial class CT_PhieuNhap
    {
        private class CT_PhieuNhapMetaData
        {
            [Remote(
            "doesProductNameExistUnderCategory",
            "CT_PhieuNhap",
            AdditionalFields = "MaPhieu,MaSach",
            ErrorMessage = "Sản phẩm này đã có trong danh sách. Vui lòng nhập lại",
            HttpMethod = "POST"
        )]
            public int DonGiaNhap { get; set; }
            public int MaPhieu { get; set; }
            public int MaSach { get; set; }
        }
    }
}