using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class PrioridadManager
    {
        private PrioridadDAL objDAL;

        public List<PrioridadEN> ListarPrioridad()
        {
            try
            {
                objDAL = new PrioridadDAL();
                return objDAL.ListarPrioridad();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<PrioridadEN>();
            }
           
        }
    }
}
