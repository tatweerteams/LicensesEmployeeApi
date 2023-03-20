﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{

    public interface IUnitOfWorkDapper : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IsolationLevel IsolationLevel { get; }

        Action<string> Log { get; set; }

        bool Commit();
        void Rollback();
    }
}
