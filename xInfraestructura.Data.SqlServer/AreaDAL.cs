using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Data.SqlServer
{
   public class AreaDAL:Conexion
    {
        public List<AreasEN> ListarAreas()
        {
            List<AreasEN> lista = new List<AreasEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarAreas");
                while (lector.Read())
                {
                    AreasEN objM = new AreasEN();
                    objM.IdArea = lector.GetInt32(0);
                    objM.descArea = lector.GetString(1);
                    lista.Add(objM);
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
