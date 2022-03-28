using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace Infraestructura.Data.SqlServer
{
   public class PrioridadDAL:Conexion
    {
        public List<PrioridadEN> ListarPrioridad()
        {
            List<PrioridadEN> lista = new List<PrioridadEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarPrioridad");
                while (lector.Read())
                {
                    PrioridadEN objP = new PrioridadEN();
                    objP.IdPrio = lector.GetInt32(0);
                    objP.DesPrio = lector.GetString(1);
                    lista.Add(objP);
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
