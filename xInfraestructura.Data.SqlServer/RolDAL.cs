using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infraestructura.Data.SqlServer
{
    public class RolDAL:Conexion
    {
        public List<RolEN> ListarRol()
        {
            List<RolEN> lista = new List<RolEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarRol");
                while (lector.Read())
                {
                    RolEN objR = new RolEN();
                    objR.IdRol = lector.GetInt32(0);
                    objR.DesRol = lector.GetString(1);
                    lista.Add(objR);
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
