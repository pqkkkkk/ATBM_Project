﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.DataAccess.MetaData.TableView
{
    public interface ITableViewDao
    {
        List<OracleObject> getAllTableView();
    }
}
