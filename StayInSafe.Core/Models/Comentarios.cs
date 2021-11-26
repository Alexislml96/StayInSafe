using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Models
{
    public class Comentarios
    {
        public int Id_Comentario { get; set; }
        public string Comentario { get; set; }
        public int Calificacion { get; set; }
        public int Id_Sitio { get; set; }
    }
}
