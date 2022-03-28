using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infraestructura.Data.SqlServer
{
    public class EncargadoDAL: Conexion
    {
        public List<EncargadoEN> ListarEncargado()
        {
            List<EncargadoEN> lista = new List<EncargadoEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarEncargado");
                while (lector.Read())
                {
                    EncargadoEN objE = new EncargadoEN();
                    objE.IdEnc = lector.GetInt32(0);
                    objE.Nombre = lector.GetString(1);
                    lista.Add(objE);
                }
                lector.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EncargadoEN> ListarEncargadoporArea(int idarea)
        {
            List<EncargadoEN> lista = new List<EncargadoEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarEncargado_porArea",idarea);
                while (lector.Read())
                {
                    EncargadoEN objE = new EncargadoEN();
                    objE.IdEnc = lector.GetInt32(0);
                    objE.Nombre = lector.GetString(1);
                    lista.Add(objE);
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
