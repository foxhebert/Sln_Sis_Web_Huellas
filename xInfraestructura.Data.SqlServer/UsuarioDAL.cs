using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infraestructura.Data.SqlServer
{
    public class UsuarioDAL:Conexion
    {
        public UsuarioEN loginUsuario(string usuario, string clave)
        {
            UsuarioEN objU = null;
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "InicioSesion", usuario, clave);
                if (lector.Read())
                {
                    objU = new UsuarioEN();
                    objU.IdUsu = lector.GetInt32(0);
                    objU.Nombres = lector.GetString(1);
                    objU.ApePaterno = lector.GetString(2);
                    if (!lector.IsDBNull(3))
                    {
                        objU.ApeMaterno = lector.GetString(3);
                    }
                    objU.Usuario = lector.GetString(4);
                    objU.clave = lector.GetString(5);
                    RolEN objR = new RolEN();
                    objR.IdRol = lector.GetInt32(6);
                    EmpresaEN objE = new EmpresaEN();
                    objE.IdEmp = lector.GetInt32(7);
                    objE.RazonSocial = lector.GetString(8);
                    if (!lector.IsDBNull(9))
                    {
                        objU.imgUsu = (byte[])lector.GetValue(9);
                    }
                    objU.rol = objR;
                    objU.empresa = objE;
                }
                lector.Close();
                return objU;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ActualizarUsuario(UsuarioEN objU)
        {
            int update = 0;
            try
            {           
                SqlHelper.ExecuteNonQuery(cnx, "ActualizarUsuario", objU.IdUsu,
                                                                    objU.Nombres,
                                                                    objU.ApePaterno,
                                                                    objU.ApeMaterno,
                                                                    objU.Usuario,
                                                                    objU.clave,
                                                                    objU.rol.IdRol,
                                                                    objU.empresa.IdEmp,
                                                                    objU.imgUsu);
                update = 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return update;
        }
    }
}
