using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infraestructura.Data.SqlServer
{
   public class ReporteDAL:Conexion
    {
        public List<TicketEN> ReporteProgresoBarras(DateTime dateStart, DateTime dateEnd,int idResponsable)
        {
            List<TicketEN> reporteBarra = new List<TicketEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ReporteProgresoBarras", dateStart.ToString("yyyyMMdd"), dateEnd.ToString("yyyyMMdd"),idResponsable);
                while (lector.Read())
                {
                    TicketEN ticketEN = new TicketEN();
                    DetalleTicketEN detalleTicketEN = new DetalleTicketEN();
                    ticketEN.Nro = lector.GetInt32(0);
                    detalleTicketEN.progreso = lector.GetInt32(1);
                    ticketEN.detalle = detalleTicketEN;                    
                    reporteBarra.Add(ticketEN);
                }
                return reporteBarra;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TicketEN> ReporteAvanceBarras(DateTime dateStart, DateTime dateEnd, int idResponsable,int idArea)
        {
            List<TicketEN> reporteBarra = new List<TicketEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ReporteAvanceBarras", dateStart.ToString("yyyyMMdd"), dateEnd.ToString("yyyyMMdd"), idResponsable,idArea);
                while (lector.Read())
                {
                    TicketEN ticketEN = new TicketEN();
                    DetalleTicketEN detalleTicketEN = new DetalleTicketEN();
                    EstadoEN objEstado = new EstadoEN();
                    EncargadoEN objEnc = new EncargadoEN();

                    ticketEN.Nro = lector.GetInt32(0);
                    detalleTicketEN.progreso = lector.GetInt32(1);
                    objEstado.IdEstado = lector.GetInt32(2);
                    objEstado.DesEstado = lector.GetString(3);
                    ticketEN.cantSoporte = lector.GetInt32(4);
                    objEnc.Nombre = lector.GetString(5);

                    detalleTicketEN.estado = objEstado;
                    detalleTicketEN.encargado = objEnc;
                    ticketEN.detalle = detalleTicketEN;
                    reporteBarra.Add(ticketEN);
                }
                return reporteBarra;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstadoEN> ReporteEstadoDonuts(DateTime dateStart, DateTime dateEnd,int idResponsable,int idArea)
        {
            List<EstadoEN> reporteDonuts = new List<EstadoEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ReporteEstadoDonuts", dateStart.ToString("yyyyMMdd"), dateEnd.ToString("yyyyMMdd"),idResponsable,idArea);
                while (lector.Read())
                {
                    EstadoEN estadoEN = new EstadoEN();
                    estadoEN.IdEstado = lector.GetInt32(0);
                    estadoEN.DesEstado = lector.GetString(2);
                    reporteDonuts.Add(estadoEN);
                }
                return reporteDonuts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //REPORTE HISTORIAL TICKET
        public List<HistorialTicket> ReporteHistorialTickets(DateTime fecInicio, DateTime fecFin, int idEstado, int idArea,int idResp, bool includeFecReg)
        {
            List<HistorialTicket> lista = new List<HistorialTicket>();

            try
            {
                List<TicketEN> listaTicket = ListarHistorialTickets(fecInicio, fecFin, idEstado, idArea,idResp, includeFecReg);

                if (listaTicket.Count > 0)
                {
                    var agrupar = listaTicket.GroupBy(x => new { Nro = x.Nro })
                                           .Select(g => new { Nro = g.Key.Nro })
                                           .OrderBy(z => z.Nro);

                    agrupar.ToList().ForEach(x =>
                    {
                        HistorialTicket objHisto = new HistorialTicket();
                        objHisto.Nro = x.Nro;                        
                        var subGrupo = listaTicket.Where(y => y.Nro == x.Nro).OrderBy(z => z.detalle.Id);

                        objHisto.tickets = subGrupo.ToList();
                        objHisto.tiempoDura = subGrupo.ToList().FirstOrDefault().TiempoDuracion;

                        lista.Add(objHisto);
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<TicketEN> ListarHistorialTickets(DateTime fecInicio, DateTime fecFin, int intEstado, int idArea, int idResp,bool includeFecReg)
        {
            List<TicketEN> lista = new List<TicketEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarHistorialTickets", fecInicio.ToString("yyyyMMdd"), fecFin.ToString("yyyyMMdd"), intEstado, idArea, idResp,includeFecReg);
                while (lector.Read())
                {
                    TicketEN objTicket = new TicketEN();
                    DetalleTicketEN objDetalle = new DetalleTicketEN();
                    EstadoEN objEstado = new EstadoEN();
                    EncargadoEN objEncargado = new EncargadoEN();

                    objTicket.Nro = lector.GetInt32(0);
                    objDetalle.Id = lector.GetInt32(1);
                    objTicket.empresa = lector.GetString(2);
                    objDetalle.FechaRegistro = lector.GetDateTime(3);
                    objDetalle.progreso = lector.GetInt32(4);
                    objEstado.IdEstado = lector.GetInt32(5);
                    objEstado.DesEstado = lector.GetString(6);
                    objEncargado.Nombre = lector.GetString(7);
                    objTicket.TiempoDuracion = lector.GetString(8);

                    objDetalle.encargado = objEncargado;
                    objDetalle.estado = objEstado;
                    objTicket.detalle = objDetalle;

                    lista.Add(objTicket);
                }
                lector.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
