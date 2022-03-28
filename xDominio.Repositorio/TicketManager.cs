using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class TicketManager
    {
        private TicketsDAL objDAL;

        public List<TicketEN> ListarTicketsGenerados(int nroTck, DateTime fecInicio, DateTime fecFin, int usu, int intEstado,int idArea)
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.ListarTicketsGenerados(nroTck, fecInicio, fecFin, usu, intEstado, idArea);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<TicketEN>();
            }
           
        }

        public List<TicketEN> BuscarTicket(int nroTck, bool add, int usu)
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.BuscarTicket(nroTck, add, usu);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<TicketEN>();
            }
            
        }

        public int InsertarNuevoTicket(TicketEN objTicket, int IdUsuCrea, ref int nroRegis)
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.InsertarNuevoTicket(objTicket, IdUsuCrea, ref nroRegis);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return -1;
            }
            
        }

        public int InsertarDetalleTicket(TicketEN objTicket, ref string fechaRegistrada)
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.InsertarDetalleTicket(objTicket, ref fechaRegistrada);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return -1;
            }
           
        }

        public int ConsultarCorrelativo()
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.ConsultarCorrelativo();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return -1;
            }
            
        }

        public int CambiarEstadoTicket(int nro,string obser)
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.CambiarEstadoTicket(nro, obser);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return -1;
            }
            
        }

        public TicketEN datosEmail(int nro)
        {
            try
            {
                objDAL = new TicketsDAL();
                return objDAL.datosEmail(nro);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return null;
            }
           
        }
       

    }
}
