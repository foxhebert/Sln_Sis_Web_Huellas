using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Infraestructura.Data.SqlServer
{
    public class EstadoDAL:Conexion
    {
        public List<EstadoEN> ListarEstado()
        {
            List<EstadoEN> lista = new List<EstadoEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarEstado");
                while (lector.Read())
                {
                    EstadoEN objE = new EstadoEN();
                    objE.IdEstado = lector.GetInt32(0);
                    objE.DesEstado = lector.GetString(1);
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
