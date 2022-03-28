using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SisWebHuellas.App_Start
{
    [DataContract]
    public class Respuesta
    {
        [DataMember] public bool exito { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string message { get; set; }
        [DataMember] public object entidad { get; set; }
    }
}