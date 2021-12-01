using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Interfaces
{
    public interface IComentario : IDisposable
    {
        long AddComment(Comentarios comentario);
        IEnumerable<Comentarios> GetCommentSito(int id);
    }
}
