using Alexis.CORE.Connection.Models;
using StayInSafe.Core.Configuration;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Services
{
    public static class FactorizeService
    {
        public static IUser Usuario(EServer typeServer)
        {
            return typeServer switch
            {
                EServer.UDEFINED => throw new NullReferenceException(),
                EServer.LOCAL => new UserService(BridgeDbConnection<Users>.Create(ConnectionStrings.LocalServer, DbEnum.Sql)),
                EServer.CLOUD => new UserService(BridgeDbConnection<Users>.Create(ConnectionStrings.CloudServer, DbEnum.Sql)),
                _ => throw new NullReferenceException(),
            };
        }

        public static ILogin Login(EServer typeServer)
        {
            return typeServer switch
            {
                EServer.UDEFINED => throw new NullReferenceException(),
                EServer.LOCAL => new LoginService(BridgeDbConnection<Users>.Create(ConnectionStrings.LocalServer, DbEnum.Sql)),
                EServer.CLOUD => new LoginService(BridgeDbConnection<Users>.Create(ConnectionStrings.CloudServer, DbEnum.Sql)),
                _ => throw new NullReferenceException(),
            };
        }

        public static IPassword Password(EServer typeServer)
        {
            return typeServer switch
            {
                EServer.UDEFINED => throw new NullReferenceException(),
                EServer.LOCAL => new PasswordService(BridgeDbConnection<LoginModel>.Create(ConnectionStrings.LocalServer, DbEnum.Sql)),
                EServer.CLOUD => new PasswordService(BridgeDbConnection<LoginModel>.Create(ConnectionStrings.CloudServer, DbEnum.Sql)),
                _ => throw new NullReferenceException(),
            };
        }

        public static IRefresh Refresh(EServer typerServer)
        {
            return typerServer switch
            {
                EServer.UDEFINED => throw new NullReferenceException(),
                EServer.LOCAL => new RefreshService(BridgeDbConnection<RefreshToken>.Create(ConnectionStrings.LocalServer, DbEnum.Sql)),
                EServer.CLOUD => new RefreshService(BridgeDbConnection<RefreshToken>.Create(ConnectionStrings.CloudServer, DbEnum.Sql)),
                _ => throw new NullReferenceException(),
            };
        }
    }
}
