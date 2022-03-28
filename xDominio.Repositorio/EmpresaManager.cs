using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class EmpresaManager
    {
        private EmpresaDAL objDAL;

        public List<EmpresaEN> ListarEmpresa()
        {           
            try
            {
                objDAL = new EmpresaDAL();
                return objDAL.ListarEmpresa();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<EmpresaEN>();
            }
        }
    }
}
