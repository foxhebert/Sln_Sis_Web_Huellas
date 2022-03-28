using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class EstadoManager
    {
        private EstadoDAL objDAL;

        public List<EstadoEN> ListarEstado()
        {          
            try
            {
                objDAL = new EstadoDAL();
                return objDAL.ListarEstado();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<EstadoEN>();
            }
        }
    }
}
