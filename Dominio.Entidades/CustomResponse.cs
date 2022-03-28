using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class CustomResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string extramsg { get; set; }
        public object objeto { get; set; }

        public object objetoLista { get; set; }
    }
}
