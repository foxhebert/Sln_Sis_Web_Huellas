using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class MotivoEN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione un motivo")]
        public int IdMotivo { get; set; }

        public string DescMotivo { get; set; }
    }
}
