using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Dominio.Entidades
{
    public class TEXTOCORREO
    {
        [DataMember] public string saludo { get; set; }
        [DataMember] public string despedida { get; set; }
        [DataMember] public string texto1 { get; set; }
        [DataMember] public string texto2 { get; set; }
        [DataMember] public string texto3 { get; set; }
        [DataMember] public string texto4 { get; set; }
        [DataMember] public string texto5 { get; set; }
        [DataMember] public string pie1 { get; set; }
        [DataMember] public string pie2 { get; set; }
        [DataMember] public string pie3 { get; set; }
    }
}
