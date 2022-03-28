using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class ReporteManager
    {
        private ReporteDAL reporteDAL;
        public List<TicketEN> ReporteProgresoBarras(DateTime dateStart, DateTime dateEnd,int idResponsable)
        {
            try
            {
                reporteDAL = new ReporteDAL();
                return reporteDAL.ReporteProgresoBarras(dateStart, dateEnd, idResponsable);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<TicketEN>();
            }
           
        }
        public List<TicketEN> ReporteAvanceBarras(DateTime dateStart, DateTime dateEnd, int idResponsable, int idArea)
        {
            try
            {
                reporteDAL = new ReporteDAL();
                return reporteDAL.ReporteAvanceBarras(dateStart, dateEnd, idResponsable, idArea);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<TicketEN>();
            }
            
        }

        public List<EstadoEN> ReporteEstadoDonuts(DateTime dateStart, DateTime dateEnd, int idResponsable, int idArea)
        {
            try
            {
                reporteDAL = new ReporteDAL();
                return reporteDAL.ReporteEstadoDonuts(dateStart, dateEnd, idResponsable, idArea);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<EstadoEN>();
            }
            
        }
        public List<HistorialTicket> ReporteHistorialTickets(DateTime fecInicio, DateTime fecFin, int idEstado, int idArea,int idResp, bool includeFecReg)
        {
            try
            {
                reporteDAL = new ReporteDAL();
                return reporteDAL.ReporteHistorialTickets(fecInicio, fecFin, idEstado, idArea, idResp, includeFecReg);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<HistorialTicket>();
            }
           
        }

    }
}
