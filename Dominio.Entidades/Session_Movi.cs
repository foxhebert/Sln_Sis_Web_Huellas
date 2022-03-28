using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Dominio.Entidades
{
    public class Session_Movi
    {

        [DataMember] public int intIdSesionMov { get; set; }
        [DataMember] public int intIdSesion { get; set; }
        [DataMember] public int intIdSoft { get; set; }
        [DataMember] public int intIdMenu { get; set; }
        [DataMember] public int intIdEntid { get; set; }
        [DataMember] public string strNomEntid { get; set; }
        [DataMember] public Int32 tinNuOpera { get; set; }
        [DataMember] public DateTime? dttFeRegistro { get; set; }
        [DataMember] public int intIdentity { get; set; }
        [DataMember] public int intIdUsuario { get; set; }
        [DataMember] public string strIpHost { get; set; }
    }
}
