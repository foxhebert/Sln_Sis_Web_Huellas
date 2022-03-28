using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class EstadoEN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione un estado")]
        public int IdEstado { get; set; }

        public string DesEstado { get; set; }
    }
}
