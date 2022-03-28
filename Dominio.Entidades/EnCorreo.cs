using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class EnCorreo
    {
        public string strhost { get; set; }
        public string strpuerto { get; set; }
        public string strccorreo { get; set; }
        public string strcpass { get; set; }
        public string strremitente { get; set; }
        public string strEmail { get; set; }
        public string strCliente { get; set; }
        public string strTipoDocumento { get; set; }
        public string strSerie { get; set; }
        public string strNumero { get; set; }
        public string strTipoMoneda { get; set; }
        public string strImporteTotal { get; set; }
        public bool   bitAutentificacion { get; set; }
    }
}
