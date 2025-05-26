using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.DataAccess.ThongBao
{
    public interface IThongBaoDao: IBaseDao
    {
        public List<LabelComponent> GetAllLevels();
        public List<LabelComponent> GetAllDepartments();
        public List<LabelComponent> GetAllGroups();
        public bool SendNotification(string content, string label);

    }
}
