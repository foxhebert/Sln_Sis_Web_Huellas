using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class EncargadoManager
    {
        private EncargadoDAL objDAL;

        public List<EncargadoEN> ListarEncargado()
        {
           
            try
            {
                objDAL = new EncargadoDAL();
                return objDAL.ListarEncargado();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<EncargadoEN>();
            }
        }

        public List<EncargadoEN> ListarEncargadoporArea(int idarea)
        {          
            try
            {
                objDAL = new EncargadoDAL();
                return objDAL.ListarEncargadoporArea(idarea);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<EncargadoEN>();
            }

        }
            
        
    }
}
