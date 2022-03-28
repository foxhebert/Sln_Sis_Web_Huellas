using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace Infraestructura.Data.SqlServer
{
    public class MotivoDAL:Conexion
    {
        public List<MotivoEN> ListarMotivo()
        {
            List<MotivoEN> lista = new List<MotivoEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarMotivo");
                while (lector.Read())
                {
                    MotivoEN objM = new MotivoEN();
                    objM.IdMotivo = lector.GetInt32(0);
                    objM.DescMotivo = lector.GetString(1);
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
