using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Interfaces
{
    public interface IContactos : IDisposable
    {
        long AddContact(Contactos contacto);
        IEnumerable<Contactos> GetContactsById(int id);
        void DeleteContact(int idContact);
    }
}
