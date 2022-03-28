using Dominio.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class UsuarioEN
    {
        public int IdUsu { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre Requerido")]
        public string Nombres { get; set; }

        public string ApePaterno { get; set; }

        public string ApeMaterno { get; set; }

        [Display(Name = "Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Usuario Requerido")]
        public string Usuario { get; set; }

        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña Requerida")]
        [DataType(DataType.Password)]
        public string clave { get; set; }

        [DataType(DataType.Password)]
        [Compare("clave", ErrorMessage = "La contraseña no coincide")]
        public string confirmClave { get; set; }

        public RolEN rol { get; set; }

        public EmpresaEN empresa { get; set; }
        public byte[] imgUsu { get; set; }
    }
}
