using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DataAccess.DangKy;
using Application.DataAccess.DonVi;
using Application.DataAccess.HocPhan;
using Application.DataAccess.MoMon;
using Application.DataAccess.NhanVien;
using Application.DataAccess.SinhVien;

namespace Application.ViewModels.User
{
    public class SVViewModel : INotifyPropertyChanged
    {
        public Dictionary<string, IBaseDao> daoList { get; set; }

        public SVViewModel()
        {
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKySVDao());
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonSVDao());
            daoList.Add("NhanVien", new NhanVienSVDao());
            daoList.Add("SinhVien", new SinhVienSVDao());
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
