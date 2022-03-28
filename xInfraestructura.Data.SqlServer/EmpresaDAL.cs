using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infraestructura.Data.SqlServer
{
    public class EmpresaDAL:Conexion
    {
        public List<EmpresaEN> ListarEmpresa()
        {
            List<EmpresaEN> lista = new List<EmpresaEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListarEmpresa");
                while (lector.Read())
                {
                    EmpresaEN objE = new EmpresaEN();
                    objE.IdEmp = lector.GetInt32(0);
                    objE.RazonSocial = lector.GetString(1);
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
