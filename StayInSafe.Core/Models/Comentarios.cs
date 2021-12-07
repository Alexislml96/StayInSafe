using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Models
{
    public class Comentarios
    {
        public long Id_Comentario { get; set; }
        public string Comentario { get; set; }
        public decimal Calificacion { get; set; }
        public int Id_Sitio { get; set; }
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
    }
}
