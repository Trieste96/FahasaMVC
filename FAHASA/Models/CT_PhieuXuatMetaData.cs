using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FAHASA.Models
{
    [MetadataType(typeof(CT_PhieuXuat.CT_PhieuXuatMetaData))]
    public partial class CT_PhieuXuat
    {
        private class CT_PhieuXuatMetaData
        {
            [Remote(
        "doesProductNameExistUnderCategory",
        "CT_PhieuXuat",
        AdditionalFields = "MaPhieu,MaSach",
        ErrorMessage = "Sản phẩm này đã có trong danh sách. Vui lòng nhập lại",
        HttpMethod = "POST"
    )]
            public int DonGiaXuat { get; set; }
            public int MaPhieu { get; set; }
            public int MaSach { get; set; }
        }
    }
}