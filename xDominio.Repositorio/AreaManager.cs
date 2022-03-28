using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorio
{
    public class AreaManager
    {
        private AreaDAL objDal;

        public List<AreasEN> ListarAreas()
        {
            try
            {
                objDal = new AreaDAL();
                return objDal.ListarAreas();
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return new List<AreasEN>();
            }
        }
    }
}
