using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StayInSafe.Core.Models
{
    public class Users
    {
		public long Id { get; set; }
		public string? Email { get; set; }
		public string? Pass { get; set; }
		public string? P_Nombre { get; set; }
		public string? S_Nombre { get; set; }
		public string? Apellido_Paterno { get; set; }
		public string? Apellido_Materno { get; set; }
		public DateTime? Fecha_Nacimiento { get; set; }
		public string? Curp { get; set; }
		public decimal Telefono { get; set; }
		public string Imagen { get;set; }
	}
}
