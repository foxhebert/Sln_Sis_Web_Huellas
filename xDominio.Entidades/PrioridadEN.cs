using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class PrioridadEN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione una prioridad")]
        public int IdPrio { get; set; }

        public string DesPrio { get; set; }
    }
}
