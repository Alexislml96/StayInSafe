using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Interfaces
{
    public interface IUser : IDisposable
    {
        long Register(Users user);
        bool UpdateUser(Users user);

        Users GetUser(long id);
    }
}
