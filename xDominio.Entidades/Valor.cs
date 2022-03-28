using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Valor
    {
        public bool bitValor { get; set; }

        public string bitTexto { get; set; }
        public Valor(bool bitValor, string bitTexto)
        {
            this.bitValor = bitValor;
            this.bitTexto = bitTexto;
        }
    }
}
