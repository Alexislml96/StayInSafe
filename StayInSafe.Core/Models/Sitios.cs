using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Models
{
    public class Sitios
    {
        public int? Id_Sitio { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        //public double CalificacionTotal => Comentarios.Where(x => x.Id_Sitio == Id_Sitio).Average(r => r.Calificacion);
    }
}
