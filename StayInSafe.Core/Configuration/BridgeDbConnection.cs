using Alexis.CORE.Connection;
using Alexis.CORE.Connection.Interfaces;
using Alexis.CORE.Connection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Configuration
{
    public  class BridgeDbConnection<T>
    {
        public static IConnectionDB<T> Create(string ConnectionString, DbEnum DB)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                return Factorizer<T>.Create(ConnectionString, DB);
            else
                return Factorizer<T>.Create(ConnectionString, DB);
        }
    }
}
