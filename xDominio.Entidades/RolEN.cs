using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class RolEN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione un rol")]
        public int IdRol { get; set; }

        public string DesRol { get; set; }
    }
}
