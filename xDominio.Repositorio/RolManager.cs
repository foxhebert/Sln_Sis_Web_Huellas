using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class RolManager
    {
        private RolDAL objDAL;

        public List<RolEN> ListarRol()
        {
            try
            {
                objDAL = new RolDAL();
                return objDAL.ListarRol();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<RolEN>();
            }
            
        }
    }
}
