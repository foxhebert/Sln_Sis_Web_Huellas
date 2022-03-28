using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class TicketEN
    {
        public int Nro { get; set; }

        [Display(Name = "Contacto")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "se requiere contacto")]
        public string contacto { get; set; }

        [Display(Name = "Empresa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "se requiere empresa")]
        public string empresa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "se requiere motivo")]
        public MotivoEN motivo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "se requiere detalle")]
        public DetalleTicketEN detalle { get; set; }

        //solo para el listado de tickest        
        public string TiempoDuracion { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public int cantSoporte { get; set; }
    }

    public class HistorialTicket
    {
        public int Nro { get; set; }
        public string tiempoDura { get; set; }
        public List<TicketEN> tickets { get; set; }
    }
}
