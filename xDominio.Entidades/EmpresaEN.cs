using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class EmpresaEN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione una Empresa")]
        public int IdEmp { get; set; }

        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
    }
}
