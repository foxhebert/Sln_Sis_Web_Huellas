using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class MotivoManager
    {
        private MotivoDAL objDAL;

        public List<MotivoEN> ListarMotivo()
        {
            try
            {
                objDAL = new MotivoDAL();
                return objDAL.ListarMotivo();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<MotivoEN>();
            }
            
        }
    }
}
