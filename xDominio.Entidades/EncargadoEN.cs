using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class EncargadoEN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione un responsable")]
        public int IdEnc { get; set; }

        public string Nombre { get; set; }

        public string correo { get; set; }
    }
}
