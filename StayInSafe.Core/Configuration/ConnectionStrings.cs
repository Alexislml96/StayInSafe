using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Configuration
{
    public static class ConnectionStrings
    {
        public static string LocalServer = "workstation id=SuperLy.mssql.somee.com;packet size=4096;user id=Alexlml96_SQLLogin_1;pwd=c9k4se6ngs;data source=SuperLy.mssql.somee.com;persist security info=False;initial catalog=SuperLy";

        public static string CloudServer = "Server=tcp:mtwdm-alexis.database.windows.net,1433;Initial Catalog=core_3crud;Persist Security Info=False;User ID=Alexlml96;Password=Maestria_123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static string SecretKey = "SPCM8455ArgXtg8FUTr8fpnvdQ5f0jaYXvnUfcdEeNTJmQHO8GEcJFr9uOqlwDZO";

    }
}
