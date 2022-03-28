using Dominio.Entidades;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
   public class DetalleTicketEN
    {
        public int Id{get;set;}

        [Display(Name = "Fecha")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "se requiere Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro{get;set;}

        public string horaRegistro { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione una prioridad")]
        public PrioridadEN prioridad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione encargado")]
        public EncargadoEN encargado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione estado")]
        public EstadoEN estado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese una descripción")]
        public string descripcion { get; set; }

        public string adjunto { get; set; }
        public int progreso { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese una respuesta")]
        public string respuesta { get; set; }

        public string GuiaServicio { get; set; }

        public bool CartaConf { get; set; }

        public int flgSoporte { get; set; }
    }
}
